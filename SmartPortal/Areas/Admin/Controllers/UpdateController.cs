using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartPortal.Areas.Admin.Controllers
{
    /// <summary>
    /// 更新维护
    /// 提供建库、升级建库、补丁脚本、安全程序补丁更新维护功能。
    /// </summary>
    [Filter.DefaultAuthorizationFilter]
    [Filter.DefaultLoggerActionFilter]
    [Filter.DefaultExceptionFilter]
    public class UpdateController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
