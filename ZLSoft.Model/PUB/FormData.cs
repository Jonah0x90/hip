using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PUB
{
    /// <summary>
    ///PUB_表单数据
    /// <summary>
    public class FormData : BuzzModel
    {
        /// <summary>
        /// 表单文件ID
        /// <summary>
        public string ID { get; set; }

        /// <summary>
        /// 表单结构ID
        /// <summary>
        public string FSID { get; set; }

        /// <summary>
        /// 数据内容
        /// <summary>
        public string DataText { get; set; }

        /// <summary>
        /// 版本号
        /// <summary>
        public string VersionCode { get; set; }
        
        /// <summary>
        /// 表单ID
        /// </summary>
        public string FormID { get; set; }


        public override string GetModelName()
        {
            return "FormData";
        }

        public override string GetTableName()
        {
            return "PUB_表单数据";
        }
    }
}
