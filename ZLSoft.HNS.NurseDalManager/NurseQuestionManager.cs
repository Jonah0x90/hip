using System;
using System.Collections.Generic;
using System.Linq;
using ZLSoft.Cache;
using ZLSoft.DalManager;
using ZLSoft.Model.HNS;
using ZLSoft.Model.PUB;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;

namespace ZLSoft.HNS.NurseDalManager
{
    public class NurseQuestionManager : CRUDManager
    {
        private const string KEY = "NurseQuestionManager";

        #region 构造

        private static NurseQuestionManager _instance = new NurseQuestionManager();
        public static NurseQuestionManager Instance
        {
            get
            {
                return NurseQuestionManager._instance;
            }
        }

        private NurseQuestionManager()
        {
        }

        #endregion

        public IList<NursingQuestion> LstNurseQuestion()
        {
            var cache = CacheIO.Get(KEY);
            if (cache == null)
            {
                var data = List<NursingQuestion>(null);
                CacheIO.Insert(KEY, data);
                return data;
            }
            return cache as IList<NursingQuestion>;
        }

        public void RemoveCache()
        {
            CacheIO.Remove(KEY);
        }

        public bool SvaeRelationship(string qId, object[] funIds)
        {
            var result = false;
            DB.BeginTransaction();
            try
            {
                this.Delete<QuestionRelation>(new { QuestionID = qId }.toStrObjDict());

                foreach (var id in funIds)
                {
                    this.InsertOrUpdate<QuestionRelation>(new { FunID = id, QuestionID = qId }.toStrObjDict());
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


        public IList<StrObjectDict> GetQuestionRelation(IDictionary<string, object> obj)
        {
            return DB.ListSod("LIST_QuestionRelation", obj);
        }

        public List<dynamic> GetQuestionRelationship(IDictionary<string, object> obj)
        {
            var result = DB.ListSod("LIST_QuestionRelation", obj);
            var objs = new List<dynamic>();
            var grouping = result.GroupBy(g => new
            {
                QuestionID = g.GetString("QID"),
                QuestionName = g.GetString("QNAME")
            });

            foreach (var item in grouping)
            {
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
                PlanTargetID = sod.GetString("FID"),
                Code = code,
                TargetName = sod.GetString("FNAME")
            };
        }



        public bool Delete(StrObjectDict obj)
        {
            var id = obj.GetString("ID");

            var batchStates = new DBState[] 
            { 
                new DBState(){
                    Name =  new NursingQuestion().MAP_DELETE,
                    Param = new{ ID = id}.toStrObjDict(),
                    Type = ESqlType.DELETE
                },
                new DBState(){
                    Name =  new NursingPlanDetails().MAP_DELETE,
                    Param = new{ QuestionID = id}.toStrObjDict(),
                    Type = ESqlType.DELETE
                },
                new DBState(){
                    Name = new QuestionRelation().MAP_DELETE,
                    Param = new{ QuestionID =id}.toStrObjDict(),
                    Type = ESqlType.DELETE
                }
            };
            return DB.ExecuteWithTransaction(batchStates) > 0;
        }

        public bool ExistReference(StrObjectDict sod)
        {
            var id = sod.GetString("ID");
            var nCount = DB.Count<NursingPlanDetails>(new { QuestionID = id });
            return nCount > 0;
        }

        public bool ExistsFourElement(StrObjectDict sod)
        {
            var questionIds = sod.GetString("QuestionIds").Split(',');
            var lst = DB.ListSod("GetElementCount",
                                        new { QuestionIds = questionIds }.toStrObjDict());
            //结果集包含4种元素，结果集长度最小为4
            if (lst == null || lst.Count < 4) 
                return false;

            var grouping = lst.GroupBy(a => a.GetString("ID"));
            //通过ID分组后，如果与入参个数不一致，表示有护理问题下面没有包含元素
            if (questionIds.Length != grouping.Count())
                return false;

            foreach (var item in grouping)
            {
                var cGrouping = item.GroupBy(a => a.GetString("Code"));
                if (cGrouping.Count() != 4)
                    return false;
            }

            return true;
        }
    }
}
