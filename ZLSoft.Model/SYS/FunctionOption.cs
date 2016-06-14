using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.SYS
{
    public class FunctionOption:BuzzModel
    {
        /// <summary>
        /// 功能ID
        /// </summary>
        public long FunID { get; set; }

        /// <summary>
        /// 操作代码
        /// </summary>
        public string OptionCode { get; set; }


        /// <summary>
        /// 操作名称
        /// </summary>
        public string OptionName { get; set; }
        
        /// <summary>
        /// 排序序号
        /// </summary>
        public string SeqNo { get; set; }


        public override string GetModelName()
        {
            return "FunctionOption";
        }

        public override string GetTableName()
        {
            return "系统_功能操作";
        }
    }
}
