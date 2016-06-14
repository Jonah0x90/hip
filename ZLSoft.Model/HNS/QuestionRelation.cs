using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.HNS
{
    /// <summary>
    ///HNS_护理问题_方法关联
    /// <summary>
    public class QuestionRelation : BuzzModel
    {
        /// <summary>
        /// 问题ID
        /// <summary>
        public string QuestionID { get; set; }

        /// <summary>
        /// 方法ID
        /// <summary>
        public string FunID { get; set; }

        public override string GetModelName()
        {
            return "QuestionRelation";
        }

        public override string GetTableName()
        {
            return "HNS_护理问题_方法关联";
        }
    }
}
