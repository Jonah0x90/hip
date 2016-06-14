using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model.SYS;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;

namespace ZLSoft.DalManager
{
    public class SiteDeviceManager:CRUDManager
    {
        private static SiteDeviceManager _Instance = new SiteDeviceManager();
        public static SiteDeviceManager Instance
        {
            get
            {
                return SiteDeviceManager._Instance;
            }
        }
        private SiteDeviceManager()
        {
        }


        /// <summary>
        /// 站点设备注册(设备信息注册，设备所属注册)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Add(SiteDevice obj,string moduleCode)
        {
            obj.ID = Utils.getGUID();
            IList<DBState> dblist = new List<DBState>();
            dblist.Add(new DBState
            {
                Name = obj.MAP_INSERT,
                Param = obj.ToDict(),
                Type = ESqlType.INSERT
            });
            dblist.Add(new DBState
            {
                Name = "INSERT_SiteDevice_Module",
                Param = new
                {
                    ModuleCode = moduleCode,
                    DeviceID = obj.DeviceID
                }.toStrObjDict(),
                Type = ESqlType.INSERT
            });
            return DB.Execute(dblist);
        }
    }
}
