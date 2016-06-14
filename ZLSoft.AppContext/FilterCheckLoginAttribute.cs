using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace ZLSoft.AppContext
{
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class FilterCheckLoginAttribute : FilterAttribute, IActionFilter
    {
        public FilterCheckLoginAttribute()
        {
            base.Order = 5;
        }
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool flag = false;
            if (filterContext.HttpContext.Session["ID"] != null)
            {
                flag = true;
            }
            HttpRequestBase request = filterContext.HttpContext.Request;
            if (!flag)
            {
                if (request.IsAjaxRequest())
                {
                    string str = "SESSION过期";
                    filterContext.Result = new ContentResult
                    {
                        Content = "{flag:-1,msg:'" + str + "'}"
                    };
                    filterContext.HttpContext.Response.Clear();
                    filterContext.HttpContext.Response.StatusCode = 503;
                }
                else
                {
                    if (this.CheckAgent(filterContext))
                    {
                        //filterContext.Result = new RedirectResult("~/mobile/mlogin/FramLogOn?fromurl=" + HttpUtility.UrlEncode(request.Url.PathAndQuery));
                        string str = "SESSION过期";
                        filterContext.Result = new ContentResult
                        {
                            Content = "{flag:-1,msg:'" + str + "'}"
                        };
                        filterContext.HttpContext.Response.Clear();
                        filterContext.HttpContext.Response.StatusCode = 503;
                    }
                    else
                    {
                        //filterContext.Result = new RedirectResult("~/sys/login/FramLogOn?fromurl=" + HttpUtility.UrlEncode(request.Url.PathAndQuery));
                        string str = "SESSION过期";
                        filterContext.Result = new ContentResult
                        {
                            Content = "{flag:-1,msg:'" + str + "'}"
                        };
                        filterContext.HttpContext.Response.Clear();
                        filterContext.HttpContext.Response.StatusCode = 503;
                    }
                }
            }
        }
        public bool CheckAgent(ActionExecutingContext filterContext)
        {
            bool result = false;
            string userAgent = filterContext.HttpContext.Request.UserAgent;
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
    }
}
