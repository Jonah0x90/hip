using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PUB
{
    /// <summary>
    ///PUB_报表文件
    /// <summary>
    public class ReportFormFile : BuzzModel
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
        /// 说明
        /// <summary>
        public string Remark { get; set; }

        /// <summary>
        /// 报表内容
        /// <summary>
        //public string Content { get; set; }

        /// <summary>
        /// 报表参数
        /// <summary>
        //public string Parameter { get; set; }

        /// <summary>
        /// 分类ID
        /// <summary>
        public string TypeID { get; set; }

        /// <summary>
        /// 修改人
        /// <summary>
        public string UpdateUser { get; set; }

        public override string GetModelName()
        {
            return "ReportFormFile";
        }

        public override string GetTableName()
        {
            return "PUB_报表文件";
        }
    }
}
