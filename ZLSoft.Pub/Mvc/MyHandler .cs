using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ZLSoft.Pub.Mvc
{
    public class MyHandler : MvcHandler
    {
        public MyHandler(RequestContext requestContext):base(requestContext)
        {

            this.RequestContext = requestContext;
        }

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write(String.Format("this is a Wheel for {0}Controller and {1}action "
                , this.RequestContext.RouteData.Values["Controller"]
                , this.RequestContext.RouteData.Values["Action"]));
            context.Response.End();
        }

        #endregion

        public RequestContext RequestContext { get; private set; }
    }
}
