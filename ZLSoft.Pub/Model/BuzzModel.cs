using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;


namespace ZLSoft.Pub.Model
{
    public abstract class BuzzModel:BaseModel
    {
        


        #region 公共属性


        /// <summary>
        /// 是否作废
        /// </summary>
        public int IsInvalid { get; set; }

        [ScriptIgnore]
        public string InvalidTimeTxt
        {
            get
            {
                if (InvalidTime == null)
                {
                    return string.Empty;
                }
                return InvalidTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }


        /// <summary>
        /// 作废时间
        /// </summary>
        [ScriptIgnore]
        public DateTime? InvalidTime
        {
            get;
            set;
        }

        

        /// <summary>
        /// 输入码
        /// </summary>
        public string InputCode { get; set; }



        /// <summary>
        /// 排序序号
        /// </summary>
        [ScriptIgnore]
        public int SeqNo { get; set; }


        #endregion
    }
}
