using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model.PLATFORM;
using ZLSoft.Model.SYS;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;

namespace ZLSoft.DalManager
{
    public class DataServiceManager:CRUDManager
    {
         #region
        private static DataServiceManager _Instance = new DataServiceManager();
        public static DataServiceManager Instance
        {
            get
            {
                return DataServiceManager._Instance;
            }
        }
        private DataServiceManager()
        {
        }

        #endregion

        /// <summary>
        /// 保存类型为数据源查询的服务
        /// </summary>
        /// <returns></returns>
        public int SaveThirdServiceSqlType(StrObjectDict obj)
        {
            IList<DBState> dblist = new List<DBState>();
            if (obj.ContainsKey("ID"))
            {
                dblist.Add(new DBState
                {
                    Name = "UPDATE_DataService",
                    Param = obj,
                    Type = ESqlType.UPDATE
                });
            }
            else
            {
                obj["ID"] = Utils.getGUID();
                dblist.Add(new DBState
                {
                    Name = "INSERT_DataService",
                    Param = obj,
                    Type = ESqlType.INSERT
                });
            }
            

            if (obj.GetInt("IsRef") == 1 && obj["Reference"] != null && (obj["Reference"] as object[]).Count() > 0)
            {
                if(!obj.ContainsKey("ID")){

                }
                dblist.Add(new DBState
                {
                    Name = "DELETE_DataServiceColumnMap",
                    Param = new
                    {
                        ServiceID = obj["ID"]
                    },
                    Type = ESqlType.DELETE
                });
                object[] relist = obj["Reference"] as object[];
                IDictionary<string, object> dict = null;
                foreach (var item in relist)
                {
                    dict = item as IDictionary<string, object>;
                    dblist.Add(new DBState
                    {
                        Name = "INSERT_DataServiceColumnMap",
                        Param = new
                        {
                            ServiceID = obj["ID"],
                            ThirdColumn = dict["ThirdColumn"],
                            LocalColumn = dict["LocalColumn"]
                        },
                        Type = ESqlType.INSERT
                    });
                }
            }


            return DB.Execute(dblist);
        }

        /// <summary>
        /// 获取服务数据字段对照(Sql方式)
        /// </summary>
        /// <param name="serviceID"></param>
        /// <returns></returns>
        public IList<DataServiceColumnMap> GetColumnMapList(string serviceID)
        {
            return this.List<DataServiceColumnMap>(new
            {
                ServiceID = serviceID
            }.toStrObjDict());
        }

    }
}
