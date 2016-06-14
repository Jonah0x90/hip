using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.SYS
{
    /// <summary>
    /// 三方数据源
    /// </summary>
    public class OtherDataSource:BuzzModel
    {
        /// <summary>
        /// 数据源名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据源提供者
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 数据库
        /// </summary>
        public string DataBase { get; set; }

        /// <summary>
        /// 连接参数
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 应用程序配置文件
        /// </summary>
        public string AppSetting { get; set; }


        public override string GetModelName()
        {
            return "OtherDataSource";
        }

        public override string GetTableName()
        {
            return "系统_三方数据源";
        }

    }
}
