using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PUB
{
    /// <summary>
    /// 系统_角色成员
    /// </summary>
    public class RoleMember : BuzzModel
    {
        /// <summary>
        /// 角色ID
        /// <summary>
        public string RoleID { get; set; }

        /// <summary>
        /// 用户ID
        /// <summary>
        public string UserID { get; set; }

        public override string GetModelName()
        {
            return "RoleMember";
        }

        public override string GetTableName()
        {
            return "系统_角色成员";
        }
    }
}
