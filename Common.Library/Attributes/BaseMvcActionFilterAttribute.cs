using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Common.Library.Attributes
{
    /// <summary></summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
    public class BaseMvcFilterAttribute : ActionFilterAttribute
    {
        /// <summary>允许的站点</summary>
        public string[] AllowSites { get; set; }

        /// <summary>在执行Action方法前处理</summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            this.OnAllowOriginExcute(filterContext);
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
