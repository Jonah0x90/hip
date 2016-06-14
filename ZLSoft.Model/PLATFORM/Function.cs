using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PLATFORM
{
    public class Function:BuzzModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 表现方式
        /// </summary>
        public int MethodType { get; set; }

        /// <summary>
        /// 参数说明
        /// </summary>
        public string ParamDesc { get; set; }

        /// <summary>
        /// 默认参数值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 功能类别代码
        /// </summary>
        public string FunctionCode { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 是否支持移动
        /// </summary>
        public int IsSupportMobile { get; set; }


        public override string GetModelName()
        {
            return "Function";
        }

        public override string GetTableName()
        {
            return "系统_功能";
        }
    }
}
