using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model.SYS;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;
using ZLSoft.Pub.Model;
using ZLSoft.Pub.PageData;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace ZLSoft.DalManager
{
    /// <summary>
    ///  消息服务Mananger
    /// </summary>
    public class MsgManager:CRUDManager
    {
        private static MsgManager _Instance = new MsgManager();
        public static MsgManager Instance
        {
            get
            {
                return MsgManager._Instance;
            }
        }
        private MsgManager()
        {

        }

        public string InsertMsg(StrObjectDict dict) 
        {
            DBState state = null;
            dict["ID"] = Utils.getGUID();
            string msgID = dict["ID"].ToString();
            state = new DBState
                {
                    Name = "INSERT_MessageLog",
                    Param = new
                    {
                        ID = dict["ID"],
                        EventType = dict["EventType"],
                        ExtendType = dict["ExtendType"],
                        SendModuleID = dict["SendModuleID"],
                        GetModuleID = dict["GetModuleID"],
                        MsgContent = dict["MsgContent"],
                        CreateDate = DateTime.Now,
                        IsNeedToRespond = dict["IsNeedToRespond"],
                        IsRespond = dict["IsRespond"],
                        Extend = dict["Extend"],
                        EventName = dict["EventName"]
                    }.toStrObjDict(),
                    Type = ESqlType.INSERT
                };
            if (DB.Execute(state) > 0)
            {
                //保存成功
                return msgID;    
            }
            else
            { 
                //保存失败
                return null;
            }
        }

        ///<summary> 
        /// 序列化 
        /// </summary> 
        /// <param name="data">要序列化的对象</param> 
        /// <returns>返回存放序列化后的数据缓冲区</returns> 
        public static byte[] Serialize(object data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream rems = new MemoryStream();
            formatter.Serialize(rems, data);
            return rems.GetBuffer();
        }

        public IList<StrObjectDict> GetMsgByID(IDictionary<string, object> obj)
        {
            return DB.ListSod("LIST_MessageLog", obj);
        }

        public string Switch(string str)
        {
            string sender = str;

            //得到SENDID所属模块ID/用户角色组

            //查询匹配接收模块ID/对应的接收用户(需详细业务逻辑)

            //SendMsg()

            return null;
        }

    }
}
