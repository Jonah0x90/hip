using System;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.HNS
{ 
    /// <summary>
    ///HNS_病人护理评分
    /// <summary>
    public class PatientCareMark : BuzzModel
    {
        /// <summary>
        /// ID
        /// <summary>
        public string ID { get; set; }

        /// <summary>
        /// 评分表ID
        /// <summary>
        public string MarkTabID { get; set; }

        /// <summary>
        /// 评分人
        /// <summary>
        public string MarkUser { get; set; }

        /// <summary>
        /// 评分时间
        /// <summary>
        public DateTime MarkTime { get; set; }

        /// <summary>
        /// 评分结果
        /// <summary>
        public int MarkResult { get; set; }

        /// <summary>
        /// 评分结论
        /// <summary>
        public string MarkVerdict { get; set; }

        /// <summary>
        /// 病人ID
        /// <summary>
        public string PatientID { get; set; }

        /// <summary>
        /// 表单记录ID
        /// <summary>
        public string FormID { get; set; }

        /// <summary>
        /// 问题ID
        /// <summary>
        public string QuestionID { get; set; }

        /// <summary>
        /// 诊断ID
        /// <summary>
        public string DiagnosisID { get; set; }

        public override string GetModelName()
        {
            return "PatientCareMark";
        }

        public override string GetTableName()
        {
            return "HNS_病人护理评分";
        }
    }
}
