using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Script.Serialization;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.SYS
{
    /// <summary>
    /// 站点设备版本信息
    /// </summary>
    public class SiteVersionInfo:BuzzModel
    {
        [ScriptIgnore]
        public override string ID
        {
            get;
            set;
        }

        /// <summary>
        /// 模块代码
        /// </summary>
        [ScriptIgnore]
        public string ModuleCode { get; set; }

        /// <summary>
        /// 当前版本代码
        /// </summary>
        public int CurrentVersionCode { get; set; }

        /// <summary>
        /// 升级版本代码
        /// </summary>
        [ScriptIgnore]
        public int UpdateVersionCode { get; set; }


        /// <summary>
        /// 升级文件地址
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// 升级帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 升级密码
        /// </summary>
        public string Password { get; set; }


        /// <summary>
        /// 升级方式(1：后台升级，2：立即升级，0：不升级)
        /// </summary>
        public int UpdateMethod { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [ScriptIgnore]
        public int Enable { get; set; }

        public override string GetModelName()
        {
            return "SiteVersionInfo";
        }

        public override string GetTableName()
        {
            return "系统_模块版本";
        }
    }
}
