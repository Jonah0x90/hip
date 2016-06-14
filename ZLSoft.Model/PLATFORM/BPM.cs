using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PLATFORM
{
    public class BPM:BuzzModel 
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        public string SuperID { get; set; }


        /// <summary>
        /// 服务ID
        /// 
        /// </summary>
        public string ServiceID { get; set; }


        /// <summary>
        /// 是否自动执行
        /// </summary>
        public int IsAutomatic { get; set; }

        /// <summary>
        /// 执行计划
        /// </summary>
        public string PlanContent { get; set; }

        /// <summary>
        /// 是否系统内置
        /// </summary>
        public int IsSystem { get; set; }


        [ScriptIgnore]
        public DateTime? LastTime { get; set; }



        public override string GetModelName()
        {
            return GetType().Name;
        }

        public override string GetTableName()
        {
            return "系统_数据导入计划";
        }
    }
}
