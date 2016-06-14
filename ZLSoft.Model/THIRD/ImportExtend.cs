using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.THIRD
{
    public class ImportExtend : BaseModel
    {
        /// <summary>
        /// 对象相关ID
        /// </summary>
        public string ObjectID { get; set; }

        /// <summary>
        /// 扩展信息名
        /// </summary>
        public string ExtendKey { get; set; }
        /// <summary>
        /// 扩展信息值
        /// </summary>
        public string ExtendValue { get; set; }


        public override string GetModelName()
        {
            return GetType().Name;
        }

        public override string GetTableName()
        {
            return "PUB_病人补充信息";
        }
    }
}
