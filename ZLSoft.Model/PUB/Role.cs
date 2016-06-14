using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PUB
{
    /// <summary>
    /// 基础角色信息
    /// </summary>
    public class Role:BuzzModel
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleGroupName { get; set; }

        /// <summary>
        /// 所属模块代码
        /// </summary>
        public string ModuleCode { get; set; }

        /// <summary>
        /// 用户组类型
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        /// 默认载入功能
        /// </summary>
        public string DefalutFunction { get; set; }

        public override string GetModelName()
        {
            return "Role";
        }

        public override string GetTableName()
        {
            return "系统_角色";
        }
    }
}
