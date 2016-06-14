using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PUB
{
    /// <summary>
    ///PUB_交班记录
    /// <summary>
    public class ChangesTationLogRec : BuzzModel
    {
        /// <summary>
        /// ID
        /// <summary>
        public string ID { get; set; }

        /// <summary>
        /// 病区ID
        /// <summary>
        public string LesionID { get; set; }

        /// <summary>
        /// 标题
        /// <summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// <summary>
        public string Content { get; set; }

        /// <summary>
        /// 记录时间
        /// <summary>
        public DateTime? RecordTime { get; set; }

        /// <summary>
        /// 预留字段
        /// <summary>
        public string Obj { get; set; }

        /// <summary>
        /// 病人ID
        /// <summary>
        public string PatientID { get; set; }

        /// <summary>
        /// 记录人
        /// <summary>
        public string RecordPerson { get; set; }

        public override string GetModelName()
        {
            return "ChangesTationLogRec";
        }

        public override string GetTableName()
        {
            return "PUB_交班记录";
        }
    }
}
