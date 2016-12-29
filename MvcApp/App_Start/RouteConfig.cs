using MvcApp.Areas.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcApp
{
    /// <summary></summary>
    public class RouteConfig
    {
        /// <summary></summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Admin", action = "Index", id = UrlParameter.Optional },
                //namespaces: new string[] { typeof(AdminAreaRegistration).Namespace + ".Controllers" }
                namespaces: new string[] { "MvcApp.Areas.Admin.Controllers" }
            ).DataTokens.Add("area","Admin");
        }
    }
}