using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub.Model;

namespace ZLSoft.Model.SYS
{
    /// <summary>
    /// 系统_消息记录
    /// </summary>
    public class MessageLog:BuzzModel
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public int EventType { get; set; }

        /// <summary>
        /// 扩展类别
        /// </summary>
        public int ExtendType { get; set; }

        /// <summary>
        /// 发送模块ID
        /// </summary>
        public string SendModuleID { get; set; }

        /// <summary>
        /// 接收模块ID
        /// </summary>
        public string GetModuleID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 是否需要应答
        /// </summary>
        public int IsNeedToRespond { get; set; }

        /// <summary>
        /// 是否应答
        /// </summary>
        public int IsRespond { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string MsgContent { get; set; }

        /// <summary>
        /// 扩展
        /// </summary>
        public string Extend { get; set; }

        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName { get; set; }


        public override string GetModelName()
        {
            return "MessageLog";
        }

        public override string GetTableName()
        {
            return "系统_消息记录";
        }
    }
}
