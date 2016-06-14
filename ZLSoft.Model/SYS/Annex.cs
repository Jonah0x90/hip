using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.SYS
{
    /// <summary>
    /// 系统_附件
    /// </summary>
    public class Annex : BuzzModel
    {
        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 附件类别
        /// </summary>
        public string AnnexType { get; set; }

        /// <summary>
        /// 大小
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 后缀名
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// 原始名称
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// 上传后的名称
        /// </summary>
        public string UploadName { get; set; }

        /// <summary>
        /// 缩略图名称
        /// </summary>
        public string ThumbnailName { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifiTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 是否作废
        /// </summary>
        public int IsNullify { get; set; }

        /// <summary>
        /// 下载次数
        /// </summary>
        public int DownLoadNumber { get; set; }

        public override string GetModelName()
        {
            return "Annex";
        }
        public override string GetTableName()
        {
            return "系统_附件";
        }
    }
}
