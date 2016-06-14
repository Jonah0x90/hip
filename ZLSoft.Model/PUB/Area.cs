using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLSoft.Model.PUB
{
    /// <summary>
    /// 院区实体
    /// </summary>
    public class Area : ReferModel
    {
        public string Name { get; set; }


        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }


        /// <summary>
        /// 邮编
        /// </summary>
        public string Post { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Telephone { get; set; }


        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }



        public override string GetModelName()
        {
            return "Area";
        }

        public override string GetTableName()
        {
            return "系统_院区";
        }
    }
}
