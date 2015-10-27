﻿using System.Web.Mvc;
using System.Web.Routing;

namespace IOC.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*thumb}", new { thumb = @".+\.thumb\.axd" });
            routes.IgnoreRoute("{*transform}", new { transform = @".+\.transform\.axd" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}