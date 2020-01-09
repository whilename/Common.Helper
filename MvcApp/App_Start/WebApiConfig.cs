using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Filters;

namespace MvcApp
{
    /// <summary></summary>
    public static class WebApiConfig
    {
        /// <summary></summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Filters.Add(new ApiHandleErrorAttribute());
        }
    }

    /// <summary>Handling exceptions raised by action methods</summary>
    public class ApiHandleErrorAttribute : ExceptionFilterAttribute
    {
        /// <summary>Log when an exception occurs.</summary>
        /// <param name="filterContext"></param>
        public override void OnException(HttpActionExecutedContext filterContext)
        {
            base.OnException(filterContext);
            StringBuilder message = new StringBuilder(filterContext.Exception.Message + filterContext.Exception.StackTrace);
            // Handle up to three layers of exception messages
            if (filterContext.Exception.InnerException != null)
            {
                message.Insert(0, filterContext.Exception.InnerException.Message
                    + filterContext.Exception.InnerException.StackTrace);
                if (filterContext.Exception.InnerException.InnerException != null)
                {
                    message.Insert(0, filterContext.Exception.InnerException.InnerException.Message
                        + filterContext.Exception.InnerException.InnerException.StackTrace);
                }
            }
            Log.WriteErrorLog(message.ToString());
        }
    }
}
