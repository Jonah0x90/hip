using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PUB
{
    /// <summary>
    ///HNS_护理元素
    /// <summary>
    public class ElementType : BuzzModel
    {
        /// <summary>
        /// ID
        /// <summary>
        public string ID { get; set; }

        /// <summary>
        /// 分类ID
        /// <summary>
        public string TypeID { get; set; }

        /// <summary>
        /// 名称
        /// <summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// <summary>
        public string FormType { get; set; }

        /// <summary>
        /// 快捷码
        /// <summary>
        public string FastCode { get; set; }

        /// <summary>
        /// 校验规则
        /// <summary>
        public string ValidationRules { get; set; }

        /// <summary>
        /// 表现形式
        /// <summary>
        public string FormOfExpression { get; set; }

        /// <summary>
        /// 格式串
        /// <summary>
        public string FormatString { get; set; }

        /// <summary>
        /// 数据字段
        /// <summary>
        public string DataField { get; set; }

        /// <summary>
        /// 数据值域
        /// <summary>
        public string ValueRange { get; set; }

        /// <summary>
        /// 项目ID
        /// <summary>
        public string ProjectID { get; set; }

        /// <summary>
        /// 业务含义
        /// <summary>
        public string Implication { get; set; }

        public override string GetModelName()
        {
            return "ElementType";
        }

        public override string GetTableName()
        {
            return "HNS_护理元素";
        }
    }
}
