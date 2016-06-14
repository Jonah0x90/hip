using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.Pub.Mvc;
using ZLSoft.Pub;
using ZLSoft.DalManager;
using ZLSoft.Pub.Db;
using ZLSoft.AppContext;
using ZLSoft.AppContext.Context;
using System.Web;
using ZLSoft.Model.SYS;
using ZLSoft.Model.PUB;

namespace ZLSoft.Sys.Controllers
{

    /// <summary>
    /// 登陆相关功能
    /// </summary>
    public class LoginController : BaseController
    {

        /// <summary>
        /// 登录状态检查
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginOn()
        {
            //if (Request.Cookies[CookieContext.AppID] == null && Request.Cookies[CookieContext.EmployeeID] == null)\
            bool result = LoginSession.Current.IsLogon();
            if (result)
            {
                string UserName = LoginSession.Current.USERNAME;

                //string Dept = LoginSession.Current.DEPTID;

                //string IP = LoginSession.Current.IP;

                //string ksdm = LoginSession.Current.KSDM;

                //string ksmc = LoginSession.Current.KSMC;

                //string srm = LoginSession.Current.SRM;

                //string zgid = LoginSession.Current.ZGID;

                //IList<string> list = new List<string>();
                //list.Add(UserName);
                //list.Add(Dept);
                //list.Add(IP);
                //list.Add(ksdm);
                //list.Add(ksmc);
                //list.Add(srm);
                //list.Add(zgid);

                //return this.MyJson(1, list);
                return this.MyJson(1, "当前用户: [" + UserName + "] 已登录。");
            }
            else
            {
                return this.MyJson(0, "未登录");
            }
        }


        /// <summary>
        /// 平台登录
        /// </summary>
        /// <returns></returns>
        //public ActionResult UserLogin()
        //{
        //    StrObjectDict dict = GetParams();
        //    string username = dict.GetString("UserName");
        //    string password = dict.GetString("PassWord");
        //    if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
        //    {
        //        return this.MyJson(0, "用户名或密码不能为空");
        //    }

        //    if (Utils.GetString(base.HttpContext.Application["REGCODE"]) == "false")
        //    {
        //        return this.MyJson(0, "系统未注册,请先注册!");
        //    }
        //    else
        //    {
        //        StrObjectDict u = LoginManager.Instance.GetByUserNameAndPwd(username, password);
        //        if (u == null)
        //        {
        //            return this.MyJson(0, "用户名或者密码错误");

        //        }

        //        if (u.ContainsKey("IsInvalid") && u["IsInvalid"].ToString() == "1")
        //        {
        //            return this.MyJson(0, "你的帐号不可用，请联系管理员");
        //        }

        //        //登录成功，放入会话中保存
        //        HttpContext.Session["LOGINED"] = true;
        //        LoginSession.Current.Refresh(u);
        //        HttpCookie httpCookie = new HttpCookie(CookieContext.EmployeeID, username);
        //        httpCookie.Expires = System.DateTime.Now.AddYears(1);
        //        HttpContext.Response.Cookies.Add(httpCookie);
        //        return this.MyJson(1, u);
        //    }
        //}


        /// <summary>
        /// 登陆(所有终端平台通用)
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            StrObjectDict dict = GetParams();
            string deviceID = dict.GetString("DeviceID");
            string userName = dict.GetString("UserName");
            string pwd = dict.GetString("Password");
            string moduleCode = dict.GetString("ModuleCode");

            #region 检查参数有效性

            if (moduleCode != "WebBrowser" && string.IsNullOrEmpty(deviceID))
            {
                return this.MyJson(0, "参数为空:DeviceID");
            }

            if (string.IsNullOrEmpty(userName))
            {
                return this.MyJson(0, "参数为空:UserName");
            }

            if (string.IsNullOrEmpty(pwd))
            {
                return this.MyJson(0, "参数为空:Password");
            }

            #endregion



            //检查设备是否可用

            if (moduleCode != "WebBrowser")
            {
                SiteDevice device = SiteManager.Instance.GetSiteDeviceByDeviceID(deviceID);
                if (device == null || device.Enable == 0)
                {
                    return this.MyJson(-999, "该设备未注册或者已禁用，请联系系统管理人员");
                }
            }


            //检查帐号合法性
            StrObjectDict u = LoginManager.Instance.GetByUserNameAndPwd(userName, pwd);

            if (u == null)
            {
                return this.MyJson(-999, "用户名或者密码错误");

            }

            if (u.ContainsKey("IsInvalid") && u["IsInvalid"].ToString() == "1")
            {
                return this.MyJson(-999, "你的帐号不可用，请联系管理员");
            }

            //登陆成功，放入会话中保存
            LoginSession.Current.Refresh(u);

            IList<StrObjectDict> userinfo = LoginManager.Instance.GetRoleAndDefault(u);

            if (userinfo.Count > 0)
            {
                IList<StrObjectDict> list = new List<StrObjectDict>();
                foreach (var item in userinfo)
                {
                    list.Add(item);
                }
                u["RoleData"] = list; 
                //u["Role"] = userinfo[0]["Role"];
                //u["DefaultFunction"] = userinfo[0]["DefaultFunction"];
                //u["URL"] = userinfo[0]["URL"];
                //u["DefaultName"] = userinfo[0]["DefaultName"];  
            }

            return this.MyJson(1, u);
        }

        public ActionResult LoginOut()
        {
            //if (Request.Cookies[CookieContext.AppID] == null && Request.Cookies[CookieContext.EmployeeID] == null)
            //if()
            //{
            //    return this.MyJson(0, "尚未登录。");
            //}
            //else
            //{
            LoginSession.Current.LogOut(); //标记登出
            LoginSession.Current.Clear();  //Clear Session
            //HttpCookie cookie = Request.Cookies[CookieContext.EmployeeID];
            //if (cookie != null)
            //{
            //    cookie.Expires = DateTime.Now.AddDays(-1000);
            //    HttpContext.Response.AppendCookie(cookie);
            //}
            return this.MyJson(1, "已安全退出。");
            //}
        }

        public ActionResult SelectUser()
        {
            StrObjectDict dict = GetParams();
            string userName = dict.GetString("UserName");

            if (string.IsNullOrEmpty(userName))
            {
                return this.MyJson(0, "UserName参数错误!");
            }

            IList<StrObjectDict> list = UserManager.Instance.GetUser(new
            {
                UserName = userName
            }.toStrObjDict()
            );

            return this.MyJson(1, list);
        }
    }
}
