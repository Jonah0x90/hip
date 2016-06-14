using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.SYS
{
    /// <summary>
    /// 树定义
    /// </summary>
    public class TreeDefinition:BuzzModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 脚本_ORC
        /// </summary>
        public string OrcScript { get; set; }

        /// <summary>
        /// 脚本_MSSQL
        /// </summary>
        public string MssqlScript { get; set; }

        /// <summary>
        /// 脚本_MYSQL
        /// </summary>
        public string MysqlScript { get; set; }


        /// <summary>
        /// 是否系统树
        /// </summary>
        public int IsSysTree { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Explain { get; set; }

        /// <summary>
        /// 是否多级
        /// </summary>
        public int IsMultilayer { get; set; }

        /// <summary>
        /// 根节点名称
        /// </summary>
        public string RootName { get; set; }

        /// <summary>
        /// 根节点格式
        /// </summary>
        public string NodeFormat { get; set; }

        /// <summary>
        /// 是否显示根节点
        /// </summary>
        public int IsRootVisable { get; set; }

        /// <summary>
        /// 根节点生成方式
        /// </summary>
        public int RootBuildMethod { get; set; }


        public override string GetModelName()
        {
            return this.GetType().Name;
        }

        public override string GetTableName()
        {
            return "系统_树定义";
        }
    }
}
