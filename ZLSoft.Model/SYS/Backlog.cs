using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.SYS
{
    /// <summary>
    ///系统_待办事项
    /// <summary>
    public class Backlog : BuzzModel
    {
        /// <summary>
        /// ID
        /// <summary>
        public string ID { get; set; }

        /// <summary>
        /// 名称
        /// <summary>
        public string Name { get; set; }

        /// <summary>
        /// 对象ID
        /// <summary>
        public string ObjectID { get; set; }

        /// <summary>
        /// 内容
        /// <summary>
        public string Content { get; set; }

        /// <summary>
        /// 修改人
        /// <summary>
        public string UpdateUser { get; set; }

        /// <summary>
        /// 类型
        /// <summary>
        public int FormType { get; set; }

        /// <summary>
        /// 处理者类型
        /// <summary>
        public int HanderType { get; set; }

        /// <summary>
        /// 默认处理对象
        /// <summary>
        public string DefaultObject { get; set; }

        /// <summary>
        /// 所属范围
        /// <summary>
        public string Range { get; set; }

        public string URL { get; set; }

        public string Params { get; set; }

        public override string GetModelName()
        {
            return "Backlog";
        }

        public override string GetTableName()
        {
            return "系统_待办事项";
        }
    }
}
