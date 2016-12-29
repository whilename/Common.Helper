using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Common
{
    /// <summary></summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
    public class MvcFilterAttribute : ActionFilterAttribute
    {
        /// <summary>允许的站点</summary>
        public string[] AllowSites { get; set; }

        /// <summary></summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            this.OnAllowOriginExcute(filterContext);
        }

        /// <summary></summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        /// <summary>检查允许跨域请求站点</summary>
        /// <param name="context"></param>
        public void OnAllowOriginExcute(ControllerContext context)
        {
            var origin = context.HttpContext.Request.Headers["Origin"];
            //Action action = () => { context.HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", origin); };
            if (AllowSites != null && AllowSites.Contains(origin))
            {
                context.HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", origin);
            }
        }
    }
}