using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model.PUB;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;

namespace ZLSoft.DalManager
{
    /// <summary>
    ///  Form Manager
    /// </summary>
    public class FormManager : CRUDManager
    {
        #region
        private static FormManager _Instance = new FormManager();
        public static FormManager Instance
        {
            get
            {
                return FormManager._Instance;
            }
        }
        private FormManager()
        {
        }

        #endregion


        public int InsertData(StrObjectDict list, StrObjectDict dict)
        {
            var name = "type";
            List<Object> result = ((Object[])list.GetObject("SyncRoot")).ToList();
            IList<DBState> dblist = new List<DBState>();
            foreach (var item in result)
            {

                StrObjectDict temp = item.toStrObjDict();

                //string superID = temp.GetString("id");

                if (temp.ContainsKey("options"))
                {
                    List<Object> temp1 = ((Object[])temp.GetObject("options")).ToList();
                    temp.Remove("options");
                    var id = temp.GetString("id");
                    var ywhy = temp.GetString("YWHY");
                    var type = temp.GetString("type");
                    var text = temp.GetString("text");
                    var no = temp.GetString("NO");
                    var style = 1;
                    switch (type)
                    {
                        case "input":
                            style = 1;
                            break;
                        case "radio":
                            style = 2;
                            break;
                        case "checkbox":
                            style = 2;
                            break;
                        case "grade":
                            style = 1;
                            break;
                        case "label":
                            style = 1;
                            break;
                    }
                    //foreach (var value in temp)
                    //{
                    dblist.Add(new DBState
                    {
                        Name = "INSERT_FormStructure",
                        Param = new
                        {
                            ID = id,
                            FormID = dict["FormID"],
                            VersionCode = dict["VersionCode"],
                            ElementName = text,
                            ElementAttribute = type,
                            Implication = ywhy,
                            FormOfExpression = style,
                            NO = no
                        }.toStrObjDict(),
                        Type = ESqlType.INSERT
                    });
                    //}

                    foreach (var lls in temp1)
                    {
                        StrObjectDict sod = lls.toStrObjDict();
                        var ids = sod.GetString("id");
                        var ywhys = sod.GetString("YWHY");
                        var types = sod.GetString("type");
                        var texts = sod.GetString("text");
                        var nos = sod.GetString("NO");
                        var styles = 1;
                        switch (types)
                        {
                            case "input":
                                style = 1;
                                break;
                            case "radio":
                                style = 2;
                                break;
                            case "checkbox":
                                style = 2;
                                break;
                            case "grade":
                                style = 1;
                                break;
                            case "label":
                                style = 1;
                                break;
                        }
                        dblist.Add(new DBState
                        {
                            Name = "INSERT_FormStructure",
                            Param = new
                            {
                                ID = ids,
                                FormID = dict["FormID"],
                                VersionCode = dict["VersionCode"],
                                ElementName = texts,
                                ElementAttribute = types,
                                Implication = ywhys,
                                FormOfExpression = styles,
                                SuperID = id,
                                NO = nos
                            }.toStrObjDict(),
                            Type = ESqlType.INSERT
                        });
                    }
                }
                else
                {
                    var id = temp.GetString("id");
                    var ywhy = temp.GetString("YWHY");
                    var type = temp.GetString("type");
                    var text = temp.GetString("text");
                    var no = temp.GetString("NO");
                    var style = 1;
                    switch (type)
                    {
                        case "input":
                            style = 1;
                            break;
                        case "radio":
                            style = 2;
                            break;
                        case "checkbox":
                            style = 2;
                            break;
                        case "grade":
                            style = 1;
                            break;
                        case "label":
                            style = 1;
                            break;
                    }
                    dblist.Add(new DBState
                    {
                        Name = "INSERT_FormStructure",
                        Param = new
                        {
                            ID = id,
                            FormID = dict["FormID"],
                            VersionCode = dict["VersionCode"],
                            ElementName = text,
                            ElementAttribute = type,
                            Implication = ywhy,
                            FormOfExpression = style,
                            NO = no
                        }.toStrObjDict(),
                        Type = ESqlType.INSERT
                    });
                }
            }
            return DB.Execute(dblist);
        }

        public int DelData(IDictionary<string,object> obj)
        {

            IList<DBState> dblist = new List<DBState>();
            dblist.Add(new DBState
            {
                Name = "Del_FormStructure",
                Param = obj,
                Type = ESqlType.DELETE
            });
            return DB.Execute(dblist);
        }

        //public int InsertData(StrObjectDict list, StrObjectDict dict)
        //{
        //    List<Object> result = ((Object[])list.GetObject("SyncRoot")).ToList();
        //    IList<DBState> dblist = new List<DBState>();
        //    foreach (var item in result)
        //    {
        //        StrObjectDict temp = item.toStrObjDict();

        //        string superID = temp.GetString("id");

        //        if (temp.ContainsKey("options"))
        //        {
        //            List<Object> temp1 = ((Object[])temp.GetObject("options")).ToList();
        //            temp.Remove("options");
        //            string listone = "";
        //            foreach (var value in temp)
        //            {
        //                listone += value;
        //            }
        //            dblist.Add(new DBState
        //            {
        //                Name = "INSERT_FormData",
        //                Param = new
        //                {
        //                    FormID = dict["FormID"],
        //                    DataText = listone,
        //                    VersionCode = dict["VersionCode"]
        //                }.toStrObjDict(),
        //                Type = ESqlType.INSERT
        //            });
        //            foreach (var items in temp1)
        //            {
        //                string listtwo = "";
        //                foreach (var lls in temp1)
        //                {
        //                    StrObjectDict sod = lls.toStrObjDict();
        //                    sod["superID"] = superID;
        //                    foreach (var values in sod)
        //                    {
        //                        listtwo += values;
        //                    }
        //                    dblist.Add(new DBState
        //                    {
        //                        Name = "INSERT_FormData",
        //                        Param = new
        //                        {
        //                            FormID = dict["FormID"],
        //                            DataText = listtwo,
        //                            VersionCode = dict["VersionCode"]
        //                        }.toStrObjDict(),
        //                        Type = ESqlType.INSERT
        //                    });
        //                }

        //            }

        //        }
        //    }
        //    return DB.Execute(dblist);
        //}


        /// <summary>
        /// 请求表单类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<StrObjectDict> Hns_GetFormType(IDictionary<string, object> obj)
        {
            return DB.ListSod("List_GetForm", obj);
        }

        /// <summary>
        /// 客户端调用
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<StrObjectDict> Client_GetFormType(IDictionary<string, object> obj)
        {
            return DB.ListSod("List_Client_GetForm", obj);
        }

        /// <summary>
        /// 请求表单数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<StrObjectDict> Hns_GetForm(IDictionary<string, object> obj)
        {
            string versioncode = null;
            string Agent = null;
            try
            {
                versioncode = obj["VersionCode"].ToString();
                Agent = obj["Agent"].ToString();
            }
            catch (Exception error)
            {

            }
            //入参版本号判断
            if (string.IsNullOrEmpty(versioncode))
            {
                //返回表单版本号/发布人/发布时间/发布说明
                return DB.ListSod("ListInfo_GetForm", obj);
            }
            else
            {
                //返回表单数据
                switch (Agent)
                {
                    //样式A
                    case "4f31eca8-ff29-404f-99f7-8cc8afe395a5":
                        return DB.ListSod("ResultA_GetForm", obj);
                    //样式B
                    case "c4bbc193-9f77-408c-909c-4073a9da2d01":
                        return DB.ListSod("ResultB_GetForm", obj);
                    //样式C
                    case "7f9b2933-fe36-45b0-9442-d3fb7384d90e":
                        return DB.ListSod("ResultC_GetForm", obj);
                    default:
                        return DB.ListSod("Result_GetForm", obj);
                }
            }
        }

        public int FormInsert(IDictionary<string, object> obj)
        {
            IList<DBState> dblist = new List<DBState>();
            dblist.Add(new DBState
            {
                Name = "INSERT_Form",
                Param = obj.toStrObjDict(),
                Type = ESqlType.INSERT
            });
            dblist.Add(new DBState
            {
                Name = "INSERT_FormFile",
                Param = obj.toStrObjDict(),
                Type = ESqlType.INSERT
            });
            return DB.Execute(dblist);
        }

        public int FormUpdate(IDictionary<string, object> obj)
        {
            IList<DBState> dblist = new List<DBState>();
            obj["FormID"] = obj["ID"];
            dblist.Add(new DBState
            {
                Name = "UPDATE_Form",
                Param = obj.toStrObjDict(),
                Type = ESqlType.UPDATE
            });
            dblist.Add(new DBState
            {
                Name = "UPDATE_FormFile",
                Param = obj.toStrObjDict(),
                Type = ESqlType.UPDATE
            });
            return DB.Execute(dblist);
        }

        public int FormVerInsert(IDictionary<string, object> obj)
        {
            IList<DBState> dblist = new List<DBState>();
            dblist.Add(new DBState
            {
                Name = "INSERT_FormFile",
                Param = obj.toStrObjDict(),
                Type = ESqlType.INSERT
            });
            return DB.Execute(dblist);
        }

        public int FormVerUpdate(IDictionary<string, object> obj)
        {
            IList<DBState> dblist = new List<DBState>();
            dblist.Add(new DBState
            {
                Name = "UPDATE_FormFile",
                Param = obj.toStrObjDict(),
                Type = ESqlType.UPDATE
            });
            return DB.Execute(dblist);
        }

        public int FormVerDel(IDictionary<string, object> obj)
        {
            IList<DBState> dblist = new List<DBState>();
            dblist.Add(new DBState
            {
                Name = "DELETEVer_FormFile",
                Param = obj.toStrObjDict(),
                Type = ESqlType.DELETE
            });
            return DB.Execute(dblist);
        }

        public int FomrVerDelCatch(string id)
        {
            IList<DBState> dblist = new List<DBState>();
            dblist.Add(new DBState
            {
                Name = "CatchDel_FormFile",
                Param = new
                {
                    ID = id
                }.toStrObjDict(),
                Type = ESqlType.DELETE
            });
            return DB.Execute(dblist);
        }



        /// <summary>
        /// Insert Or Update 表单信息
        /// </summary>
        /// <param name="nsf">护理表单</param>
        /// <param name="nsfv">护理表单版本</param>
        /// <param name="nsff">护理表单版本文件</param>
        /// <returns></returns>
        public int NursingFormInsertOrUpdate(Form nsf, FormFile nsff)
        {
            string ids = nsf.ID;
            if (string.IsNullOrEmpty(ids))
            {
                ///insert
                string id = Utils.getGUID();
                nsf.ID = id;
                nsff.FormID = id;
                IList<DBState> dblist = new List<DBState>();
                dblist.Add(new DBState
                {
                    Name = nsf.MAP_INSERT,
                    Param = nsf.ToDict(),
                    Type = ESqlType.INSERT
                });
                dblist.Add(new DBState
                {
                    Name = nsff.MAP_INSERT,
                    Param = nsff.ToDict(),
                    Type = ESqlType.INSERT
                });
                return DB.Execute(dblist);
            }
            else
            {
                ///update
                IList<DBState> dblist = new List<DBState>();
                dblist.Add(new DBState
                {
                    Name = "UPDATE_Form",
                    Param = nsf.ToDict(),
                    Type = ESqlType.UPDATE
                });
                dblist.Add(new DBState
                {
                    Name = "UPDATE_FormFile",
                    Param = nsff.ToDict(),
                    Type = ESqlType.UPDATE
                });
                return DB.Execute(dblist);
            }
        }

        /// <summary>
        /// Delete 表单信息
        /// </summary>
        /// <param name="id">表单ID</param>
        /// <returns></returns>
        public int NursingFormInsertDelete(string id)
        {
            DBState state = null;
            IList<DBState> dblist = new List<DBState>();
            dblist.Add(new DBState
            {
                Name = "DELETE_Form",
                Param = new
                {
                    ID = id
                }.toStrObjDict(),
                Type = ESqlType.DELETE
            });
            dblist.Add(new DBState
            {
                Name = "DELETE_FormFile",
                Param = new
                {
                    ID = id
                }.toStrObjDict(),
                Type = ESqlType.DELETE
            });
            return DB.Execute(dblist);
        }

        /// <summary>
        /// 发布表单(保存)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Hns_PublishInsert(IDictionary<string, object> obj)
        {
            IList<DBState> dblist = new List<DBState>();
            dblist.Add(new DBState
            {
                Name = "ToPublishA_GetForm",
                Param = obj.toStrObjDict(),
                Type = ESqlType.UPDATE
            });
            dblist.Add(new DBState
            {
                Name = "ToPublishB_GetForm",
                Param = obj.toStrObjDict(),
                Type = ESqlType.UPDATE
            });
            return DB.Execute(dblist);
        }

        /// <summary>
        /// 发布表单(返回)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<StrObjectDict> Hns_Publish(IDictionary<string, object> obj)
        {
            return DB.ListSod("ResultPublish_GetForm", obj);
        }


        /// <summary>
        /// 表单状态（停用与启用）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<StrObjectDict> Hns_SetFormState(IDictionary<string, object> obj)
        {
            //停用
            if (obj["State"].ToString() == "0")
            {
                return DB.ListSod("SetStateA_Form", obj);
            }
            //State = 1 启用
            else
            {
                return DB.ListSod("SetStateB_Form", obj);
            }
        }

        /// <summary>
        /// 请求适用科室列表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<StrObjectDict> HNS_GetDepartmentPersonnel(IDictionary<string, object> obj)
        {
            return DB.ListSod("ResultDeptList_Form", obj);
        }

        /// <summary>
        /// 保存适用科室
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Hns_setFormDepts(IDictionary<string, object> obj)
        {
            DBState state = null;
            state = new DBState
            {
                Name = "SaveDeptList_Form",
                Param = new
                {
                    DeptRange = obj["DeptRange"],
                    ID = obj["ID"]
                }.toStrObjDict(),
                Type = ESqlType.UPDATE
            };
            return DB.Execute(state);
        }

        /// <summary>
        /// 请求对应宣教单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public StrObjectDict HNS_GetEvaluateStyleByID(IDictionary<string, object> obj)
        { 
            string[] temp = obj["ID"].ToString().Split(',');
            //IList<dynamic> list = new List<dynamic>();
            IList<StrObjectDict> lists = new List<StrObjectDict>();
            StrObjectDict result = new StrObjectDict();
            int num = 0;
            foreach (var i in temp)
            {
                obj["ID"] = i;
                //list.Add(new { Data = DB.ListSod("ResultEvaluatestyleID_Form", obj) });
                lists = DB.ListSod("ResultEvaluatestyleID_Form", obj);
                result.Add(num.ToString(), lists);
                num++;
            }
            return result;
        }

        public IList<StrObjectDict> HNS_GetEvaluateStyle(IDictionary<string, object> obj)
        { 
             return DB.ListSod("ResultEvaluatestyle_Form", obj);
        }

        /// <summary>
        /// 保存对应宣教单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Hns_setFormMap(IDictionary<string, object> obj)
        {
            DBState state = null;
            state = new DBState
            {
                Name = "SaveFormMap_Form",
                Param = new
                {
                    FormAgainst = obj["FormAgainst"],
                    ID = obj["ID"]
                }.toStrObjDict(),
                Type = ESqlType.UPDATE
            };
            return DB.Execute(state);
        }

        /// <summary>
        /// 检查表单名称是否重复
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<StrObjectDict> CheckName(IDictionary<string, object> obj)
        {
            return DB.ListSod("CheckName_Form", obj);
        }

        /// <summary>
        /// 请求元素详细信息
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="Type">0:取元素,1:取元素组</param>
        /// <returns></returns>
        public IList<StrObjectDict> Hns_GetElementTypes(IDictionary<string, object> obj)
        {
            if (obj.GetString("Type") == "0")
            {
                //取元素分类
                return DB.ListSod("Get_FormElementGroup", obj);
            }
            else
            {
                //取元素组分类
                return DB.ListSod("Get_FormPartGroups", obj);
            }
        }

        /// <summary>
        /// 请求元素列表
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IList<StrObjectDict> Hns_GetTypeElements(IDictionary<string, object> obj)
        {
            if (obj.GetString("Type") == "0")
            {
                //返回所有分类
                return DB.ListSod("Get_TypeElementsAll", obj);
            }
            if (obj.GetString("Type") == "1")
            {
                //返回元素分类ID下的分类
                return DB.ListSod("Get_TypeElementsByID", obj);
            }
            if (obj.GetString("Type") == "2")
            {
                //返回元素组分类ID下的分类
                return DB.ListSod("Get_TypeElementsGroupByID", obj);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 保存元素
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Hns_SaveElement(IDictionary<string, object> obj)
        {
            if (!obj.ContainsKey("ID"))
            {
                obj["ID"] = Utils.getGUID();
                DBState state = null;
                state = new DBState
                {
                    Name = "INSERT_ElementType",
                    Param = obj.toStrObjDict(),
                    Type = ESqlType.INSERT
                };
                return DB.Execute(state);
            }
            else
            {
                DBState state = null;
                state = new DBState
                {
                    Name = "UPDATE_ElementType",
                    Param = obj.toStrObjDict(),
                    Type = ESqlType.UPDATE
                };
                return DB.Execute(state);
            }
        }

        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Hns_DelElement(IDictionary<string, object> obj)
        {
            DBState state = null;
            state = new DBState
            {
                Name = "DELETE_ElementType",
                Param = obj.toStrObjDict(),
                Type = ESqlType.DELETE
            };
            return DB.Execute(state);
        }

        /// <summary>
        /// 保存元素组(要素)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Hns_SaveElementGroup(IDictionary<string, object> obj)
        {
            if (!obj.ContainsKey("ID"))
            {
                obj["ID"] = Utils.getGUID();
                DBState state = null;
                state = new DBState
                {
                    Name = "INSERT_ElementPart",
                    Param = obj.toStrObjDict(),
                    Type = ESqlType.INSERT
                };
                return DB.Execute(state);
            }
            else
            {
                DBState state = null;
                state = new DBState
                {
                    Name = "UPDATE_ElementPart",
                    Param = obj.toStrObjDict(),
                    Type = ESqlType.UPDATE
                };
                return DB.Execute(state);
            }
        }

        /// <summary>
        /// 删除元素组(要素)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Hns_DelElementGroup(IDictionary<string, object> obj)
        {
            DBState state = null;
            state = new DBState
            {
                Name = "DELETE_ElementPart",
                Param = obj.toStrObjDict(),
                Type = ESqlType.DELETE
            };
            return DB.Execute(state);
        }

        /// <summary>
        /// 保存护理元素分类
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Hns_SaveEType(IDictionary<string, object> obj)
        {
            if (!obj.ContainsKey("ID"))
            {
                obj["ID"] = Utils.getGUID();
                DBState state = null;
                state = new DBState
                {
                    Name = "INSERT_ElementTypeGroup",
                    Param = obj.toStrObjDict(),
                    Type = ESqlType.INSERT
                };
                return DB.Execute(state);
            }
            else
            {
                DBState state = null;
                state = new DBState
                {
                    Name = "UPDATE_ElementTypeGroup",
                    Param = obj.toStrObjDict(),
                    Type = ESqlType.UPDATE
                };
                return DB.Execute(state);
            }
        }

        /// <summary>
        /// 删除护理元素分类
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Hns_DelEType(IDictionary<string, object> obj)
        {
            DBState state = null;
            state = new DBState
            {
                Name = "DELETE_ElementTypeGroup",
                Param = obj.toStrObjDict(),
                Type = ESqlType.DELETE
            };
            return DB.Execute(state);
        }

        /// <summary>
        /// 保存护理要素分类
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Hns_SavePType(IDictionary<string, object> obj)
        {
            if (!obj.ContainsKey("ID"))
            {
                obj["ID"] = Utils.getGUID();
                DBState state = null;
                state = new DBState
                {
                    Name = "INSERT_ElementPartGroup",
                    Param = obj.toStrObjDict(),
                    Type = ESqlType.INSERT
                };
                return DB.Execute(state);
            }
            else
            {
                DBState state = null;
                state = new DBState
                {
                    Name = "UPDATE_ElementPartGroup",
                    Param = obj.toStrObjDict(),
                    Type = ESqlType.UPDATE
                };
                return DB.Execute(state);
            }
        }

        /// <summary>
        /// 删除护理要素分类
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Hns_DelPType(IDictionary<string, object> obj)
        {
            DBState state = null;
            state = new DBState
            {
                Name = "DELETE_ElementPartGroup",
                Param = obj.toStrObjDict(),
                Type = ESqlType.DELETE
            };
            return DB.Execute(state);
        }

        /// <summary>
        /// 请求护理要素内容
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<StrObjectDict> Hns_SelectElementGrup(IDictionary<string, object> obj)
        {
            return DB.ListSod("Get_ElementPartInfo", obj);
        }

        public IList<StrObjectDict> CheckVerCode(IDictionary<string, object> obj)
        {
            return DB.ListSod("GetMaxVerCode_Form", obj);
        }

        public IList<StrObjectDict> GetFormTypes(IDictionary<string, object> obj)
        {
            return DB.ListSod("GetFormTypes_Form", obj);
        }

        public int SetVerCode(IDictionary<string, object> obj)
        {
            DBState state = null;
            state = new DBState
            {
                Name = "SetVerCode_Form",
                Param = obj.toStrObjDict(),
                Type = ESqlType.UPDATE
            };
            return DB.Execute(state);
        }

        public string NewVerCode(string str)
        {
            try
            {
                double temp = double.Parse(str);
                temp += 0.1;
                return temp.ToString("F1");
            }
            catch
            {
                return "Error";
            }
        }

        public IList<StrObjectDict> GetFormDept(IDictionary<string, object> obj)
        {
            return DB.ListSod("GetFormDept_Form", obj);
        }

        public IList<StrObjectDict> GetFormStyle(IDictionary<string, object> obj)
        {
            return DB.ListSod("GetStyleCss_Form", obj);
        }

        public int SetMobileStyle(IDictionary<string, object> obj)
        {
            DBState state = null;
            state = new DBState
            {
                Name = "SetMobileStyle_Form",
                Param = obj.toStrObjDict(),
                Type = ESqlType.UPDATE
            };
            return DB.Execute(state);
        }

        public int SetDesktopStyle(IDictionary<string, object> obj)
        {
            DBState state = null;
            state = new DBState
            {
                Name = "SetDesktopStyle_Form",
                Param = obj.toStrObjDict(),
                Type = ESqlType.UPDATE
            };
            return DB.Execute(state);
        }

        public IList<StrObjectDict> StyleInfo(IDictionary<string, object> obj)
        {
            return DB.ListSod("Styleinfo_Form", obj);
        }

        public IList<StrObjectDict> ClientStyleInfo(IDictionary<string, object> obj)
        {
            return DB.ListSod("ClientStyleinfo_Form", obj);
        }

        public IList<StrObjectDict> CheckedPublish(IDictionary<string, object> obj)
        {
            return DB.ListSod("CheckedPublish_GetForm", obj);
        }
    }
}
