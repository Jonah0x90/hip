using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model.PUB;
using ZLSoft.Model.SYS;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;
using ZLSoft.Pub.PageData;

namespace ZLSoft.DalManager
{
    public class UserManager:CRUDManager
    {
         #region
        private static UserManager _Instance = new UserManager();
        public static UserManager Instance
        {
            get
            {
                return UserManager._Instance;
            }
        }
        private UserManager()
        {
        }

        #endregion



        /// <summary>
        /// 获取部门人员列表
        /// </summary>
        /// <returns></returns>
        public IList<StrObjectDict> Search(string keyword)
        {
            return DB.ListSod("LIST_PubUser", new
            {
                KeyWord = keyword+"%"

            }.toStrObjDict(false));
        }


        public PageData<StrObjectDict> ListSod(StrObjectDict obj, Page p)
        {
            object listcount = DB.Scalar("",obj);
            IList<StrObjectDict> listData = DB.ListSod("", obj, p.PageNumber, p.PageSize);
            return new PageData<StrObjectDict>(listData, p.PageNumber, int.Parse(listcount.ToString()));
        }


        public bool InsertOrUpdatePubuser(PubUser pu)
        {
            return this.InsertOrUpdate<PubUser>(pu.toStrObjDict()) > 0 ? true : false;
        }

        public override int InsertOrUpdate<T>(ZLSoft.Pub.StrObjectDict dict)
        {
            DBState state = null;
            T t = System.Activator.CreateInstance<T>();
            if (!dict.ContainsKey("ID") || string.IsNullOrEmpty(dict["ID"].ToString()))
            {
                dict["ID"] = Utils.getGUID();
                dict["RelatID"] = dict["ID"];
                state = new DBState
                {
                    Name = t.MAP_INSERT,
                    Param = dict,
                    Type = ESqlType.INSERT
                };
            }
            else
            {
                state = new DBState
                {
                    Name = t.MAP_UPDATE,
                    Param = dict,
                    Type = ESqlType.UPDATE
                };
            }
            return DB.Execute(state);
        }

        public IList<StrObjectDict> GetUser(IDictionary<string, object> obj)
        {
            return DB.ListSod("Select_UserName", obj);
        }
    
    }
}
