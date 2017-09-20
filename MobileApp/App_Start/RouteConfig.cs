using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MobileApp
{
    /// <summary>Rout url config</summary>
    public class RouteConfig
    {
        /// <summary>Register Routes</summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "auth", action = "Index", id = UrlParameter.Optional }
                //,namespaces: new string[] { "MobileApp.Areas.WeChat.Controllers" }
            //).DataTokens.Add("area", "WeChat");
            );
        }
    }
}