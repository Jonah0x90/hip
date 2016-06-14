using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub;
using System.Web;
using ZLSoft.Pub.Model;
using ZLSoft.Model.PUB;

namespace ZLSoft.AppContext
{
    public class LoginSession
    {
        private static LoginSession _Instance;
        public string USERID
        {
            get
            {
                return Utils.GetString(HttpContext.Current.Session["ID"]);
            }
        }
        public string USERNAME
        {
            get
            {
                return Utils.GetString(HttpContext.Current.Session["UserName"]);
            }
        }
        public string NAME
        {
            get
            {
                return Utils.GetString(HttpContext.Current.Session["Name"]);
            }
        }
        public string DEPTID
        {
            get
            {
                return Utils.GetString(HttpContext.Current.Session["OrgID"]);
            }
        }
        public string KSDM
        {
            get
            {
                return Utils.GetString(HttpContext.Current.Session["KSDM"]);
            }
        }
        public string KSMC
        {
            get
            {
                return Utils.GetString(HttpContext.Current.Session["KSMC"]);
            }
        }
        public string SEX
        {
            get
            {
                return Utils.GetString(HttpContext.Current.Session["Sex"]);
            }
        }
        public string ZGID
        {
            get
            {
                return Utils.GetString(HttpContext.Current.Session["EmID"]);
            }
        }
        public string SRM
        {
            get
            {
                string @string = Utils.GetString(HttpContext.Current.Session["MRSRM"]);
                return string.IsNullOrEmpty(@string) ? "SRM1" : @string;
            }
        }
        public string IP
        {
            get
            {
                return Utils.GetString(HttpContext.Current.Session["IP"]);
            }
        }
        public string SESSION_ID
        {
            get
            {
                return Utils.GetString(HttpContext.Current.Session["SESSION_ID"]);
            }
        }
        public string Role
        {
            get
            {
                return Utils.GetString(HttpContext.Current.Session["Role"]);
            }
        }
        public string DefalutFunction
        {
            get
            {
                return Utils.GetString(HttpContext.Current.Session["DefalutFunction"]);
            }
        }
        public string URL
        {
            get
            {
                return Utils.GetString(HttpContext.Current.Session["URL"]);
            }
        }
        public static LoginSession Current
        {
            get
            {
                return LoginSession._Instance;
            }
        }
        private LoginSession()
        {
        }
        public static void Init()
        {
            if (LoginSession._Instance == null)
            {
                LoginSession._Instance = new LoginSession();
            }
        }
        public string Get(string key)
        {
            return Utils.GetString(HttpContext.Current.Session[key]);
        }
        public void Clear()
        {
            HttpContext.Current.Session.Clear();
        }
        public void Clear(string key)
        {
            HttpContext.Current.Session[key] = null;
            HttpContext.Current.Session.Remove(key);
        }
        public void Abandon()
        {
            HttpContext.Current.Session.Abandon();
        }
        public void Refresh(IDictionary<string, object> dictionary)
        {
            this.Refresh(dictionary, false);
        }
        public void Refresh(IDictionary<string, object> dicionary, bool nullAsKey)
        {
            foreach (string current in dicionary.Keys)
            {
                bool flag = nullAsKey || dicionary[current] != null;
                if (flag)
                {
                    HttpContext.Current.Session[current] = dicionary[current];
                }
            }
        }

        public bool IsLogon()
        {
            if(string.IsNullOrEmpty(USERNAME)){
                return false;
            }
            return true;
        }

        public StrObjectDict GetUser()
        {
            return new PubUser
            {
                ID = USERID,
                UserName = USERNAME
            }.toStrObjDict();
        }

        public void LogOut()
        {
            HttpContext.Current.Session["LOGINED"] = null;
            LoginSession.Current.Clear();
        }

        /// <summary>
        /// 为model注入基本信息
        /// </summary>
        /// <param name="bm"></param>
        /// <returns></returns>
        public BuzzModel InitUserModel(BuzzModel bm)
        {
            bm.IsInvalid = 0;
            bm.ModifyTime = DateTime.Now;
            bm.ModifyUser = this.USERID;
            return bm;
        }

        /// <summary>
        /// 为model注入基本信息
        /// </summary>
        /// <param name=""></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        public Dictionary<string, object> InitUserModel(Dictionary<string, object> dict)
        {
            dict["IsInvalid"] = 0;
            dict["ModifyTime"] = DateTime.Now;
            dict["ModifyUser"] = this.USERID;
            return dict;
        }

    }
}
