using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.HNS
{
    /// <summary>
    ///PUB_病人补充信息
    /// <summary>
    public class NursingPatientExtra : BuzzModel
    {
        /// <summary>
        /// 对象相关ID
        /// <summary>
        public string RelatID { get; set; }

        /// <summary>
        /// 信息名
        /// <summary>
        public string InfoName { get; set; }

        /// <summary>
        /// 信息值
        /// <summary>
        public string InfoValue { get; set; }

        public override string GetModelName()
        {
            return "NursingPatientExtra";
        }

        public override string GetTableName()
        {
            return "PUB_病人补充信息";
        }
    }
}
