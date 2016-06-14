using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Pub;
using ZLSoft.Pub.Db;
using System.Web;
using ZLSoft.Model.PUB;

namespace ZLSoft.DalManager
{
    /// <summary>
    /// 登陆相关
    /// </summary>
    public class LoginManager:CRUDManager
    {
        private static LoginManager _Instance = new LoginManager();
        public static LoginManager Instance
        {
            get
            {
                return LoginManager._Instance;
            }
        }
        private LoginManager()
        {
        }

        #region old
        public StrObjectDict DoLogin(string username, string password, out string retmsg)
        {
            string text = MD5Encode.Encode(password);
            string text2 = " 1 = 1 ";
            string csz = ParmManager.Instance.getCsz("XT_MD5ENCODE");
            if (string.IsNullOrEmpty(password))
            {
                text2 += " and (mm is null or mm ='')";
            }
            else
            {
                if (csz == "1")
                {
                    text2 = text2 + " and (mm = '" + text + "' )";
                }
                else
                {
                    string text3 = text2;
                    text2 = string.Concat(new string[]
					{
						text3,
						" and  (mm = '",
						text,
						"' or mm='",
						password,
						"' )"
					});
                }
            }
            //string sql = DB.GetSql("LIST2_XT_YHXX", StrObjectDict.FromVariable(new
            //{
            //    YHGH = username,
            //    WHERE_CLAUSE = text2,
            //    ZFPB = 0
            //}));
            IList<StrObjectDict> list = DB.ListSod("LIST2_XT_YHXX", StrObjectDict.FromVariable(new
            {
                YHGH = username,
                WHERE_CLAUSE = text2,
                ZFPB = 0
            }));
            if (list.Count > 0)
            {
                retmsg = "";
                return list.FirstOrDefault<StrObjectDict>();
            }
            else
            {
                list = DB.ListSod("LIST2_XT_YHXX", StrObjectDict.FromVariable(new
                {
                    YHGH = username,
                    ZFPB = 0
                }));
                if (list.Count > 0)
                {
                    retmsg = "密码错误！";
                }
                else
                {
                    retmsg = "工号错误！";
                }
            }
            return null;
        }






        //public bool DoLoginForPort(string logintype, string username)
        //{
        //    string wHERE_CLAUSE = " 1 = 1 ";
        //    System.Collections.Generic.IList<StrObjectDict> list;
        //    if (logintype == "YHID")
        //    {
        //        list = DB.ListSod("LIST2_XT_YHXX", StrObjectDict.FromVariable(new
        //        {
        //            YHID = username,
        //            WHERE_CLAUSE = wHERE_CLAUSE,
        //            ZFPB = 0
        //        }));
        //    }
        //    else
        //    {
        //        list = DB.ListSod("LIST2_XT_YHXX", StrObjectDict.FromVariable(new
        //        {
        //            YHGH = username,
        //            WHERE_CLAUSE = wHERE_CLAUSE,
        //            ZFPB = 0
        //        }));
        //    }
        //    bool result;
        //    if (list.Count > 0)
        //    {
        //        HttpContext.Current.Session["LOGINED"] = true;
        //        LoginSession.Current.Refresh(list.FirstOrDefault<StrObjectDict>());
        //        HttpCookie httpCookie = new HttpCookie("YONGHUGH", username);
        //        httpCookie.Expires = System.DateTime.Now.AddYears(1);
        //        HttpContext.Current.Response.Cookies.Add(httpCookie);
        //        result = true;
        //    }
        //    else
        //    {
        //        HttpContext.Current.Session["LOGINED"] = null;
        //        result = false;
        //    }
        //    return result;
        //}

        #endregion


        /// <summary>
        /// 验证用户名和密码
        /// </summary>
        /// <param name="userName">工号</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public StrObjectDict GetByUserNameAndPwd(string userName, string pwd)
        {
            string passwordMd5 = MD5Encode.Encode(pwd);

            return this.LoadObjectSod<PubUser>(new
            {
                UserName = userName,
                Password = passwordMd5
            }.toStrObjDict());
        }

        /// <summary>
        /// 获取用户归属角色&默认载入功能
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList<StrObjectDict> GetRoleAndDefault(IDictionary<string, object> obj)
        {
            return DB.ListSod("Get_RoleAndDefaultFunction", obj);
        }
    }
}
