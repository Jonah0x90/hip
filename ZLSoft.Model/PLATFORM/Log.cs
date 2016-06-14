using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PLATFORM
{
    public class Log:BaseModel
    {

        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 日志功能
        /// </summary>
        public string Function { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        public int Level { get; set; }




        public override string GetModelName()
        {
            return "Log";
        }

        public override string GetTableName()
        {
            return "系统_日志管理";
        }
    }
}
