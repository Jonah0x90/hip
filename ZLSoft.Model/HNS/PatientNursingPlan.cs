using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.HNS
{
    /// <summary>
    /// 病人护理计划
    /// </summary>
    public class PatientNursingPlan:BuzzModel
    {
        /// <summary>
        /// 标准计划ID
        /// </summary>
        public string PlanStandardID { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public string DeptID { get; set; }

        /// <summary>
        /// 对象ID,如病人ID
        /// </summary>
        public string ObjectID { get; set; }

        public override string GetModelName()
        {
            return "PatientNursingPlan";
        }

        public override string GetTableName()
        {
            return "HNS_病人护理计划";
        }
    }
}
