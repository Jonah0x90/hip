

using ZLSoft.Pub.Model;
namespace ZLSoft.Model.HNS
{
    public class PatientMarkDetails : BuzzModel
    { 
        /// <summary>
        /// 评分ID
        /// <summary>
        public string MarkID { get; set; }

        /// <summary>
        /// 评分项目ID
        /// <summary>
        public string TargetID { get; set; }

        /// <summary>
        /// 选择值
        /// <summary>
        public string ChoseValue { get; set; }

        /// <summary>
        /// 分值
        /// <summary>
        public int ScoreValue { get; set; }

        public override string GetModelName()
        {
            return "PatientMarkDetails";
        }

        public override string GetTableName()
        {
            return "HNS_病人护理评分明细";
        }
    }
}
