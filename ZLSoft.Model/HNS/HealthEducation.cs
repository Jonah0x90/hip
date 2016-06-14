using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.HNS
{
    public class HealthEducation:BuzzModel
    {
        /// <summary>
        /// 表单ID
        /// 
        /// </summary>
        public long FormID { get; set; }

        /// <summary>
        /// 病人ID
        /// </summary>
        public string InpatientID { get; set; }
        /// <summary>
        /// 对象
        /// </summary>
        public string Object { get; set; }

        /// <summary>
        /// 评价效果
        /// </summary>
        public string Effect { get; set; }

        public override string GetModelName()
        {
            return "HealthEducation";
        }

        public override string GetTableName()
        {
            return "HNS_健康宣教执行";
        }
    }
}
