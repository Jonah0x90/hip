using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model.SYS;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;

namespace ZLSoft.DalManager
{
    public class SiteManager:CRUDManager
    {
        #region 构造器
        private static SiteManager _Instance = new SiteManager();
        public static SiteManager Instance
        {
            get
            {
                return SiteManager._Instance;
            }
        }
        private SiteManager()
        {
        }

        #endregion

        #region 站点管理方法

        /// <summary>
        /// 根据设备标识获取站点信息
        /// </summary>
        /// <param name="deviceID">设备标识</param>
        /// <returns></returns>
        public SiteDevice GetSiteDeviceByDeviceID(string deviceID)
        {
            SiteDevice d = DB.Load<SiteDevice, PK_SiteDevice>(new PK_SiteDevice
            {
                DeviceID = deviceID
            });
            return d;
        }


        /// <summary>
        /// 获取已经注册的站点数量
        /// </summary>
        /// <returns></returns>
        public int GetSiteDeviceCount()
        {
            IList<StrObjectDict> list = DB.ListSod("COUNT_SiteDevice", null);

            if (list.Count > 0)
            {
                return int.Parse((list.FirstOrDefault<StrObjectDict>()).GetString("DEVICECOUNT", ECase.UPPER));
            }
            return 0;
        }

        /// <summary>
        /// 根据模块代码获取版本信息
        /// </summary>
        /// <returns></returns>
        public SiteVersionInfo GetVersionByModule(StrObjectDict obj)
        {
            string sql = DB.GetSql("Load_SiteVersionInfo_AsVersion", obj);
            return this.LoadObject<SiteVersionInfo, StrObjectDict>("Load_SiteVersionInfo_AsVersion", obj);
        }

        #endregion
    }
}
