using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartPortal.Areas.Admin.Controllers
{
    /// <summary>
    /// 访客分析
    /// 分析网站的访客流量、设置第三方统计代码等。
    /// </summary>
    [Filter.DefaultAuthorizationFilter]
    [Filter.DefaultLoggerActionFilter]
    [Filter.DefaultExceptionFilter]
    public class AnalysisController : BaseController
    {
        //
        // GET: /Analysis/

        public ActionResult Index()
        {
            return View();
        }

    }
}
