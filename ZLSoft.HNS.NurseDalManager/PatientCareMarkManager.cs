using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.DalManager;
using ZLSoft.Model.HNS;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;

namespace ZLSoft.HNS.NurseDalManager
{
    public class PatientCareMarkManager : CRUDManager
    {
        private static PatientCareMarkManager _instance = new PatientCareMarkManager();
        public static PatientCareMarkManager Instance
        {
            get
            {
                return PatientCareMarkManager._instance;
            }
        }

        public string Save(StrObjectDict sod, string userName)
        {
            var id = Guid.NewGuid().ToString();
            var mark = sod.GetObject("Mark") as Dictionary<string, object>;
            var details = sod.GetObject("Details") as object[];
            var time = DateTime.Now;
            mark.Add("MarkUser", userName);
            mark.Add("MarkTime", time);
            mark.Add("ModifyTime", time);
            List<DBState> batchStates = new List<DBState>();

            //包含ID键,则需要删除详细,然后在增加详细以及修改评分
            if (mark.ContainsKey("ID"))
            {
                //删除详细
                batchStates.Add(new DBState()
                {
                    Name = "DELETE_PatientMarkDetails",
                    Param = new { MarkID = mark.GetString("ID") }.toStrObjDict(),
                    Type = ESqlType.DELETE
                });
                //修改评分表
                batchStates.Add(
                 new DBState()
                 {
                     Name = "UPDATE_PatientCareMark",
                     Param = mark,
                     Type = ESqlType.UPDATE
                 });

                //增加评分详细 
                foreach (var item in details)
                {
                    batchStates.Add(new DBState()
                    {
                        Name = "INSERT_PatientMarkDetails",
                        Param = item as Dictionary<string, object>,
                        Type = ESqlType.INSERT
                    });
                }
                DB.ExecuteWithTransaction(batchStates);
                return "update";
            }
            else//添加评分以及详细
            {
                mark["ID"] = id;
                //增加评分
                batchStates.Add(new DBState()
                {
                    Name = "INSERT_PatientCareMark",
                    Param = mark,
                    Type = ESqlType.INSERT
                });

                //增加评分详细 
                foreach (var item in details)
                {
                    var dic = item as Dictionary<string, object>;
                    dic["MarkID"] = id;
                    batchStates.Add(new DBState()
                    {
                        Name = "INSERT_PatientMarkDetails",
                        Param = dic,
                        Type = ESqlType.INSERT
                    });
                }
                DB.ExecuteWithTransaction(batchStates);
                return id;
            }   
        }

        public string GetResultByScore(StrObjectDict sod)
        {
            var result = DB.Scalar("GetResultByScore", sod);
            return result == DBNull.Value || result == null ? string.Empty : result.ToString();
        }

        public bool Delelte(StrObjectDict sod)
        {
            return DB.ExecuteWithTransaction(new DBState[] { 
                new DBState{
                            Name =  "DELETE_PatientCareMark",
                            Param = sod,
                            Type = ESqlType.DELETE
                },
                new DBState{
                            Name =  "DELETE_PatientMarkDetails",
                            Param = new{MarkID=sod.GetString("ID")}.toStrObjDict(),
                            Type = ESqlType.DELETE
                }
            }) > 0;
        }
    }
}
