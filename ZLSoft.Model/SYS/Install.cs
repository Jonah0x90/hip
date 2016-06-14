using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLSoft.Model.SYS
{
    public class Install
    {
        /// <summary>
        /// SID
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 主机名
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 口令
        /// </summary>
        public string Password { get; set; }
    }
}
