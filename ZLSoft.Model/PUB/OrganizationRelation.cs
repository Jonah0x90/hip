using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLSoft.Model.PUB
{
    /// <summary>
    /// 基础组织机构关联信息
    /// </summary>
    
    public class OrganizationRelation : ReferModel
    {
        /// <summary>
        /// 病区相关ID
        /// </summary>
        public string LesionID
        {
            get;
            set;
        }

        /// <summary>
        /// 科室相关ID
        /// </summary>
        public string DepartmentID
        {
            get;
            set;
        }

        public override string GetModelName()
        {
            return "OrganizationRelation";
        }

        public override string GetTableName()
        {
            return "系统_组织机构关联";
        }
    }
}
