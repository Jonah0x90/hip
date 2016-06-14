using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.HNS
{
    /// <summary>
    ///HNS_护理问题方法
    /// <summary>
    public class NursingPlanTarget : BuzzModel
    {
        /// <summary>
        /// 名称
        /// <summary>
        public string Name { get; set; }

        /// <summary>
        /// 代码
        /// <summary>
        public string Code { get; set; }

        /// <summary>
        /// 性质
        /// <summary>
        public int Nature { get; set; }

        /// <summary>
        /// 内容
        /// <summary>
        public string Content { get; set; }

        /// <summary>
        /// 序号
        /// <summary>
        public int SerialNumber { get; set; }

        public override string GetModelName()
        {
            return "NursingPlanTarget";
        }

        public override string GetTableName()
        {
            return "HNS_护理问题方法";
        }
    }

}
