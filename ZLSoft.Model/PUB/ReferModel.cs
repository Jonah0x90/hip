using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.PUB
{
    /// <summary>
    /// 公共实体父类
    /// </summary>
    public abstract class ReferModel:BuzzModel
    {
        /// <summary>
        /// 相关ID
        /// </summary>
        [ScriptIgnore]
        public string RelatID
        {
            get;
            set;
        }

        /// <summary>
        /// 是否参照三方
        /// </summary>
        [ScriptIgnore]
        public int IsBaseThird { get; set; }
    }
}
