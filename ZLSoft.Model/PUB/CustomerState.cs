using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLSoft.Model.PUB
{
    public class CustomerState:ReferModel
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDateTime { get; set; }


        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// 导入时间
        /// </summary>
        public DateTime ImportTime { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        public string User { get; set; }

        public override string GetModelName()
        {
            return GetType().Name;
        }

        public override string GetTableName()
        {
            return "PUB_病人状态";
        }
    }
}
