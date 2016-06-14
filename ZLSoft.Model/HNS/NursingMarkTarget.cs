using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.HNS
{
    /// <summary>
    /// 护理评分项目
    /// </summary>
    public class NursingMarkTarget:BuzzModel
    {
        /// <summary>
        /// ID
        /// <summary>
        public string ID { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }


        /// <summary>
        /// 评分表ID
        /// <summary>
        public string MarkTabID { get; set; }

        /// <summary>
        /// 名称
        /// <summary>
        public string Name { get; set; }

        /// <summary>
        /// 说明
        /// <summary>
        public string Remark { get; set; }

        /// <summary>
        /// 序号
        /// <summary>
        public int SerialNumber { get; set; }

        /// <summary>
        /// 数据值域
        /// <summary>
        public string ValueRange { get; set; }

        /// <summary>
        /// 分值
        /// <summary>
        public string ScoreValue { get; set; }

        /// <summary>
        /// 是否多选
        /// <summary>
        public int IsMultiple { get; set; }

        /// <summary>
        /// 元素ID
        /// <summary>
        public string ElementID { get; set; }

        /// <summary>
        /// 分值说明
        /// <summary>
        public string ScoreRemark { get; set; }

        public override string GetModelName()
        {
            return "NursingMarkTarget";
        }

        public override string GetTableName()
        {
            return "HNS_护理评分项目";
        }
    }
}
