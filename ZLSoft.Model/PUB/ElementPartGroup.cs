using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PUB
{
    /// <summary>
    ///HNS_护理要素分类
    /// <summary>
    public class ElementPartGroup : BuzzModel
    {
        /// <summary>
        /// ID
        /// <summary>
        public string ID { get; set; }

        /// <summary>
        /// 上级ID
        /// <summary>
        public string SuperID { get; set; }

        /// <summary>
        /// 编号
        /// <summary>
        public string No { get; set; }

        /// <summary>
        /// 名称
        /// <summary>
        public string Name { get; set; }

        /// <summary>
        /// 说明
        /// <summary>
        public string Remark { get; set; }

        public override string GetModelName()
        {
            return "ElementPartGroup";
        }

        public override string GetTableName()
        {
            return "HNS_护理要素分类";
        }
    }
}
