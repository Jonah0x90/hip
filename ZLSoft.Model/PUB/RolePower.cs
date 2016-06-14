using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PUB
{
    /// <summary>
    /// 系统_角色权限
    /// </summary>
    public class RolePower : BuzzModel
    {
        /// <summary>
        /// 角色ID
        /// <summary>
        public string RoleID { get; set; }

        /// <summary>
        /// 菜单ID
        /// <summary>
        public string MenuID { get; set; }

        /// <summary>
        /// 菜单操作ID
        /// <summary>
        public string PowerID { get; set; }

        public override string GetModelName()
        {
            return "RolePower";
        }

        public override string GetTableName()
        {
            return "系统_角色权限";
        }
    }
}
