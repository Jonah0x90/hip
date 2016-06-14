using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;
using System.Web.Script.Serialization;

namespace ZLSoft.Model.PUB
{
    /// <summary>
    /// 基础用户信息
    /// </summary>
    public class PubUser:ReferModel
    {
        public string UserName { get; set; }


        [ScriptIgnore]
        public string Password { get; set; }


        /// <summary>
        /// 职工相关ID
        /// </summary>
        public string EmRelatID { get; set; }


        public override string GetModelName()
        {
            return this.GetType().Name;
        }

        public override string GetTableName()
        {
            return "系统_用户信息";
        }
    }
}
