using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PLATFORM
{
    public class DataService:BuzzModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 入参模板
        /// </summary>
        public string InputTemplate { get; set; }


        /// <summary>
        /// 出参模板
        /// </summary>
        public string OutputTemplate { get; set; }

        /// <summary>
        /// 是否异步
        /// </summary>
        public int IsAsync { get; set; }

        /// <summary>
        /// 数据源,当Type=4时，该值为三方数据源ID,否则为URL
        /// </summary>
        public string DataSource { get; set; }

        public int IsLocalStorage { get; set; }

        /// <summary>
        /// 是否系统内置
        /// </summary>
        public int IsSystem { get; set; }

        /// <summary>
        /// 是否对照表对象
        /// </summary>
        public int IsRef { get; set; }

        /// <summary>
        /// 对照表对象
        /// </summary>
        public string RefObject { get; set; }

        public override string GetModelName()
        {
            return GetType().Name;
        }

        public override string GetTableName()
        {
            return "系统_三方服务定义";
        }
    }
}
