using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace ZLSoft.Pub.Mvc
{
    public class MyRoutingModule : IHttpModule
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Init(HttpApplication context)
        {
            context.PostResolveRequestCache += new EventHandler(context_PostResolveRequestCache);
        }

        void context_PostResolveRequestCache(object sender, EventArgs e)
        {
            HttpContextBase context = new HttpContextWrapper(((HttpApplication)sender).Context);
            this.PostResolveRequestCache(context);

        }

        private void PostResolveRequestCache(HttpContextBase context)
        {
            RouteData routeData = RouteTable.Routes.GetRouteData(context);

            if (routeData == null)
            {
                throw new InvalidOperationException();
            }

            IRouteHandler routeHandler = routeData.RouteHandler;
            if (routeHandler == null)
            {
                throw new InvalidOperationException();
            }

            RequestContext requestContext = new RequestContext(context, routeData);
            IHttpHandler httpHandler = routeHandler.GetHttpHandler(requestContext);
            if (httpHandler == null)
            {
                throw new InvalidOperationException("无法创建对应的HttpHandler对象");
            }
            context.RemapHandler(httpHandler);

        }
    }
}
