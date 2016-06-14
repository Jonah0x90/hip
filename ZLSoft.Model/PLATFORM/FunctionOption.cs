using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PLATFORM
{
    /// <summary>
    ///系统_功能操作
    /// <summary>
    public class FunctionOption : BuzzModel
    {
        /// <summary>
        /// 操作地址
        /// <summary>
        public string OptUrl { get; set; }

        /// <summary>
        /// 功能ID
        /// <summary>
        public string FunID { get; set; }

        /// <summary>
        /// 操作代码
        /// <summary>
        public string OptionCode { get; set; }

        /// <summary>
        /// 操作名称
        /// <summary>
        public string OptionName { get; set; }


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
