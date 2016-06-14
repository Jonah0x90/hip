using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PLATFORM
{
    /// <summary>
    ///系统_公共代码明细
    /// <summary>
    public class CommonCodeDetail : BuzzModel
    {
        /// <summary>
        /// 数据值域
        /// <summary>
        public string ValueRange { get; set; }

        /// <summary>
        /// 数据字段
        /// <summary>
        public string DataField { get; set; }

        /// <summary>
        /// ID
        /// <summary>
        public string ID { get; set; }

        /// <summary>
        /// 代码ID
        /// <summary>
        public string CodeID { get; set; }

        /// <summary>
        /// 明细名称
        /// <summary>
        public string Name { get; set; }

        /// <summary>
        /// 上级ID
        /// <summary>
        public string SuperID { get; set; }

        /// <summary>
        /// 级别
        /// <summary>
        public string Level { get; set; }

        /// <summary>
        /// 是否末级
        /// <summary>
        public string IsFinal { get; set; }

        /// <summary>
        /// 修改日期
        /// <summary>
        public string ModifyTime { get; set; }

        /// <summary>
        /// 作废日期
        /// <summary>
        public string InvalidTime { get; set; }

        /// <summary>
        /// 备注
        /// <summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 排序序号
        /// </summary>
        public string SortNumber { get; set; }

        public override string GetModelName()
        {
            return "CommonCodeDetail";
        }

        public override string GetTableName()
        {
            return "系统_公共代码明细";
        }
    }
}
