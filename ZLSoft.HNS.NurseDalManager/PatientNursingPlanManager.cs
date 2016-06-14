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
    public class PatientNursingPlanManager : CRUDManager
    {

        private static PatientNursingPlanManager _instance = new PatientNursingPlanManager();
        public static PatientNursingPlanManager Instance
        {
            get
            {
                return PatientNursingPlanManager._instance;
            }
        }

        /// <summary>
        /// 获取计划详细
        /// </summary>
        /// <returns></returns>
        public StrObjectDict GetSchemeDetails(StrObjectDict sod)
        {
            var details = DB.ListSod("GetSchemeDetails", sod);
            if (details.Any(d => string.IsNullOrEmpty(d.GetString("Code"))))
                return null;
            var group = details.GroupBy(g => new
            {
                Code = g["Code"].ToString()
            });

            var lstMb = new List<dynamic>();
            var lstYs = new List<dynamic>();
            var lstCs = new List<dynamic>();
            var lstPg = new List<dynamic>();

            foreach (var item in group)
            {
                foreach (var t in item)
                {
                    var code = item.Key.Code;
                    switch (code)
                    {
                        case "mb":
                            lstMb.Add(GetContent(t, code));
                            break;
                        case "ys":
                            lstYs.Add(GetContent(t, code));
                            break;
                        case "cs":
                            lstCs.Add(GetContent(t, code));
                            break;
                        case "pg":
                            lstPg.Add(GetContent(t, code));
                            break;
                    }
                }
            }

            if (!group.Any())
                return null;
            var eva = group.First().First();

            return new
            {
                QuestionType = eva["QuestionType"],
                Targets = new { MB = lstMb, YS = lstYs, CS = lstCs, PG = lstPg },//问题选项
                Evaluate = new  //评估
                {
                    ID = eva["EvaluateID"],
                    Evaluater = eva["Evaluater"],
                    EvaluateTime = eva["EvaluateTime"],
                    Content = eva["EvaluateContent"],
                    Status = eva["EvaluateStatus"],
                    SignID = eva["EvaluateSignID"]
                },
                Sign = new
                {
                    ID = eva["SignID"],
                    Nature = eva["SignStatus"],
                    Signer = eva["Signer"],
                    SignedTime = eva["SignedTime"]
                }
            }.toStrObjDict();
        }

        private dynamic GetContent(StrObjectDict sod, string code)
        {
            return new
            {
                Code = code,
                TargetContent = sod.GetString("TargetContent"),
                TargetID = sod.GetString("TargetID")
            };
        }

        /// <summary>
        /// 新增计划(保存病人护理计划、方案、方案明细)
        /// </summary>
        /// <param name="sod"></param>
        /// <returns></returns>
        public bool SaveNewPlan(StrObjectDict sod,  string createUser)
        {
            var batchStates = new List<DBState>();

            var planID = Guid.NewGuid().ToString();
            batchStates.Add(new DBState//病人计划保存
                 {
                     Name = "INSERT_PatientNursingPlan",
                     Param = new
                     {
                         ID = planID,
                         DeptID = sod.GetString("LesionID"),
                         ObjectID = sod.GetString("PatientID"),
                         ModifyUser = createUser,
                         ModifyTime = DateTime.Now,
                         PlanStandardID = "",
                         IsInvalid = 0
                     }.toStrObjDict(),
                     Type = ESqlType.INSERT
                 });

            foreach (var item in sod.GetObject("Questions") as object[])
            {
                var question = item as Dictionary<string, object>;
                var questionID = question["ID"].ToString();
                var schemeID = Guid.NewGuid().ToString();
                //计划方案保存
                batchStates.Add(new DBState
                {
                    Name = "INSERT_PatientScheme",
                    Type = ESqlType.INSERT,
                    Param = new
                    {
                        ID = schemeID,
                        PatientPlanID = planID,
                        QuestionID = questionID,
                        FormType = int.Parse(question["Type"].ToString()),
                        IsInvalid = 0
                    }.toStrObjDict()
                });

                #region =========     计划方案明细保存     =================

                var planStandardID = question.GetString("PlanStandardID");
                if (string.IsNullOrEmpty(planStandardID))//标准计划ID为空查询<问题_方法关联>
                {
                    var result = DB.ListSod("LIST_QuestionRelation", new { ID = questionID });
                    foreach (var details in result)
                    {
                        batchStates.Add(new DBState
                        {

                            Name = "INSERT_PatientSchemeDetails",
                            Param = new
                            {
                                SchemeID = schemeID,
                                Name = "",
                                Code = details["CODE"].ToString(),
                                Content = details["FNAME"].ToString(),
                                TargetID = details["FID"].ToString(),
                                IsCustom = 1,
                                ModifyTime = DateTime.Now,
                                ModifyUser = createUser
                            }.toStrObjDict(),
                            Type = ESqlType.INSERT
                        });
                    }
                }
                else//查询标准计划明细
                {
                    var result = DB.ListSod("LIST_PlantDetails",
                                            new
                                            {
                                                PlanID = planStandardID,
                                                QuestionID = questionID
                                            });

                    foreach (var details in result)
                    {
                        batchStates.Add(new DBState
                        {

                            Name = "INSERT_PatientSchemeDetails",
                            Param = new
                            {
                                SchemeID = schemeID,
                                Name = "",
                                Code = details["CODE"].ToString(),
                                Content = details["TARGETNAME"].ToString(),
                                TargetID = details["PLANTARGETID"].ToString(),
                                IsCustom = 1,
                                ModifyTime = DateTime.Now,
                                ModifyUser = createUser
                            }.toStrObjDict(),
                            Type = ESqlType.INSERT
                        });
                    }
                }

                #endregion
            }

            return DB.ExecuteWithTransaction(batchStates) > 0;
        }

        /// <summary>
        /// 删除计划方案 (逻辑删除)
        /// </summary>
        /// <returns></returns>
        public bool RemoveScheme(StrObjectDict sod)
        {
            sod["IsInvalid"] = 1;
            return this.InsertOrUpdate<PatientScheme>(sod) > 0;
        }

        /// <summary>
        /// 修改计划方案明细
        /// </summary>
        /// <returns></returns>
        public bool UpdateSchemeDetails(StrObjectDict sod, string user)
        {
            //先删除该计划方案下的明细，然后重新添加
            var batchStates = new List<DBState>();

            //删除计划方案下的明细
            var schemeID = sod.GetString("SchemeID");
            batchStates.Add(new DBState
            {
                Name = "DELETE_PatientSchemeDetails",
                Param = new { SchemeID = schemeID }.toStrObjDict(),
                Type = ESqlType.DELETE
            });

            //添加明细
            if (sod.GetObject("Targets") != null)
            {
                foreach (var item in sod.GetObject("Targets") as Dictionary<string, object>)
                {
                    foreach (var details in item.Value as object[])
                    {
                        var target = details as Dictionary<string, object>;
                        batchStates.Add(new DBState
                        {
                            Name = "INSERT_PatientSchemeDetails",
                            Param = new
                            {
                                SchemeID = schemeID,
                                Name = "",
                                Code = target["Code"].ToString(),
                                Content = target["TargetContent"].ToString(),
                                TargetID = target["TargetID"].ToString(),
                                IsCustom = 1,
                                ModifyTime = DateTime.Now,
                                ModifyUser = user
                            }.toStrObjDict(),
                            Type = ESqlType.INSERT
                        });
                    }
                }
            }

            return DB.ExecuteWithTransaction(batchStates) > 0;
        }

        /// <summary>
        /// 创建签名
        /// </summary>
        /// <returns></returns>
        public bool CreateSign(StrObjectDict sod, string userName)
        {
            var nature = sod.GetInt("Nature").Value;
            if (nature == 2)//评估签名 
            {
                var batchStates = new List<DBState>();

                if (sod.ContainsKey("Unsign") && sod.GetInt("Unsign") == 1) //取消评估签名
                {
                    //逻辑删除评估签名
                    batchStates.Add(new DBState
                    {
                        Name = "UPDATE_NursingSign",
                        Param = new
                        {
                            IsInvalid = 1,
                            ID = sod.GetString("SignID")
                        }.toStrObjDict(),
                        Type = ESqlType.UPDATE
                    });

                    //修改评估表签名ID字段
                    batchStates.Add(new DBState
                    {
                        Name = "UPDATE_NursingEvaluat",
                        Param = new
                        {
                            SignID = "",
                            ID = sod.GetString("SourceID")
                        }.toStrObjDict(),
                        Type = ESqlType.UPDATE
                    });
                }
                else
                {
                    var signID = Guid.NewGuid().ToString();
                    //创建签名
                    batchStates.Add(new DBState
                    {
                        Name = "INSERT_NursingSign",
                        Param = new NursingSign
                        {
                            ID = signID,
                            Nature = nature,
                            SourceID = sod.GetString("SourceID"),
                            Signer = userName,
                            SignedTime = DateTime.Now,
                            IsInvalid = 0
                        }.toStrObjDict(),
                        Type = ESqlType.INSERT
                    });
                    //修改评估表签名ID字段
                    batchStates.Add(new DBState
                    {
                        Name = "UPDATE_NursingEvaluat",
                        Param = new
                        {
                            SignID = signID,
                            ID = sod.GetString("SourceID")
                        }.toStrObjDict(),
                        Type = ESqlType.UPDATE
                    });
                }
                return DB.ExecuteWithTransaction(batchStates) > 0;
            }

            return this.InsertOrUpdate<NursingSign>(new
            {
                Nature = nature,
                SourceID = sod.GetString("SourceID"),
                Signer = userName,
                SignedTime = DateTime.Now,
                IsInvalid = 0
            }.toStrObjDict()) > 0;
        }

        /// <summary>
        /// 删除签名 (逻辑删除)
        /// </summary>
        /// <param name="sod"></param>
        /// <returns></returns>
        public bool RemogeSign(StrObjectDict sod)
        {
            sod["IsInvalid"] = 1;
            return this.InsertOrUpdate<NursingSign>(sod) > 0;
        }

        public bool CreateEvaluat(StrObjectDict sod, string user)
        {
            sod["Evaluator"] = user;
            sod["EvaluatTime"] = DateTime.Now;
            sod["IsInvalid"] = 0;
            return this.InsertOrUpdate<NursingEvaluat>(sod) > 0;
        }

        /// <summary>
        /// 删除评估(逻辑删除)
        /// </summary>
        /// <returns></returns>
        public bool RemoveEvaluat(StrObjectDict sod)
        {
            sod["IsInvalid"] = 1;
            return this.InsertOrUpdate<NursingEvaluat>(sod) > 0;
        }

        public IList<NursingEvaluat> GetEvaluatLst(StrObjectDict sod)
        {
            sod["IsInvalid"] = 0;
            return DB.List<NursingEvaluat>(sod);
        }
    }
}
