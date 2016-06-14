using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PUB
{
    /// <summary>
    ///PUB_交班日志
    /// </summary>
    public class ChangesTationLog : BuzzModel
    {
        /// <summary>
        /// 交班记录ID
        /// </summary>
        public string ChangesTationLogID { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime? SubmitTime { get; set; }

        /// <summary>
        /// 提交人
        /// </summary>
        public string SubmitPerson { get; set; }

        public override string GetModelName()
        {
            return "ChangesTationLog";
        }

        public override string GetTableName()
        {
            return "PUB_交班日志";
        }
    }
}
