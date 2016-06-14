using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.HNS
{
    /// <summary>
    /// 护理评分
    /// </summary>
    public class NursingMark : BuzzModel
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }


        public override string GetModelName()
        {
            return "NursingMark";
        }

        public override string GetTableName()
        {
            return "HNS_护理评分";
        }
    }
}
