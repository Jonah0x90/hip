using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.HNS
{
    /// <summary>
    /// 护理计划明细
    /// </summary>
    public class NursingPlanDetails:BuzzModel
    {
        /// <summary>
        /// 计划ID
        /// <summary>
        public string PlanID { get; set; }

        /// <summary>
        /// 问题ID
        /// <summary>
        public string QuestionID { get; set; }

        /// <summary>
        /// 选项业务ID
        /// <summary>
        public string PlanTargetID { get; set; }


        public override string GetModelName()
        {
            return "NursingPlanDetails";
        }

        public override string GetTableName()
        {
            return "HNS_护理计划明细";
        }
    }
}
