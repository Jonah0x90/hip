using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PLATFORM
{
    /// <summary>
    /// 系统_公共代码
    /// </summary>
    public class CommonCode:BuzzModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类别ID
        /// </summary>
        public string CateID { get; set; }

        /// <summary>
        /// 系统标记
        /// </summary>
        public int SystemLabel { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 编码规则
        /// </summary>
        public string CodingRule { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        public string SuperID { get; set; }

        /// <summary>
        /// 是否扩展SQL
        /// </summary>
        public int IsExtSQL { get; set; }

        /// <summary>
        /// 扩展SQL
        /// </summary>
        public string ExtSQL { get; set; }

        public override string GetModelName()
        {
            return "CommonCode";
        }

        public override string GetTableName()
        {
            return "系统_公共代码";
        }
    }
}
