using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZLSoft.DalManager;
using ZLSoft.Model.SYS;
using ZLSoft.Model.PLATFORM;
using ZLSoft.Pub;
using System.Web.Routing;

namespace ZLSoft.AppContext
{
    [FilterException]
    public class BaseController:Controller
    {
        protected override void Execute(RequestContext requestContext)
        {
            base.Execute(requestContext);
        }


        protected override void Initialize(RequestContext requestContext)
        {
            RouteData rd = requestContext.RouteData;
            Route r = rd.Route as Route;

            object o = r.DataTokens["Namespaces"];

            if (o == null)
            {
                base.Initialize(requestContext);
                return;
            }

            string ns = ((string[])o)[0];
            string currentNameSpace = this.GetType().Namespace;

            if (ns == currentNameSpace)
            {
                base.Initialize(requestContext);
                return;
            }
            OnError(requestContext, "非法路径");
        }


        protected void OnError(RequestContext requestContext,string error)
        {
            requestContext.HttpContext.Response.Clear();
            requestContext.HttpContext.Response.StatusCode = 500;
            requestContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            requestContext.HttpContext.Response.Write(new
            {
                Flag = 0,
                Msg = error
            }.ToJson());
            requestContext.HttpContext.Response.Flush();
            requestContext.HttpContext.Response.End();
        }


        //protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        //{
            
        //}

        private StrObjectDict httpData;

        protected bool CheckAgent()
        {
            bool result = false;
            string userAgent = Request.UserAgent;
            string[] array = new string[]
			{
				"Android",
				"iPhone",
				"iPod",
				"iPad",
				"Windows Phone",
				"MQQBrowser"
			};
            if (!userAgent.Contains("Windows NT") || (userAgent.Contains("Windows NT") && userAgent.Contains("compatible; MSIE 9.0;")))
            {
                if (!userAgent.Contains("Windows NT") && !userAgent.Contains("Macintosh"))
                {
                    string[] array2 = array;
                    for (int i = 0; i < array2.Length; i++)
                    {
                        string value = array2[i];
                        if (userAgent.Contains(value))
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }
            return result;
        }


        public StrObjectDict GetHttpData()
        {
            StrObjectDict dict = Request.HttpDataToDict();
            if(!dict.ContainsKey("Params")){
                throw new Exception("非法操作!");
            }
            return dict;
        }

        public StrObjectDict getHttpData(bool nullValueAsKey)
        {
            if (httpData == null)
            {
                httpData = Request.HttpDataToDict(nullValueAsKey);
                return httpData;
            }
            return httpData;
        }

        public StrObjectDict GetParams()
        {
            return getHttpData(false).GetObject("Params").toStrObjDict();
        }

        public StrObjectDict GetData(string str)
        {
            return getHttpData(false).GetObject(str).toStrObjDict();
        }

        public StrObjectDict GetPageInfo(bool nullValueAsKey)
        {
            return getHttpData(nullValueAsKey).GetObject("Page").toStrObjDict();
        }

        public StrObjectDict GetParams(bool nullValueAsKey)
        {
            return getHttpData(nullValueAsKey).GetObject("Params").toStrObjDict();
        }

        public StrObjectDict GetPageInfo()
        {
            return getHttpData(false).GetObject("Page").toStrObjDict();
        }

        #region 日志相关

        private Log InjectLog(string Content, string ModuleName)
        {
            var User = "System";
            try
            {
                if (!string.IsNullOrEmpty(LoginSession.Current.NAME))
                {
                    User = LoginSession.Current.NAME;
                }
            }
            catch
            { 
                
            }
            return new Log
            {
                ModifyUser = User,
                ModuleName = ModuleName,
                Content = Content,
                Function = this.GetType().Name.Replace("Controller", ""),
            };
        }

        /// <summary>
        /// 写入日志:错误
        /// </summary>
        /// <param name="Content">内容</param>
        /// <param name="ModuleName">模块名称</param>
        public void Error(string Content,string ModuleName)
        {
            LogManager.Instance.Error(InjectLog(Content,ModuleName));
        }
        /// <summary>
        /// 写入日志:错误
        /// </summary>
        /// <param name="Content">内容</param>
        public void Error(string Content)
        {
            this.Error(Content, null);
        }

        /// <summary>
        /// 写入日志:警告
        /// </summary>
        /// <param name="Content">内容</param>
        /// <param name="ModuleName">模块名称</param>
        public void Warning(string Content, string ModuleName)
        {
            LogManager.Instance.Warning(InjectLog(Content, ModuleName));
        }
        /// <summary>
        /// 写入日志:警告
        /// </summary>
        /// <param name="Content">内容</param>
        public void Warning(string Content)
        {
            this.Warning(Content, null);
        }
        /// <summary>
        /// 写入日志:信息
        /// </summary>
        /// <param name="Content">内容</param>
        /// <param name="ModuleName">模块名称</param>
        public void Info(string Content, string ModuleName)
        {
            LogManager.Instance.Info(InjectLog(Content,ModuleName));
        }
        /// <summary>
        /// 写入日志:信息
        /// </summary>
        /// <param name="Content">内容</param>
        public void Info(string Content)
        {
            this.Info(Content, null);
        }
        /// <summary>
        /// 写入日志:成功
        /// </summary>
        /// <param name="Content">内容</param>
        /// <param name="ModuleName">模块名称</param>
        public void Success(string Content, string ModuleName)
        {
            LogManager.Instance.Success(InjectLog(Content, ModuleName));
        }
        /// <summary>
        /// 写入日志:成功
        /// </summary>
        /// <param name="Content">内容</param>
        public void Success(string Content)
        {
            this.Success(Content, null);
        }
        #endregion

    }
}
