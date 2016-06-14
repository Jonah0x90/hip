using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.AppContext;
using System.Web.Mvc;
using ZLSoft.Pub;
using ZLSoft.DalManager;
using ZLSoft.Model.SYS;
using ZLSoft.Pub.PageData;

namespace ZLSoft.Sys.Controllers
{
    public class MessageController : BaseController
    {
        public ActionResult Index()
        {
            return null;
        }

        public ActionResult SendMsg()
        {
            StrObjectDict dict = GetParams();

            #region 检查入参有效性
            string eventtype = dict.GetString("EventType");
            string eventname = dict.GetString("EventName");
            string sendmoduleID = dict.GetString("SendModuleID");

            if (string.IsNullOrEmpty(eventtype))
            {
                return this.MyJson(0, "参数错误:EventType");
            }
            if (string.IsNullOrEmpty(eventname))
            {
                return this.MyJson(0, "参数错误:EventName");
            }
            if (string.IsNullOrEmpty(sendmoduleID))
            {
                return this.MyJson(0, "参数错误:SendModuleID");
            }

            #endregion

            //获取用户SESSION            
            dict["SendModuleID"] = LoginSession.Current.USERID;

            string id = MsgManager.Instance.InsertMsg(dict);

            if (id != null)
            {
                IList<StrObjectDict> dicts = MsgManager.Instance.GetMsgByID(new
                {
                    ID = id
                }.toStrObjDict(false));
                //if (dicts[0]["GetModuleID"].ToString().IndexOf(',') != -1)
                //{
                //    State = "IsSendGroup:1";
                //}
                //else
                //{
                //    State = "IsSendGroup:0";
                //}
                return this.MyJson(1,dicts);
            }
            else
            {
                return this.MyJson(0, "消息投递失败!请重试!!!");
            }
        }


        public ActionResult ReplyMsg()
        {
            return null;
        }

        public ActionResult DeleteMsg()
        {
            StrObjectDict dict = GetParams();

            #region 检查入参有效性
            string id = dict.GetString("ID");

            if (string.IsNullOrEmpty(id))
            {
                return this.MyJson(0, "参数错误:ID");
            }

            #endregion

            //删除

            int isOk = MsgManager.Instance.Delete<MessageLog>(id);
            if (isOk > 0)
            {
                return this.MyJson(1, "删除成功");
            }
            else
            {
                return this.MyJson(0, "操作失败");
            }
        }
    }
}
