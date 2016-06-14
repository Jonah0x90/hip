using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PLATFORM
{

    /// <summary>
    /// 系统模块
    /// </summary>
    public class Module:BuzzModel 
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否自定义首页
        /// </summary>
        public int IsIndexCustom { get; set; }


        /// <summary>
        /// 首页菜单ID
        /// </summary>
        public int IndexMenuID { get; set; }

        /// <summary>
        /// 是否检查客户端版本
        /// </summary>
        public int IsCheckClient { get; set; }

        /// <summary>
        /// 类别ID
        /// </summary>
        public int TypeID { get; set; }

        /// <summary>
        /// 模块代码
        /// </summary>
        public string ModuleCode { get; set; }

        public override string GetModelName()
        {
            return "Module";
        }

        public override string GetTableName()
        {
            return "系统_模块";
        }
    }
}
