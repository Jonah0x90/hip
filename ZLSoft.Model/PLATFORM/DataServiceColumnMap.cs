using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PLATFORM
{
    /// <summary>
    ///PUB_三方服务字段对照
    /// <summary>
    public class DataServiceColumnMap:BuzzModel
    {
        /// <summary>
        /// 服务ID
        /// <summary>
        public string ServiceID { get; set; }

        /// <summary>
        /// 字段
        /// <summary>
        public string LocalColumn { get; set; }

        /// <summary>
        /// 三方字段
        /// <summary>
        public string ThirdColumn { get; set; }

        public override string GetModelName()
        {
            return GetType().Name;
        }

        public override string GetTableName()
        {
            return "系统_三方服务字段对照";
        }
    }
}
