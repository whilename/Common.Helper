using MvcApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Areas.Admin.Controllers
{
    /// <summary></summary>
    public class AdminController : Controller
    {
        /// <summary></summary>
        /// <returns></returns>
        [MvcFilter]
        public ActionResult Index()
        {
            return View();
        }

    }
}
