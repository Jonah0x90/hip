using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PLATFORM
{
    /// <summary>
    ///系统_三方数据源
    /// <summary>
    public class DataSource : BuzzModel
    {

        /// <summary>
        /// 数据源名称
        /// <summary>
        public string DataSourceName { get; set; }

        /// <summary>
        /// 数据源提供者
        /// <summary>
        public string Provider { get; set; }

        /// <summary>
        /// 地址
        /// <summary>
        public string Url { get; set; }

        /// <summary>
        /// 用户名
        /// <summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// <summary>
        public string Password { get; set; }

        /// <summary>
        /// 数据库
        /// <summary>
        public string Database { get; set; }

        /// <summary>
        /// 连接参数
        /// <summary>
        public string ConnectParams { get; set; }

        /// <summary>
        /// 应用数据源配置文件
        /// <summary>
        [ScriptIgnore]
        public string ConfigFile { get; set; }

        public override string GetModelName()
        {
            return GetType().Name;
        }

        public override string GetTableName()
        {
            return "系统_三方数据源";
        }
    }
}