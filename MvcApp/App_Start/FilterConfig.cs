using Common.Utility;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    /// <summary></summary>
    public class FilterConfig
    {
        /// <summary></summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomErrorProcessAttribute());
        }
    }

    /// <summary>处理由操作方法引发的异常</summary>
    public class CustomErrorProcessAttribute : HandleErrorAttribute
    {
        /// <summary>在发生异常时调用。</summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            Log.Error(filterContext.Exception);
        }
    }
}