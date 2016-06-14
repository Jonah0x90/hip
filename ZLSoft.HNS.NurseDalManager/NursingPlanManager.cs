using System;
using System.Collections.Generic;
using System.Linq;
using ZLSoft.DalManager;
using ZLSoft.Model.HNS;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;
using ZLSoft.Pub.PageData;

namespace ZLSoft.HNS.NurseDalManager
{
    /// <summary>
    /// 护理计划
    /// </summary>
    public class NursingPlanManager : CRUDManager
    {

        #region 构造

        private static NursingPlanManager _instance = new NursingPlanManager();
        public static NursingPlanManager Instance
        {
            get
            {
                return NursingPlanManager._instance;
            }
        }

        private NursingPlanManager()
        {

        }

        #endregion

        public PageData<NursingPlan> List(IDictionary<string, object> sod, IDictionary<string, object> pageInfo)
        {
            var pageNumber = pageInfo.GetInt("PageNum").Value;
            var pageSize = pageInfo.GetInt("PageSize").Value;
            int listcount = DB.Count<NursingPlan>(sod);
            IList<NursingPlan> listData = DB.List<NursingPlan>(sod, pageNumber, pageSize);
            var lstDept = FormManager.Instance.HNS_GetDepartmentPersonnel(new { TypeID = 2 }.toStrObjDict());
            foreach (var item in listData)
            {
                var dicDept = new Dictionary<string, string>();
                if (item.DeptRange != null)
                {
                    foreach (var deptID in item.DeptRange.Split(','))
                    {
                        var dept = lstDept.FirstOrDefault(d => d.GetString("ID").Equals(deptID));
                        if (dept == null) continue;

                        dicDept.Add(deptID, dept.GetString("MC"));
                    }
                }
                item.DeptRange = dicDept.ToJson();
            }
            return new PageData<NursingPlan>(listData, pageNumber, pageSize, listcount);
        }

        public PageData<StrObjectDict> List1(IDictionary<string, object> sod, IDictionary<string, object> pageInfo)
        {
            var pageNumber = pageInfo.GetInt("PageNum").Value;
            var pageSize = pageInfo.GetInt("PageSize").Value;
            int listcount = DB.Count<NursingPlan>(sod);
            IList<NursingPlan> listData = DB.List<NursingPlan>(sod, pageNumber, pageSize);
            var lstDept = FormManager.Instance.HNS_GetDepartmentPersonnel(new { TypeID = 2 }.toStrObjDict());
            var lstDic = new List<StrObjectDict>();
            foreach (var item in listData)
            {
                var dicDept = new Dictionary<string, string>();
                if (item.DeptRange != null)
                {
                    foreach (var deptID in item.DeptRange.Split(','))
                    {
                        var dept = lstDept.FirstOrDefault(d => d.GetString("ID").Equals(deptID));
                        if (dept == null) continue;

                        dicDept.Add(deptID, dept.GetString("MC"));
                    }
                }
                item.DeptRange = dicDept.ToJson();

                var dic = item.toStrObjDict();

                var natureText = "";
                switch (item.Nature)
                {
                    case 1:
                        natureText = "按专科";
                        break;
                    case 2:
                        natureText = "按系统";
                        break;
                    case 3:
                        natureText = "按疾病";
                        break;
                    case 4:
                        natureText = "按护理诊断";
                        break;
                }
                dic.Add("KSSM", string.Format("由 {0} 创建于 {1}", item.ModifyUser,
                                                item.ModifyTime == null ? "" :
                                                item.ModifyTime.Value.ToString("yyyy-MM-dd HH:mm:ss")));
                dic.Add("NatureText", natureText);
                lstDic.Add(dic);
            }
            return new PageData<StrObjectDict>(lstDic, pageNumber, pageSize, listcount);
        }

        public List<dynamic> GetPlanDetails(IDictionary<string, object> obj)
        {
            var result = DB.ListSod("LIST_PlantDetails", obj);
            var objs = new List<dynamic>();
            var grouping = result.GroupBy(g => new
                                            {
                                                QuestionID = g.GetString("QUESTIONID"),
                                                QuestionName = g.GetString("QUESTIONNAME")
                                            });

            foreach (var item in grouping)
            {
                //var itemObjs = new List<dynamic>();
                var lstMb = new List<dynamic>();
                var lstYs = new List<dynamic>();
                var lstCs = new List<dynamic>();
                var lstPg = new List<dynamic>();
                foreach (var q in item)
                {
                    var code = q.GetString("CODE");
                    switch (code)
                    {
                        case "mb":
                            lstMb.Add(GetTarget(q, code));
                            break;
                        case "ys":
                            lstYs.Add(GetTarget(q, code));
                            break;
                        case "cs":
                            lstCs.Add(GetTarget(q, code));
                            break;
                        case "pg":
                            lstPg.Add(GetTarget(q, code));
                            break;
                    }
                }
                objs.Add(new
                {
                    Question = item.Key,
                    Targets = new { MB = lstMb, YS = lstYs, CS = lstCs, PG = lstPg }
                });
            }
            return objs;
        }

        private dynamic GetTarget(StrObjectDict sod, string code)
        {
            return new
                            {
                                PlanTargetID = sod.GetString("PLANTARGETID"),
                                Code = code,
                                TargetName = sod.GetString("TARGETNAME")
                            };
        }

        public bool Delete(StrObjectDict obj)
        {
            var batchStates = new DBState[] 
            { 
                new DBState(){
                    Name =  new NursingPlan().MAP_DELETE,
                    Param = new{ ID = obj.GetString("ID")},
                    Type = ESqlType.DELETE
                },
                new DBState(){
                    Name = new NursingPlanDetails().MAP_DELETE,
                    Param = new{ PlanID = obj.GetString("ID")},
                    Type = ESqlType.DELETE
                }
            };
            return DB.ExecuteWithTransaction(batchStates) > 0;
        }

        public bool SvaeRelationship(Dictionary<string, object> lst)
        {
            var result = false;
            DB.BeginTransaction();
            try
            {
                //修改计划信息
                var pData = lst["planData"] as Dictionary<string, object>;

                InsertOrUpdate<NursingPlan>(pData.toStrObjDict());

                //删除对应关系
                var planId = pData["ID"].ToString();

                DB.Execute(new DBState
                {
                    Name = "DELETE_NursingPlanDetails",
                    Param = new
                    {
                        PlanID = planId
                    }.toStrObjDict(),
                    Type = ESqlType.DELETE
                });

                //重新添加对应关系
                var data = lst["relationship"] as object[];

                foreach (var item in data)
                {
                    var dic = item as Dictionary<string, object>;
                    var qid = dic["qid"].ToString();
                    foreach (var keyValue in dic)
                    {
                        if (keyValue.Key.Equals("qid") || string.IsNullOrEmpty(keyValue.Value.ToString()))
                            continue;
                        SaveChanges(keyValue, qid, planId);
                    }
                }

                DB.Commit(DB.CurrentSqlMapper);
                result = true;
            }
            catch
            {
                DB.RollBackTransaction(DB.CurrentSqlMapper);
            }

            return result;
        }

        private void SaveChanges(KeyValuePair<string, object> keyValue, string qid, string planId)
        {
            var arr = keyValue.Value.ToString().Split(',');
            foreach (var id in arr.Distinct())
            {
                DB.Execute(new DBState
                {
                    Name = "INSERT_NursingPlanDetails",
                    Param = new
                    {
                        PlanID = planId,
                        QuestionID = qid,
                        PlanTargetID = id,
                        ModifyUser = "",
                        ModifyTime = DateTime.Now
                    }.toStrObjDict(),
                    Type = ESqlType.INSERT
                });
            }
        }
    }
}
