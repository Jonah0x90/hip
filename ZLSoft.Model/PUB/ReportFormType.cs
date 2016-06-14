using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PUB
{
    /// <summary>
    ///PUB_报表分类
    /// <summary>
    public class ReportFormType : BuzzModel
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
        /// 修改人
        /// <summary>
        public string UpdateUser { get; set; }

        /// <summary>
        /// 说明
        /// <summary>
        public string Remark { get; set; }

        public override string GetModelName()
        {
            return "ReportFormType";
        }

        public override string GetTableName()
        {
            return "PUB_报表分类";
        }
    }
}
