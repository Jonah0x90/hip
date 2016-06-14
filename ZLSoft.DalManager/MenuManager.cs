using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;
using ZLSoft.Model.PLATFORM;

namespace ZLSoft.DalManager
{
    public class MenuManager:CRUDManager
    {
        #region
        private static MenuManager _Instance = new MenuManager();
        public static MenuManager Instance
        {
            get
            {
                return MenuManager._Instance;
            }
        }
        private MenuManager()
        {
        }
        #endregion

        #region 菜单操作权限相关


        /// <summary>
        /// 根据菜单ID获取菜单的操作权限
        /// </summary>
        /// <returns></returns>
        public IList<StrObjectDict> GetMenuOpt(StrObjectDict dict)
        {
            return DB.ListSod("LOAD_Menu_Option", dict);
        }


        public bool IsRequestAuth(StrObjectDict dict)
        {
            IList<StrObjectDict> list = DB.ListSod("LOAD_Menu_Permission_By_userid_request", dict);
            if (list != null && list.Count > 0)
            {
                return true;
            }

            return false;
        }

        #endregion

        public int InsertOrUpdate(StrObjectDict menu, StrObjectDict menuoption)
        {
            string id = Utils.getGUID();
            string ClearID = "";
            if (!menu.ContainsKey("ID"))
            {   
                menu["ID"] = id;
                ClearID = id;
                IList<DBState> dblist = new List<DBState>();
                dblist.Add(new DBState
                {
                    Name = "INSERT_Menu",
                    Param = menu.toStrObjDict(),
                    Type = ESqlType.INSERT
                });
                List<Object> options = ((Object[])menuoption.GetObject("SyncRoot")).ToList();
                if (options.Count > 0)
                {
                    IList<DBState> dblists = new List<DBState>();
                    foreach (var item in options)
                    {
                        StrObjectDict temp = item.toStrObjDict();
                        temp["ID"] = Utils.getGUID();
                        temp["MenuID"] = id;
                        DBState db = new DBState()
                        {
                            Name = "INSERT_MenuOption",
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
                ClearID = menu["ID"].ToString();
                IList<DBState> dblist = new List<DBState>();   
                dblist.Add(new DBState
                {
                    Name = "UPDATE_Menu",
                    Param = menu.toStrObjDict(),
                    Type = ESqlType.UPDATE
                });
                List<Object> options = ((Object[])menuoption.GetObject("SyncRoot")).ToList();
                //判断是否清除当前菜单下的所有关联按钮
                if (options.Count > 0)
                {
                    //先执行Delete，后Insert。完成Update操作
                    //Delete组装
                    IList<DBState> dbdel = new List<DBState>();
                    foreach (var item in options)
                    {
                        StrObjectDict temp = item.toStrObjDict();
                        DBState db = new DBState()
                        {
                            Name = "DELETE_MenuOptionMenuID",
                            Param = temp,
                            Type = ESqlType.DELETE
                        };
                        dbdel.Add(db);
                    }
                    //Step1:清除按钮相关已分配权限
                    DBState del = null;
                    del = new DBState
                    {
                        Name = "DEL_RolePower",
                        Param = new
                        {
                           MenuID = ClearID
                        }.toStrObjDict(),
                        Type = ESqlType.DELETE
                    };
                    DB.Execute(del);
                    //Step2:Delete
                    DB.Execute(dbdel);
                    IList<DBState> dblists = new List<DBState>();
                    foreach (var item in options)
                    {
                        StrObjectDict temp = item.toStrObjDict();
                        temp["ID"] = Utils.getGUID();
                        DBState db = new DBState()
                        {
                            Name = "INSERT_MenuOption",
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
                        Name = "DELETE_MenuOptionMenuID",
                        Param = new
                        {
                           MenuID = ClearID
                        }.toStrObjDict(),
                        Type = ESqlType.DELETE
                    };
                    DB.Execute(state);
                    return DB.Execute(dblist);
                }
            }
        }
    }
}
