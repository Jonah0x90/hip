using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PUB
{
    /// <summary>
    ///PUB_表单结构
    /// <summary>
    public class FormStructure : BuzzModel
    {


        /// <summary>
        /// 表单ID
        /// <summary>
        public string FormID { get; set; }

        /// <summary>
        /// 版本号
        /// <summary>
        public string VersionCode { get; set; }

        /// <summary>
        /// 元素名称
        /// <summary>
        public string ElementName { get; set; }

        /// <summary>
        /// 元素属性
        /// </summary>
        public string ElementAttribute { get;set; }

        /// <summary>
        /// 业务含义
        /// </summary>
        public string Implication { get; set; }

        /// <summary>
        /// 表现形式
        /// <summary>
        public int FormOfExpression { get; set; }

        /// <summary>
        /// 表单样式
        /// </summary>
        public string FormStyleCss { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        public string SuperID { get; set; }

        /// <summary>
        /// 排序序号
        /// </summary>
        public string NO { get; set; }
        public override string GetModelName()
        {
            return "FormStructure";
        }

        public override string GetTableName()
        {
            return "PUB_表单结构";
        }
    }
}
