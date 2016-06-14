using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.AppContext;
using ZLSoft.DalManager;
using ZLSoft.Model.SYS;
using ZLSoft.Pub;

namespace ZLSoft.Sys.Controllers
{
    /// <summary>
    /// 站点管理相关
    /// </summary>
    public class SiteController:BaseController
    {

        #region 站点接口
        /// <summary>
        /// 1.检查站点设备
        /// 2.注册站点设备,保存内容：所属机构，站点设备相关信息,应用模块代码
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            StrObjectDict dict = GetParams();

            object pointID = dict.GetObject("PointID");
            object orgID = dict.GetObject("OrgID");
            object pointInfo = dict.GetObject("PointInfo");
            object moduleCode = dict.GetObject("ModuleCode");

            #region 检查参数是否正确
            if (pointID == null || string.IsNullOrEmpty(pointID.ToString())){
                return this.MyJson(0,"参数错误:PointID");
            }

            if (orgID == null || string.IsNullOrEmpty(orgID.ToString()))
            {
                return this.MyJson(1,"参数错误:OrgID");
            }

            if (pointInfo == null || string.IsNullOrEmpty(pointInfo.ToString()))
            {
                return this.MyJson(0,"参数错误:PointInfo");
            }

            if (moduleCode == null || string.IsNullOrEmpty(moduleCode.ToString()))
            {
                return this.MyJson(0,"参数错误:ModuleID");
            }

            #endregion

            //检查设备是否已经注册
            SiteDevice device = SiteManager.Instance.GetSiteDeviceByDeviceID(pointID.ToString());

            if (device == null || string.IsNullOrEmpty(device.ID))
            {
                
                int deviceCount = 99;//通过授权文件查询出授权使用的设备数量

                int regDeviceCount = SiteManager.Instance.GetSiteDeviceCount();

                if (deviceCount <= regDeviceCount)
                {
                    return this.MyJson(0,"已超过允许注册设备的最大数量,请联系管理员！");
                }

                //未注册同时也没有超过设备允许注册的最大数量，则自动注册该设备
                int result = SiteDeviceManager.Instance.Add(new SiteDevice
                {
                    DeviceID = pointID.ToString(),
                    DeviceInfo = pointInfo.ToString(),
                    Enable = 1,
                    IsMobile = 1,
                    RegTime = DateTime.Now,
                    Remark = "",
                    DeptID = orgID.ToString()
                },moduleCode.ToString());

                if (result < 1)
                {
                    return this.MyJson(0,"设备注册失败,请联系管理员！");
                }
            }

            return this.MyJson(1);
        }


        /// <summary>
        /// 检查是否需要更新(仅移动端使用)
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckUpdate()
        {
            StrObjectDict dict = Request.HttpDataToDict();
            string versionCode = dict.GetString("VersionCode");//移动端软件版本代码
            string deviceID = dict.GetString("PointID");//站点设备标识
            string moduleCode = dict.GetString("ModuleCode");//应用模块代码

            #region 检查参数是否正确
            if (versionCode == null || string.IsNullOrEmpty(versionCode))
            {
                return this.MyJson(new
                {
                    Flag = 0,
                    Error = "参数错误:VersionCode"
                });
            }

            if (string.IsNullOrEmpty(deviceID))
            {
                return this.MyJson(new
                {
                    Flag = 0,
                    Error = "参数错误:DeviceID"
                });
            }

            if (string.IsNullOrEmpty(moduleCode))
            {
                return this.MyJson(new
                {
                    Flag = 0,
                    Error = "参数错误:ModuleCode"
                });
            }

            #endregion

            SiteVersionInfo version = SiteManager.Instance.GetVersionByModule(new
            {
                Enable = 1,
                ModuleCode = moduleCode,
                CurrentVersionCode = versionCode
            }.toStrObjDict());

            if (version == null)
            {
                //没有要升级的信息
                return this.MyJson(0,"已经是最新版本");
            }

            return this.MyJson(1,version);
        }

        #endregion

        /// <summary>
        /// 更改站点信息,如设备转科，设备禁用，设备作废等
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifySite()
        {


            return this.MyJson(new
            {
                Flag = 0,
                Error = ""
            });
        }
    }
}
