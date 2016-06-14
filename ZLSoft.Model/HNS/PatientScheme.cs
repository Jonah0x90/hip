using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.HNS
{
    /// <summary>
    ///HNS_病人护理计划方案
    /// <summary>
    public class PatientScheme : BuzzModel
    {
        /// <summary>
        /// 类型
        /// <summary>
        public int FormType { get; set; }

        /// <summary>
        /// ID
        /// <summary>
        public string ID { get; set; }

        /// <summary>
        /// 病人计划ID
        /// <summary>
        public string PatientPlanID { get; set; }

        /// <summary>
        /// 问题ID
        /// <summary>
        public string QuestionID { get; set; }

        public override string GetModelName()
        {
            return "PatientScheme";
        }

        public override string GetTableName()
        {
            return "HNS_病人护理计划方案";
        }
    }
}
