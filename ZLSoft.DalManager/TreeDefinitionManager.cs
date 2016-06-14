using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model.SYS;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;
using ZLSoft.Pub.PageData;

namespace ZLSoft.DalManager
{
    public class TreeDefinitionManager : CRUDManager
    {
        #region 构造
        private static TreeDefinitionManager _Instance = new TreeDefinitionManager();
        public static TreeDefinitionManager Instance
        {
            get
            {
                return TreeDefinitionManager._Instance;
            }
        }
        private TreeDefinitionManager()
        {
        }
        #endregion

        public IList<StrObjectDict> GetList(IDictionary<string, object> obj)
        {
            return DB.ListSod("GetAll_TreeDefinition", obj);
        }

        public PageData<StrObjectDict> ListSod(StrObjectDict obj, Page p)
        {
            object listcount = DB.Scalar("COUNT_TreeDefinition", obj);
            IList<StrObjectDict> listData = DB.ListSod("LIST_TreeDefinition", obj, p.PageNumber, p.PageSize);
            return new PageData<StrObjectDict>(listData, p.PageNumber, int.Parse(listcount.ToString()));
        }

        public int InsertOrUpdate(IDictionary<string, object> dict)
        {
            //insert
            if (!dict.ContainsKey("ID") || string.IsNullOrEmpty(dict["ID"].ToString()))
            {
                dict["ID"] = Utils.getGUID();
                DBState state = null;
                state = new DBState
                {
                    Name = "INSERT_TreeDefinition",
                    Param = dict,
                    Type = ESqlType.INSERT
                };
                return DB.Execute(state);
            }
            //update
            else
            {
                DBState state = null;
                dict["Invalidtime"] = DateTime.Now;
                state = new DBState
                {
                    Name = "UPDATE_TreeDefinition",
                    Param = dict,
                    Type = ESqlType.UPDATE
                };
                return DB.Execute(state);
            }
        }

        public int Delete(IDictionary<string, object> dict)
        { 
            DBState state = null;
            state = new DBState
                {
                    Name = "DELETE_TreeDefinition",
                    Param = new
                    {
                        ID = dict["ID"]
                    }.toStrObjDict(),
                    Type = ESqlType.DELETE
                };
                return DB.Execute(state);
        }

    }
}
