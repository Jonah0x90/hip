using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PLATFORM
{
    public class Menu:BuzzModel
    {
        /// <summary>
        /// 菜单编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 模块代码
        /// </summary>
        public string ModuleCode { get; set; }


        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        public string SuperID { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int Grade { get; set; }

        /// <summary>
        /// 是否末级
        /// </summary>
        public int IsLast { get; set; }

        /// <summary>
        /// 是否关联功能
        /// </summary>
        public int IsRelatFun { get; set; }

        /// <summary>
        /// 功能ID
        /// </summary>
        public string FunID { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; }

        /// <summary>
        /// 打开方式
        /// </summary>
        public int OpenType { get; set; }


        /// <summary>
        /// 是否启用权限
        /// </summary>
        public int IsCtlAuth { get; set; }

        /// <summary>
        /// 是否支持移动设备
        /// </summary>
        public int IsMobileSupport { get; set; }

        /// <summary>
        /// 排序序号
        /// </summary>
        public int SeqNo { get; set; }


        public override string GetModelName()
        {
            return GetType().Name;
        }

        public override string GetTableName()
        {
            return "系统_菜单";
        }
    }
}
