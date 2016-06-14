using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model.SYS;
using ZLSoft.Model.PLATFORM;
using ZLSoft.Pub.Db;
using ZLSoft.Pub;
using ZLSoft.Pub.Constant;

namespace ZLSoft.DalManager
{
    /// <summary>
    /// 下拉框数据
    /// </summary>
    public class KeyValueManager:CRUDManager
    {
        #region 构造
        private static KeyValueManager _Instance = new KeyValueManager();

        public static KeyValueManager Instance
        {
            get
            {
                return KeyValueManager._Instance;
            }
        }


        private KeyValueManager()
        {

        }

        #endregion

        

        #region 方法
        public IList<StrObjectDict> List(string id)
        {
            //获取公共代码信息
            CommonCode commonCode = DB.Load<CommonCode, PK_CommonCode>(new PK_CommonCode 
            {
                ID = id
            });

            //公共代码明细是否为SQL获取

            if (commonCode.IsExtSQL == 1 && !string.IsNullOrEmpty(commonCode.ExtSQL))
            {
                return  DB.Select(commonCode.ExtSQL);
            }
            else
            {
                return DB.ListSod("LOAD_KeyValue", new
                {
                    ID = id,
                    ORDER_BY_CLAUSE = " 排序序号 asc"
                });
            }

           
        }

        //public IList<StrObjectDict> List(string key,string value,string tableName,string orderColumn, ORDER_TYPE order)

        //{
        //    StrObjectDict param = new StrObjectDict() 
        //    { 
        //        {"Value",value},
        //        {"Key",key},
        //        {"TableName",tableName},
        //        {"ORDER_BY_CLAUSE$","Order by" + orderColumn + order}

        //    };

        //    return DB.ListSod("LOAD_KeyValue", param);
        //}

        //public IList<StrObjectDict> List(string key, string value, string tableName, string orderColumn)
        //{
        //    return this.List(key, value, tableName, orderColumn, ORDER_TYPE.ASC);
        //}

        //public IList<StrObjectDict> List(string key, string value, string tableName)
        //{
        //    return this.List(key, value, tableName, key, ORDER_TYPE.ASC);
        //}

        //public IList<StrObjectDict> ListByMapID(string mapID)
        //{
        //    return DB.ListSod(mapID, null);
        //}
        

        #endregion

    }
}
