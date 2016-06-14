using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.HNS
{
    /// <summary>
    /// 护理问题
    /// </summary>
    public class NursingQuestion : BuzzModel
    {
        /// <summary>
        /// 名称
        /// <summary>
        public string Name { get; set; }

        /// <summary>
        /// 内容
        /// <summary>
        public string Content { get; set; }

        /// <summary>
        /// 描述
        /// <summary>
        public string Remark { get; set; }

        /// <summary>
        /// 类别
        /// <summary>
        public int Type { get; set; }

        /// <summary>
        /// 类别ID
        /// <summary>
        public int TypeId { get; set; }

        /// <summary>
        /// 序号
        /// <summary>
        public int SerialNumber { get; set; }

        public override string GetModelName()
        {
            return "NursingQuestion";
        }

        public override string GetTableName()
        {
            return "HNS_护理问题";
        }

    }
}
