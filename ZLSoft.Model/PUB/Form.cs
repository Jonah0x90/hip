using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PUB
{
    /// <summary>
    /// 表单
    /// </summary>
    public class Form:BuzzModel
    {

        public string ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 表单类型(1-评估表；2-健康宣教单；3-宣教执行单；4-自定义表单；9-临床路径表单)
        /// </summary>
        public int FormType { get; set; }

        /// <summary>
        /// 快捷码
        /// </summary>
        public string FastCode { get; set; }

        /// <summary>
        /// 子码(健康宣教单有效：1-入院；2-疾病知识；3-特殊检查；4-用药；5-术前；6-术后；7-出院；8-心理；9-饮食；10-其他)
        /// </summary>
        public int MarkCode { get; set; }

        /// <summary>
        /// 适用科室
        /// </summary>
        public string DeptRange { get; set; }

        /// <summary>
        /// 最新版本号
        /// </summary>
        public string NewVersionCode { get; set; }

        /// <summary>
        /// 表单对照
        /// </summary>
        public string FormAgainst { get; set; }

        /// <summary>
        /// 设计器版本
        /// </summary>
        public string DesignVersion { get; set; }

        /// <summary>
        /// 分辨率
        /// </summary>
        public string Resolution { get; set; }

        public override string GetModelName()
        {
            return "Form";
        }

        public override string GetTableName()
        {
            return "PUB_表单";
        }
    }
}
