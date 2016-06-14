using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace ZLSoft.Pub.Mvc
{
    public static class RouteCollectionExtension
    {
        public static Route MapWheelRoute(this RouteCollection routes, string name, string url, object defaults)
        {
            return MapWheelRoute(routes, name, url, defaults, null, null);
        }

        public static Route MapWheelRoute(this RouteCollection routes, string name, string url, object defaults, string[] namespaces)
        {
            return MapWheelRoute(routes, name, url, defaults, null, namespaces);
        }

        public static Route MapWheelRoute(this RouteCollection routes, string name, string url, object defaults, object constraints, string[] namespaces)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }
            // 在这里注册 Route 与 WheelRouteHandler的映射关系
            Route route = new Route(url, new MyRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary()
            };

            if ((namespaces != null) && (namespaces.Length > 0))
            {
                route.DataTokens["Namespaces"] = namespaces;
            }

            routes.Add(name, route);

            return route;
        }


    }
}
