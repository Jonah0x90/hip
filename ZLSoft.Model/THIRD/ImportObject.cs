using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.THIRD
{
    /// <summary>
    ///系统_三方数据导入对象
    /// <summary>
    public class ImportObject : BuzzModel
    {

        public string ID { get; set; }

        /// <summary>
        /// 表名
        /// <summary>
        public string TableName { get; set; }

        /// <summary>
        /// 导入时间
        /// <summary>
        public DateTime? ImportTime { get; set; }

        /// <summary>
        /// 导入记录数
        /// <summary>
        public int ImportDataNumber { get; set; }

        /// <summary>
        /// 耗时
        /// </summary>
        public decimal TakeTime { get; set; }


        /// <summary>
        /// 修改记录数
        /// <summary>
        public int UpdateDataNumber { get; set; }

        /// <summary>
        /// 标识列
        /// </summary>
        public string Identifying { get; set; }

        /// <summary>
        /// 扩展表
        /// </summary>
        public string ExtendTable { get; set; }

        public override string GetModelName()
        {
            return GetType().Name;
        }

        public override string GetTableName()
        {
            return "系统_三方数据导入对象";
        }
    }
}
