﻿using Common.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcApp
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    /// <summary></summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary></summary>
        protected void Application_Start()
        {
            Log.Register();
            //Log.Info("Test Message");
            // 移除所有视图引擎
            ViewEngines.Engines.Clear();
            // 添加Razor视图引擎
            ViewEngines.Engines.Add(new RazorViewEngine()); 
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}