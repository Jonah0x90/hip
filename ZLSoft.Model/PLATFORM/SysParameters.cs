using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PLATFORM
{
    /// <summary>
    /// 系统参数
    /// </summary>
    public class SysParameters:BuzzModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 说明
        /// </summary>
        public string Explain{get;set;}


        /// <summary>
        /// 值说明
        /// </summary>
        public string ValueExplain { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }


        /// <summary>
        /// 模块ID
        /// </summary>
        public string TypeID { get; set; }

        /// <summary>
        /// 是否SQL扩展
        /// </summary>
        public int IsExtSQL { get; set; }

        /// <summary>
        /// 扩展SQL
        /// </summary>
        public string ExtSQL { get; set; }

        /// <summary>
        /// 是否为系统项
        /// </summary>
        public string IsSystem { get; set; }

        public override string GetModelName()
        {
            return "SysParameters";
        }

        public override string GetTableName()
        {
            return "系统_系统参数";
        }
    }
}
