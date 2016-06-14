using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model.System;
using ZLSoft.Pub.Db;
using ZLSoft.Pub;
using ZLSoft.Model.PUB;
using ZLSoft.Model.Tree;

namespace ZLSoft.DalManager
{
    public class BulletinManager : CRUDManager
    {
        private static BulletinManager _Instance = new BulletinManager();
        public static BulletinManager Instance
        {
            get
            {
                return BulletinManager._Instance;
            }
        }
        private BulletinManager()
        {
        }


        /// <summary>
        /// 请求公告栏列表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<StrObjectDict> GetList(IDictionary<string, object> obj)
        {
            return DB.ListSod("GetList_Bulletin", obj);
        }

        /// <summary>
        /// 请求SQL
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<StrObjectDict> GetSQL(IDictionary<string, object> obj)
        {
            return DB.ListSod("GetSQL_Bulletin", obj);
        }

        public IList<StrObjectDict> GetSQLAll(IDictionary<string, object> obj)
        {
            return DB.ListSod("GetSQL_ALLBulletin", obj);
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public IList<StrObjectDict> RequestSQL(string str)
        {
            return DB.Select(str);
        }

        /// <summary>
        /// 请求ID
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<StrObjectDict> GetID(IDictionary<string, object> obj)
        {
            return DB.ListSod("GetID_Bulletin", obj);
        }

        public Tree GetTree(IDictionary<string, object> obj)
        {
            var list = DB.ListSod("Get_ALlBulletin", obj);
            StrObjectDict temp = new StrObjectDict();
            temp.Add("ID", "Top");
            temp.Add("Name", "公告栏");
            temp.Add("SuperID", "ROOT");
            list.Add(temp);
            Tree tree = new Tree();
            tree.datasList = list;
            tree.TransformTree("SuperID");
            return tree;
        }
    }
}
