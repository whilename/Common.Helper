using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartPortal.Areas.Admin.Controllers
{
    /// <summary>
    /// 网站设置
    /// 管理网站域名等信息。
    /// </summary>
    [Filter.DefaultAuthorizationFilter]
    [Filter.DefaultLoggerActionFilter]
    [Filter.DefaultExceptionFilter]
    public class ConfigurationController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
