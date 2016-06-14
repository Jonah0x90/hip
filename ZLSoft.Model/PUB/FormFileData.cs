using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PUB
{
    /// <summary>
    /// PUB_表单数据文件
    /// <summary>
    public class FormFileData : BuzzModel
    {

        /// <summary>
        /// 表单ID
        /// <summary>
        public string FormID { get; set; }

        /// <summary>
        /// 护理单元ID
        /// <summary>
        public string DepartmentID { get; set; }

        /// <summary>
        /// 病人ID
        /// <summary>
        public string PatientID { get; set; }

        /// <summary>
        /// 归档人
        /// <summary>
        public string Archive { get; set; }

        /// <summary>
        /// 归档时间
        /// <summary>
        public string ArchiveDate { get; set; }

        /// <summary>
        /// 版本号
        /// <summary>
        public string VersionCode { get; set; }

        /// <summary>
        /// 页号
        /// </summary>
        public int Pages { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        public override string GetModelName()
        {
            return "FormFileData";
        }

        public override string GetTableName()
        {
            return "PUB_表单数据文件";
        }
    }
}
