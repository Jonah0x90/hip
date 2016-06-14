using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.HNS
{
    /// <summary>
    /// 护理评分参考
    /// </summary>
    public class NursingMarkRefer : BuzzModel
    {
        public string ID { get; set; }
        /// <summary>
        /// 评分表ID
        /// </summary>
        public string MarkTabID { get; set; }

        /// <summary>
        /// 结论
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public float Min { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public float Max { get; set; }


        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否触发
        /// </summary>
        public int IsTrigger { get; set; }


        /// <summary>
        /// 是否为风险项目
        /// </summary>
        public int IsRisk { get; set; }

        /// <summary>
        /// 待办事项
        /// </summary>
        public string Backlogs { get; set; }

        public override string GetModelName()
        {
            return "NursingMarkRefer";
        }

        public override string GetTableName()
        {
            return "HNS_护理评分参考";
        }
    }
}
