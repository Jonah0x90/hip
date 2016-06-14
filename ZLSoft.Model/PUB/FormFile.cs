using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PUB
{
    /// <summary>
    /// 表单版本文件
    /// </summary>
    public class FormFile:BuzzModel
    {
        /// <summary>
        /// 表单ID
        /// </summary>
        public string FormID { get; set; }

        /// <summary>
        /// 页数
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 页号
        /// </summary>
        public int PageNo { get; set; }

        /// <summary>
        /// 网页内容
        /// </summary>
        public string PageContent { get; set; }

        /// <summary>
        /// 配套文档
        /// </summary>
        public string Docment { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string VersionCode { get; set; }

        /// <summary>
        /// 表单文件结构
        /// </summary>
        public string FormStruct { get; set; }

        /// <summary>
        /// 表单文件结构A
        /// </summary>
        public string FormStructA { get; set; }

        /// <summary>
        /// 表单文件结构B
        /// </summary>
        public string FormStructB { get; set; }

        /// <summary>
        /// 表单文件结构C
        /// </summary>
        public string FormStructC { get; set; }

        /// <summary>
        /// 移动端样式
        /// </summary>
        public string MobileStyle { get; set; }

        /// <summary>
        /// 桌面端样式
        /// </summary>
        public string DesktopStyle { get; set; }

        /// <summary>
        /// 网页内容A
        /// </summary>
        /// <returns></returns>
        public string PageContentA { get; set; }

        /// <summary>
        /// 网页内容B
        /// </summary>
        /// <returns></returns>
        public string PageContentB { get; set; }

        /// <summary>
        /// 网页内容C
        /// </summary>
        /// <returns></returns>
        public string PageContentC { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateUser { get; set; }

        /// <summary>
        /// 修改说明
        /// </summary>
        public string UpdateRemark { get; set; }

        /// <summary>
        /// 发布人
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime IssueDateTime { get; set; }

        /// <summary>
        /// 发布说明
        /// </summary>
        public string IssueRemark { get; set; }

        /// <summary>
        /// 报表关联ID
        /// </summary>
        public string RelatedReportID { get; set; }

        public override string GetModelName()
        {
            
            return "FormFile";
        }

        public override string GetTableName()
        {
            return "PUB_表单版本文件";
        }
    }
}
