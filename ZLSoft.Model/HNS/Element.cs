using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.HNS
{
    /// <summary>
    /// 护理元素
    /// </summary>
    public class Element:BuzzModel
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        public long TypeID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 校验规则
        /// </summary>
        public string CheckRule { get; set; }

        /// <summary>
        /// 表现形式
        /// 00-布尔型，01-文本；02-单选；03-下拉；04-多选；05-时间；06-日期；07-日期与时间；08-计算项；09-评分表
        /// </summary>
        public string DisplayMode { get; set; }

        /// <summary>
        /// 格式串
        /// </summary>
        public string Format { get; set; }


        /// <summary>
        /// 数据字段
        /// </summary>
        public string DataFeild { get; set; }

        /// <summary>
        /// 数据值域
        /// </summary>
        public string DataRange { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        public long ProjectID { get; set; }


        /// <summary>
        /// 业务含义
        /// </summary>
        public string Business { get; set; }

        public override string GetModelName()
        {
            return "Element";
        }

        public override string GetTableName()
        {
            return "HNS_护理元素";
        }
    }
}
