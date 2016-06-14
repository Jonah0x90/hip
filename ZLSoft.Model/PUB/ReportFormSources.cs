using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PUB
{
    /// <summary>
    ///PUB_报表数据源
    /// <summary>
    public class ReportFormSources : BuzzModel
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
        /// 内容
        /// <summary>
        public string Content { get; set; }

        /// <summary>
        /// 参数
        /// <summary>
        public string Parameter { get; set; }

        /// <summary>
        /// 三方数据源ID
        /// <summary>
        public string ThirdSourcesID { get; set; }

        /// <summary>
        /// 修改人
        /// <summary>
        public string UpdateUser { get; set; }

        public string ReportID { get; set; }

        public override string GetModelName()
        {
            return "ReportFormSources";
        }

        public override string GetTableName()
        {
            return "PUB_报表数据源";
        }
    }
}
