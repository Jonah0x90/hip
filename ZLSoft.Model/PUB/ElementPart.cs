using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PUB
{
    /// <summary>
    ///HNS_护理要素
    /// <summary>
    public class ElementPart : BuzzModel
    {
        /// <summary>
        /// ID
        /// <summary>
        public string ID { get; set; }

        /// <summary>
        /// 名称
        /// <summary>
        public string Name { get; set; }

        /// <summary>
        /// 快捷码
        /// <summary>
        public string FastCode { get; set; }

        /// <summary>
        /// 内容
        /// <summary>
        public string Content { get; set; }

        /// <summary>
        /// 修改人
        /// <summary>
        public string UpdateUser { get; set; }

        /// <summary>
        /// 上次应用表单
        /// <summary>
        public string LastUseForm { get; set; }

        /// <summary>
        /// 分类ID
        /// <summary>
        public string TypeID { get; set; }

        /// <summary>
        /// 分辨率
        /// <summary>
        public string Resolution { get; set; }

        public override string GetModelName()
        {
            return "ElementPart";
        }

        public override string GetTableName()
        {
            return "HNS_护理要素";
        }
    }
}
