using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.SYS
{
    /// <summary>
    /// 站点设备
    /// </summary>
    public class SiteDevice:BuzzModel
    {
        public string DeviceID { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 所属部门ID
        /// </summary>
        public string DeptID { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? RegTime { get; set; }


        /// <summary>
        /// 设备型号
        /// </summary>
        public string DeviceInfo { get; set; }


        /// <summary>
        /// 是否移动设备
        /// </summary>
        public int IsMobile { get; set; }


        /// <summary>
        /// 是否启用
        /// </summary>
        public int Enable { get; set; }

        public override string GetModelName()
        {
            return "SiteDevice";
        }

        public override string GetTableName()
        {
            return "系统_站点设备";
        }
    }
}
