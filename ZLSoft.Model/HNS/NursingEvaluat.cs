using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.HNS
{
    /// <summary>
    ///HNS_病人护理评估
    /// <summary>
    public class NursingEvaluat : BuzzModel
    {
        /// <summary>
        /// 计划方案ID
        /// <summary>
        public string SchemeID { get; set; }

        /// <summary>
        /// 评估方法
        /// <summary>
        public string EvaluatFun { get; set; }

        /// <summary>
        /// 评估人
        /// <summary>
        public string Evaluator { get; set; }

        /// <summary>
        /// 评估时间
        /// <summary>
        public DateTime EvaluatTime { get; set; }

        /// <summary>
        /// 签名ID
        /// <summary>
        public string SignID { get; set; }

        /// <summary>
        /// 评估理由
        /// <summary>
        public string EvaluatReason { get; set; }

        /// <summary>
        /// 状态 未评估:0
        /// 部分达标:1
        /// 完全达标:2
        /// 未达标:3
        /// <summary>
        public int Status { get; set; }

        public override string GetModelName()
        {
            return "NursingEvaluat";
        }

        public override string GetTableName()
        {
            return "HNS_病人护理评估";
        }
    }
}
