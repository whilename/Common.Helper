using eQuartz.Services;
using eQuartz.Models;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace eQuartz
{
    /// <summary></summary>
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomErrorProcessAttribute());
        }
    }

    /// <summary>处理由操作方法引发的异常</summary>
    public class CustomErrorProcessAttribute : HandleErrorAttribute
    {
        private ORMContext dbcon;
        public CustomErrorProcessAttribute() { dbcon = new ORMContext(); }
        /// <summary>在发生异常时调用。</summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            //Log.Error(filterContext.Exception);
            dbcon.Log.Add(new LogEntity { Content = filterContext.Exception.Message + "\n\t" + filterContext.Exception.StackTrace });
            dbcon.SaveChanges();
        }
    }

    /// <summary></summary>
    //public class BundleConfig
    //{
    //    // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
    //    public static void RegisterBundles(BundleCollection bundles)
    //    {
    //        bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Content/lib/layui/layui.js", "~/Content/okadmin.js"));

    //        bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/css/site.css"));
    //    }
    //}

}
