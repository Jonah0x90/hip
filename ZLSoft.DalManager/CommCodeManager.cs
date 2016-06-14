using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model.SYS;
using ZLSoft.Model.PLATFORM;
using ZLSoft.Pub.Db;
using ZLSoft.Pub;
using ZLSoft.Pub.PageData;

namespace ZLSoft.DalManager
{
    public class CommCodeManager:CRUDManager
    {
        private static CommCodeManager _Instance = new CommCodeManager();

        public static CommCodeManager Instance
        {
            get
            {
                return CommCodeManager._Instance;
            }
        }

        private CommCodeManager()
        {

        }

        /// <summary>
        /// 通过ID获取公共详细列表
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>列表</returns>
        public static IList<CommonCodeDetail> getCommCodeDetail(string id)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("代码ID",id);
            return DB.List<CommonCodeDetail>(queryDict);
        }

        public IList<StrObjectDict> getCommCodeFrom(IDictionary<string, object> obj)
        {
            return DB.ListSod("LISTSOD_From", obj);
        }

        public IList<StrObjectDict> getFromRelation(IDictionary<string, object> obj)
        {
            return DB.ListSod("LISTSOD_FromRelation", obj);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public PageData<StrObjectDict> PagesCommCode(StrObjectDict obj, Page p)
        {
            object listcount = DB.Scalar("COUNT_CommonCode", obj);
            IList<StrObjectDict> listData = DB.ListSod("LIST_CommonCode", obj, p.PageNumber, p.PageSize);
            return new PageData<StrObjectDict>(listData, p.PageNumber, int.Parse(listcount.ToString()));
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public PageData<StrObjectDict> PagesCommCodeDetail(StrObjectDict obj, Page p)
        {
            object listcount = DB.Scalar("COUNT_CommonCodeDetail", obj);
            IList<StrObjectDict> listData = DB.ListSod("LISTSOD_CommonCodeDetail", obj, p.PageNumber, p.PageSize);
            return new PageData<StrObjectDict>(listData, p.PageNumber, int.Parse(listcount.ToString()));
        }


        public int InsertOrUpdate(CommonCode cc, CommonCodeDetail ccd)
        {
            if (string.IsNullOrEmpty(cc.ID) || string.IsNullOrEmpty(ccd.CodeID))
            {
                IList<DBState> dblist = new List<DBState>();
                dblist.Add(new DBState
                {
                    Name = "INSERT_CommonCode",
                    Param = cc.ToDict(),
                    Type = ESqlType.INSERT
                });
                dblist.Add(new DBState
                {
                    Name = "INSERT_CommonCodeDetail",
                    Param = ccd.ToDict(),
                    Type = ESqlType.INSERT
                });
                return DB.Execute(dblist);
            }
            else
            {
                IList<DBState> dblist = new List<DBState>();
                dblist.Add(new DBState
                {
                    Name = "UPDATE_CommonCode",
                    Param = cc.ToDict(),
                    Type = ESqlType.UPDATE
                });
                dblist.Add(new DBState
                {
                    Name = "UPDATE_CommonCodeDetail",
                    Param = ccd.ToDict(),
                    Type = ESqlType.UPDATE
                });
                return DB.Execute(dblist);
            }
        }

        public int Dels(IDictionary<string, object> obj)
        {
            IList<DBState> dblist = new List<DBState>();
            dblist.Add(new DBState
            {
                Name = "DELETE_CommonCodeDetail",
                Param = obj,
                Type = ESqlType.DELETE
            });
            return DB.Execute(dblist);
        }
        
    }
}
