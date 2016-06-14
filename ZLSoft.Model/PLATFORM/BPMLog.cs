using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PLATFORM
{
    /// <summary>
    ///系统_BPM记录
    /// <summary>
    public class BPMLog : BuzzModel 
    {

        /// <summary>
        /// BPMID
        /// <summary>
        public string BPMID { get; set; }

        /// <summary>
        /// 完成时间
        /// <summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 执行结果
        /// <summary>
        public string ResultContent { get; set; }

        public override string GetModelName()
        {
            return GetType().Name;
        }

        public override string GetTableName()
        {
            return "系统_BPM记录";
        }
    }
}
