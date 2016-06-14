using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.HNS
{
    /// <summary>
    /// 护理计划(标准)
    /// </summary>
    public class NursingPlan : BuzzModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 性质，1-按专科制定如内科标准护理计划；2-按系统制定如循环系统标准护理计划、呼吸系统标准护理计划；3-按疾病制定如心绞痛标准护理计划、胆石症标准护理计划；4-按一个护理诊断如有皮肤完整性受损的危险等标准护理计划
        /// </summary>
        public int Nature { get; set; }

        /// <summary>
        /// 适合对象，1-内科；2-循环系统；3-心绞痛；4-有皮肤完整性受损的危险
        /// </summary>
        public int Object { get; set; }

        /// <summary>
        /// 是否系统，是否全院或者科室计划，如果是全院计划，则不允许修改
        /// </summary>
        public int IsSys { get; set; }

        /// <summary>
        /// 适用科室,科室ID用逗号隔开
        /// </summary>
        public string DeptRange { get; set; }

        public override string GetModelName()
        {
            return "NursingPlan";
        }

        public override string GetTableName()
        {
            return "HNS_护理计划";
        }

    }
}
