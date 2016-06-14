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
    public class NursingAssessmentManager : CRUDManager
    {
        #region 构造

        private static NursingAssessmentManager _instance = new NursingAssessmentManager();
        public static NursingAssessmentManager Instance
        {
            get
            {
                return NursingAssessmentManager._instance;
            }
        }
        private NursingAssessmentManager()
        {
        }

        #endregion

        /// <summary>
        /// 请求病人对应的 评估/宣教 表单列表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<StrObjectDict> GetEvaluateFile(IDictionary<string, object> obj)
        {
            return DB.ListSod("GetList_FormFileData", obj);
        }

        /// <summary>
        /// 1-首先取得版本号（HNS_AddEvaluateFile）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<StrObjectDict> GetVersion(IDictionary<string, object> obj)
        {
            return DB.ListSod("GetVersion_FormFileData", obj);
        }

        /// <summary>
        /// 2-产生数据（HNS_AddEvaluateFile）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string InSertOrUpdateEvaluateFile(IDictionary<string, object> obj)
        {
            if (!obj.ContainsKey("ID"))
            {
                string ID = Utils.getGUID();
                obj["ID"] = ID;
                DBState state = null;
                state = new DBState
                {
                    Name = "INSERT_FormFileData",
                    Param = obj.toStrObjDict(),
                    Type = ESqlType.INSERT
                };
                if (DB.Execute(state) > 0)
                {
                    return ID;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                DBState state = null;
                state = new DBState
                {
                    Name = "UPDATE_FormFileData",
                    Param = obj.toStrObjDict(),
                    Type = ESqlType.UPDATE
                };
                if (DB.Execute(state) > 0)
                {
                    return "1";
                }
                else
                {
                    return null;
                }
            }
        }
        //public string InSertOrUpdateEvaluateFile(IDictionary<string, object> obj)
        //{
        //    if (!obj.ContainsKey("ID"))
        //    {
        //        string ID = Utils.getGUID();
        //        obj["ID"] = ID;
        //        DBState state = null;
        //        state = new DBState
        //        {
        //            Name = "INSERT_PatientCareForm",
        //            Param = obj.toStrObjDict(),
        //            Type = ESqlType.INSERT
        //        };
        //        if (DB.Execute(state) > 0)
        //        {
        //            return ID;
        //        }
        //        else
        //        {
        //            return "保存失败";
        //        }
        //    }
        //    else
        //    {
        //        DBState state = null;
        //        state = new DBState
        //        {
        //            Name = "UPDATE_PatientCareForm",
        //            Param = obj.toStrObjDict(),
        //            Type = ESqlType.UPDATE
        //        };
        //        if (DB.Execute(state) > 0)
        //        {
        //            return "修改成功";
        //        }
        //        else
        //        {
        //            return "修改失败";
        //        }
        //    }
        //}



        /// <summary>
        /// 3-返回数据（HNS_AddEvaluateFile）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<StrObjectDict> EvaluateFileResult(IDictionary<string, object> obj)
        {
            return DB.ListSod("ResultAfterSave_FormFileData", obj);
        }


        /// <summary>
        /// 请求表单格式数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<StrObjectDict> GetEvaluateData(IDictionary<string, object> obj)
        {
            switch (obj["Agent"].ToString())
            {
                //样式A
                case "4f31eca8-ff29-404f-99f7-8cc8afe395a5":
                    return DB.ListSod("GetEvaluateData_FormFileDataA", obj);
                //样式B
                case "c4bbc193-9f77-408c-909c-4073a9da2d01":
                    return DB.ListSod("GetEvaluateData_FormFileDataB", obj);
                //样式C
                case "7f9b2933-fe36-45b0-9442-d3fb7384d90e":
                    return DB.ListSod("GetEvaluateData_FormFileDataC", obj);
                //自适应
                default:
                    return DB.ListSod("GetEvaluateData_FormFileData", obj);
            }

        }

        public IList<StrObjectDict> GetResultData(IDictionary<string, object> obj)
        {
            return DB.ListSod("GetData_FormFileData", obj);
        }

        public IList<StrObjectDict> GetSysElement(IDictionary<string, object> obj)
        {
            return DB.ListSod("GetSysElement_FormFileData", obj);
        }

        private static void FindChilds(Dictionary<string, object> data, Dictionary<string, Dictionary<string, object>> dicList, string key)
        {
            var dictionary = data as Dictionary<string, object>;
            if (dictionary == null) return;
            var dic = new Dictionary<string, object>();
            foreach (var item in data)
            {
                if (item.Value is Dictionary<string, object>)
                {
                    FindChilds(item.Value as Dictionary<string, object>, dicList, key);
                }

                dic.Add(item.Key, item.Value);
            }
            var check = false;
            if (dicList.Keys.Contains(key))
            {
                var temp = dicList[key];
                foreach (var item in temp)
                {
                    if (dic.Any(a => a.Value is Dictionary<string, object>))
                    {
                        check = true;
                        break;
                    }
                    dic.Add(item.Key, item.Value);
                }
                if (!check)
                {
                    dicList[key] = dic;
                }
                return;
            }
            if (!check)
            {
                dicList.Add(key, dic);
            }
        }

        public int SaveFormData(IDictionary<string, object> obj)
        {   //step1 -> delete  step2 -> Insert
            this.DelFormData(obj);
            //insert
            var temp = obj.GetString("Content");
            StrObjectDict list = JsonAdapter.FromJsonAsDictionary(temp).toStrObjDict();
            Dictionary<string, Dictionary<string, object>> dicList = new Dictionary<string, Dictionary<string, object>>();
            IList<DBState> dblist = new List<DBState>();
           
            foreach (var item in list)
            {
                if (item.Value is Dictionary<string, object>)
                {
                    FindChilds(item.Value as Dictionary<string, object>, dicList, item.Key);
                }
            }
            var t = dicList.Remove("info");
            foreach (var i in dicList)
            {
                foreach (var s in i.Value)
                {
                    dblist.Add(new DBState
                    {
                        Name = "INSERT_FormData",
                        Param = new
                        {
                            ID = obj["ID"],
                            FSID = s.Key.ToString(),
                            DataText = s.Value.ToString(),
                            VersionCode = obj["VersionCode"],
                            FormID = obj["FormID"]
                        }.toStrObjDict(),
                        Type = ESqlType.INSERT
                    });
                }
            }     
            return DB.Execute(dblist);
        }
        public int DelFormData(IDictionary<string, object> obj)
        {
            IList<DBState> dblist = new List<DBState>();
            dblist.Add(new DBState
            {
                Name = "DELETE_FormData",
                Param = obj.toStrObjDict(),
                Type = ESqlType.DELETE
            });
            return DB.Execute(dblist);
        }


        public int SaveEvaluateFile(IDictionary<string, object> obj)
        {
            IList<StrObjectDict> check = DB.ListSod("Checked_FromFileData", obj);
            if (check.Count > 0)
            {
                string str = "";
                DBState state = null;
                StrObjectDict result = obj.GetObject("Content").toStrObjDict();
                foreach (var item in result)
                {
                    str += item;
                }
                obj.Remove("Content");
                obj["Content"] = str;
                state = new DBState
                {
                    Name = "UPDATE_FormFileData",
                    Param = obj.toStrObjDict(),
                    Type = ESqlType.UPDATE
                };
                return DB.Execute(state);
            }
            else
            {
                string str = "";
                DBState state = null;
                StrObjectDict result = obj.GetObject("Content").toStrObjDict();
                foreach (var item in result)
                {
                    str += item;
                }
                obj.Remove("Content");
                obj["Content"] = str;
                state = new DBState
                {
                    Name = "INSERT_FormFileData",
                    Param = obj.toStrObjDict(),
                    Type = ESqlType.INSERT
                };
                return DB.Execute(state);
            }
        }

        public int DelEvaluateFile(IDictionary<string, object> obj)
        {
            IList<DBState> dblist = new List<DBState>();
            dblist.Add(new DBState
            {
                Name = "DELETE_FormFileData",
                Param = obj.toStrObjDict(),
                Type = ESqlType.DELETE
            });
            return DB.Execute(dblist);
        }

        public IList<StrObjectDict> GetEvaluateStyle(IDictionary<string, object> obj)
        {
            return DB.ListSod("Gets_FormFileData", obj);
        }
    }

}
