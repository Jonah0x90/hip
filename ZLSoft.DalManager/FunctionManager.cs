using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;
using ZLSoft.Model.PLATFORM;
using ZLSoft.Pub.PageData;

namespace ZLSoft.DalManager
{
    public class FunctionManager : CRUDManager
    {

        #region 构造
        private static FunctionManager _Instance = new FunctionManager();

        public static FunctionManager Instance
        {
            get
            {
                return FunctionManager._Instance;
            }
        }


        private FunctionManager()
        {

        }

        #endregion
        public int InsertOrUpdate(StrObjectDict dict,StrObjectDict dicts)
        {
            string id = Utils.getGUID();
            string ClearID = "";
            if (!dict.ContainsKey("ID"))
            {
                dict["ID"] = id;
                ClearID = id;
                IList<DBState> dblist = new List<DBState>();
                dblist.Add(new DBState
                {
                    Name = "INSERT_Function",
                    Param = dict.toStrObjDict(),
                    Type = ESqlType.INSERT
                });
                List<Object> options = ((Object[])dicts.GetObject("SyncRoot")).ToList();
                 if (options.Count > 0)
                {
                    IList<DBState> dblists = new List<DBState>();
                    foreach (var item in options)
                    {
                        StrObjectDict temp = item.toStrObjDict();
                        temp["ID"] = Utils.getGUID();
                        temp["FunID"] = id;
                        DBState db = new DBState()
                        {
                            Name = "INSERT_FunctionOption",
                            Param = temp,
                            Type = ESqlType.INSERT
                        };
                        dblists.Add(db);
                    }
                    int i = DB.Execute(dblist);
                    int a = DB.Execute(dblists);
                    if (i > 0 && a > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return DB.Execute(dblist);
                }

            }
            else
            {
                //保存待清空菜单操作的当前菜单ID
                ClearID = dict["ID"].ToString();
                IList<DBState> dblist = new List<DBState>();
                dblist.Add(new DBState
                {
                    Name = "UPDATE_Function",
                    Param = dict.toStrObjDict(),
                    Type = ESqlType.UPDATE
                });
                List<Object> options = ((Object[])dicts.GetObject("SyncRoot")).ToList();
                //判断是否清除当前功能下的所有关联按钮
                if (options.Count > 0)
                {
                    //先执行Delete，后Insert。完成Update操作
                    //Delete
                    IList<DBState> dbdel = new List<DBState>();
                    foreach (var item in options)
                    {
                        StrObjectDict temp = item.toStrObjDict();
                        DBState db = new DBState()
                        {
                            Name = "DELETE_FunctionOptionFunID",
                            Param = temp,
                            Type = ESqlType.DELETE
                        };
                        dbdel.Add(db);
                    }
                    DB.Execute(dbdel);
                    //Insert
                    IList<DBState> dblists = new List<DBState>();
                    foreach (var item in options)
                    {
                        StrObjectDict temp = item.toStrObjDict();
                        temp["ID"] = Utils.getGUID();
                        DBState db = new DBState()
                        {
                            Name = "INSERT_FunctionOption",
                            Param = temp,
                            Type = ESqlType.INSERT
                        };
                        dblists.Add(db);
                    }
                    int i = DB.Execute(dblist);
                    int a = DB.Execute(dblists);
                    if (i > 0 && a > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    //执行清除
                    DBState state = null;
                    state = new DBState
                    {
                        Name = "DELETE_FunctionOptionFunID",
                        Param = new
                        {
                            FunID = ClearID
                        }.toStrObjDict(),
                        Type = ESqlType.DELETE
                    };
                    DB.Execute(state);
                    return DB.Execute(dblist);
                }
            }
        }

        public int Delete(StrObjectDict dict,StrObjectDict dicts)
        {
            IList<DBState> dblist = new List<DBState>();
            dblist.Add(new DBState
            {
                Name = "DELETE_Function",
                Param = dict.toStrObjDict(),
                Type = ESqlType.DELETE
            });
            dblist.Add(new DBState
            {
                Name = "DELETE_FunctionOption",
                Param = dicts.toStrObjDict(),
                Type = ESqlType.DELETE
            });
            return DB.Execute(dblist);
        }

        public IList<StrObjectDict> GetOptionList(IDictionary<string, object> obj)
        {
            return DB.ListSod("Get_FunctionOption", obj);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public PageData<Function> Pages(StrObjectDict obj, Page p)
        {
            object listcount = DB.Scalar("COUNT_Function", obj);
            IList<Function> listData = DB.List<Function>(obj, p.PageNumber, p.PageSize);
            return new PageData<Function>(listData, p.PageNumber, int.Parse(listcount.ToString()));
        }

    }
}
