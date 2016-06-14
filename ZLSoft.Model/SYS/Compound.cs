using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.SYS
{
    /// <summary>
    /// 院区
    /// </summary>
    public class Compound:BuzzModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 是否作废
        /// </summary>
        public int IsNullify { get; set; }

        /// <summary>
        /// 作废日期
        /// </summary>
        public DateTime? NullifyDate { get; set; }

        /// <summary>
        /// 修改用户
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime? ModifiDate { get; set; }

        /// <summary>
        /// 输入码
        /// </summary>
        public string InputCodes { get; set; }

        /// <summary>
        /// 排序序号
        /// </summary>
        public int SortNumber { get; set; }

        public override string GetModelName()
        {
            return "Compound";
        }
        public override string GetTableName()
        {
            return "系统_院区";
        }
    }
}
