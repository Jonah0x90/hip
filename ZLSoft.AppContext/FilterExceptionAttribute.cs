using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using IBatisNet.DataMapper.Customize;
using ZLSoft.Pub;

namespace ZLSoft.AppContext
{
    public class FilterExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new System.ArgumentNullException("filterContext");
            }
            if (!string.IsNullOrEmpty(filterContext.Exception.ToString()))
            {
                ((BaseController)filterContext.Controller).Error(filterContext.Exception.Message);

            }
            if (!filterContext.ExceptionHandled)
            {
                ControllerContext controllerContext = filterContext.Controller.ControllerContext;
                HttpRequestBase request = controllerContext.HttpContext.Request;
               // if (request.IsAjaxRequest())
                //{
                    string data;
                    if (filterContext.Exception is ExecuteSqlException)
                    {
                        ExecuteSqlException ex = filterContext.Exception as ExecuteSqlException;
                        data = string.Concat(new string[]
						{
							//ex.InnerException.Message,
							//"|",
							ex.SqlStatement,
							"|",
							ex.Message
						});
                    }
                    else
                    {
                        //data = filterContext.Exception.Message ;
                       data = filterContext.Exception.Message + "|" + filterContext.Exception.StackTrace.ToString();
                    }
                    filterContext.ExceptionHandled = true;
                    filterContext.HttpContext.Response.Clear();
                    filterContext.HttpContext.Response.StatusCode = 200;
                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                    filterContext.HttpContext.Response.Write(new
                    {
                        Flag = 0,
                        Msg = data
                    }.ToJson());
                    filterContext.HttpContext.Response.Flush();
                    filterContext.HttpContext.Response.End();
               // }
                //else
                //{
                //    base.OnException(filterContext);
               // }
            }
        }
    }
}
