using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLSoft.Model.PUB
{
    /// <summary>
    /// 基础组织机构信息
    /// </summary>
    public class Organization : ReferModel
    {

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }


        /// <summary>
        /// 简称
        /// </summary>
        public string SimpleName { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        ///// <summary>
        ///// 上级ID
        ///// </summary>
        //public string SuperID { get; set; }

        /// <summary>
        /// 上级相关ID
        /// </summary>
        public string SuperRelatID { get; set; }


        /// <summary>
        /// 院区ID
        /// </summary>
        public string AreaID { get; set; }

        /// <summary>
        /// 部门性质.1:科室，2：病区
        /// </summary>
        public int DepartType { get; set; }


        /// <summary>
        /// 部门层级
        /// </summary>
        public int DepartGrade { get; set; }


        /// <summary>
        /// 是否末级
        /// </summary>
        public int IsFinal { get; set; }

        /// <summary>
        /// 排序序号
        /// </summary>
        public int SeqNo { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 简码
        /// </summary>
        public string SimpleCode { get; set; }

        public override string GetModelName()
        {
            return "Organization";
        }

        public override string GetTableName()
        {
            return "系统_组织机构";
        }
    }
}
