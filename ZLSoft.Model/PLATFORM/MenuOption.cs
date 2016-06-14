using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PLATFORM
{
    public class MenuOption:BuzzModel
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public string MenuID { get; set; }

        /// <summary>
        /// 是否控制权限
        /// </summary>
        public int IsCtlAuth { get; set; }

        /// <summary>
        /// 操作代码
        /// </summary>
        public string OptionCode { get; set; }


        /// <summary>
        /// 功能操作ID
        /// </summary>
        public string FuncOptID { get; set; }

        public override string GetModelName()
        {
            return "MenuOption";
        }

        public override string GetTableName()
        {
            return "系统_菜单操作";
        }
    }
}
