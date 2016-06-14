using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.HNS
{
    /// <summary>
    ///HNS_病人护理计划方案明细
    /// <summary>
    public class PatientSchemeDetails : BuzzModel
    {
        /// <summary>
        /// 方案ID
        /// <summary>
        public string SchemeID { get; set; }

        /// <summary>
        /// 名称
        /// <summary>
        public string Name { get; set; }

        /// <summary>
        /// 代码
        /// <summary>
        public string Code { get; set; }

        /// <summary>
        /// 内容
        /// <summary>
        public string Content { get; set; }

        /// <summary>
        /// 选项ID
        /// <summary>
        public string TargetID { get; set; }

        /// <summary>
        /// 是否自定义
        /// <summary>
        public int IsCustom { get; set; }

        public override string GetModelName()
        {
            return "PatientSchemeDetails";
        }

        public override string GetTableName()
        {
            return "HNS_病人护理计划方案明细";
        }
    }
}
