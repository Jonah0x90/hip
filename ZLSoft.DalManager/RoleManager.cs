using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model.PUB;
using ZLSoft.Pub.Db;
using ZLSoft.Pub;
using ZLSoft.Model.Tree;

namespace ZLSoft.DalManager
{
    public class RoleManager:CRUDManager
    {
        private static RoleManager _Instance = new RoleManager();

        public static RoleManager Instance
        {
            get
            {
                return RoleManager._Instance;
            }
        }

        private RoleManager()
        {

        }

        public  int InsertBatchs(IList<RoleMember> list) 
        {
            IList<DBState> states = new List<DBState>();
            foreach (RoleMember item in list)
            {
                DBState state = null;
                    state = new DBState
                    {
                        Name = item.MAP_INSERT,
                        Param = item.ToDict(),
                        Type = ESqlType.INSERT
                    };
                states.Add(state);
            }

            return DB.ExecuteWithTransaction(states);
        }

        public int DeleteBatchs(IList<RoleMember> list)
        {
            IList<DBState> states = new List<DBState>();
            foreach (RoleMember item in list)
            {
                DBState state = null;
                state = new DBState
                {
                    Name = item.MAP_DELETE,
                    Param = item.ToDict(),
                    Type = ESqlType.DELETE
                };
                states.Add(state);
            }

            return DB.ExecuteWithTransaction(states);
        }

        public IList<StrObjectDict> GetModelList(IDictionary<string, object> obj)
        {
            return DB.ListSod("LIST_Model", obj);
        }

        public IList<StrObjectDict> GetModelRoleGroup(IDictionary<string, object> obj)
        {
            return DB.ListSod("LIST_ModelRoleGroup", obj);
        }

        public IList<StrObjectDict> GetNotSelectedUser(IDictionary<string, object> obj)
        {
            return DB.ListSod("LIST_Role_NotSelectedUser", obj);
        }

        public IList<StrObjectDict> GetSelectedUser(IDictionary<string, object> obj)
        {
            return DB.ListSod("LIST_Role_SelectedUser", obj);
        }


        public IList<StrObjectDict> CheckPower(IList<RolePower> list)
        {
            return DB.ListSod("Check_Powers", list);
        }

        public int InsertPowers(IList<RolePower> list)
        {
            IList<DBState> states = new List<DBState>();
            foreach (RolePower item in list)
            {
                DBState state = null;
                state = new DBState
                {
                    Name = "INSERT_RolePower",
                    Param = item.ToDict(),
                    Type = ESqlType.INSERT
                };
                states.Add(state);
            }

            return DB.ExecuteWithTransaction(states);
        }

        public int UpdatePowers(IList<RolePower> list)
        {
            IList<DBState> states = new List<DBState>();
            foreach (RolePower item in list)
            {
                DBState state = null;
                state = new DBState
                {
                    Name = "UPDATE_RolePower",
                    Param = item.ToDict(),
                    Type = ESqlType.UPDATE
                };
                states.Add(state);
            }

            return DB.ExecuteWithTransaction(states);
        }

        public int DeletePowers(IDictionary<string,object> obj)
        {
            DBState db = new DBState();
            db = new DBState
            {
                Name = "DELALL_RolePower",
                Param = obj,
                Type =ESqlType.DELETE
            };
            return DB.Execute(db);
        }

        public Tree GetRoleTree(IDictionary<string, object> obj)
        {
            obj["TreeID"] = "3ad0e672-aba8-432e-83d4-7a30a999";
            obj["OptionID"] = "9ad0e672-aba8-432e-83d4-7a30a999";
            var Power = DB.ListSod("GetPowerTree_RolePower", obj);
            var Option = DB.ListSod("GetOptionTree_RolePower", obj);
            var psql = Power[0]["SQL"].ToString();
            var osql = Option[0]["SQL"].ToString();
            obj.Remove("TreeID");
            obj.Remove("OptionID");
            IList<StrObjectDict> list = new List<StrObjectDict>();
            if (obj != null)
            {
                foreach (var item in obj.Keys)
                {
                    psql = psql.Replace("$" + item + "$", "" + obj[item]);
                }
            }
            var pResult = DB.Select(psql);
            if (pResult.Count > 0)
            {
                foreach (var items in pResult)
                {
                    var temp = osql;
                    temp = temp.Replace("$GroupID$", "" + obj["GroupID"]);
                    temp = temp.Replace("$ID$", "" + items["ID"]);
                    var oResult = DB.Select(temp);
                    if(oResult.Count > 0)
                    {
                        for (int i = 0; i < oResult.Count;i++ )
                            if (items["SuperID"].ToString() != "ROOT" && items["ID"].ToString() == oResult[i]["MenuID"].ToString())
                            {
                                items["Children"] = oResult;
                            }
                    }
                }
            }
            Tree tree = new Tree();
            tree.datasList = pResult;
            tree.TransformTree("SuperID");
            return tree;
        }

    }
}
