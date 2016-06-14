using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.SYS
{
    /// <summary>
    ///PUB_待办事项明细
    /// <summary>
    public class BacklogDetails : BuzzModel
    {
        /// <summary>
        /// 待办事项ID
        /// <summary>
        public string BacklogID { get; set; }

        /// <summary>
        /// 对象ID
        /// <summary>
        public string ObjectID { get; set; }

        /// <summary>
        /// 表单ID
        /// <summary>
        public string FormID { get; set; }

        /// <summary>
        /// 内容
        /// <summary>
        public string Content { get; set; }

        /// <summary>
        /// 处理人
        /// <summary>
        public string Hander { get; set; }

        /// <summary>
        /// 状态
        /// <summary>
        public int Status { get; set; }

        /// <summary>
        /// 选项ID
        /// </summary>
        public string TargetID { get; set; }

        public override string GetModelName()
        {
            return "BacklogDetails";
        }

        public override string GetTableName()
        {
            return "PUB_待办事项明细";
        }
    }
}
