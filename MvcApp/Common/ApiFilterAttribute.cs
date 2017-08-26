using Common.Library.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace MvcApp.Common
{
    /// <summary></summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
    public class ApiFilterAttribute : BaseApiActionFilterAttribute
    {
        /// <summary></summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {
            this.OnAllowOriginExcute(filterContext);
            base.OnActionExecuted(filterContext);
        }
        
    }
}