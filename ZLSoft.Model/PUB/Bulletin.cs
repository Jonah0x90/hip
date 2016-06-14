using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PUB
{
    /// <summary>
    ///PUB_公告栏
    /// <summary>
    public class Bulletin : BuzzModel
    {

         /// <summary>
        /// 上级ID
        /// <summary>
        public string SuperID { get; set; }

        /// <summary>
        /// 名称
        /// <summary>
        public string Name { get; set; }

        /// <summary>
        /// 说明
        /// <summary>
        public string Remark { get; set; }

        /// <summary>
        /// 适用科室
        /// <summary>
        public string DeptRange { get; set; }

        /// <summary>
        /// 扩展SQL
        /// <summary>
        public string ExtSQL { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 样式
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// 字号
        /// </summary>
        public string FontSize { get; set; }

        public override string GetModelName()
        {
            return "Bulletin";
        }

        public override string GetTableName()
        {
            return "PUB_公告栏";
        }
    }
}
