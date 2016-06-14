using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.SYS
{
    /// <summary>
    /// 汉字库
    /// </summary>
    public class CNLibrary:BuzzModel
    {
        /// <summary>
        /// 汉字
        /// </summary>
        public string Character { get; set; }

        /// <summary>
        /// 全拼
        /// </summary>
        public string Spelling { get; set; }

        /// <summary>
        /// 拼音码
        /// </summary>
        public string PinyinCode { get; set; }

        /// <summary>
        /// 五笔码
        /// </summary>
        public string FivePenCode { get; set; }

        /// <summary>
        /// 角形码
        /// </summary>
        public string CornerShapeCode { get; set; }

        /// <summary>
        /// 多音标志
        /// </summary>
        public int MultiToneMarks { get; set; }


        public override string GetModelName()
        {
            return "CNLibrary";
        }
        public override string GetTableName()
        {
            return "系统_汉字库";
        }
    }
}
