using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileApp.Controllers
{
    /// <summary></summary>
    public class HomeController : Controller
    {
        /// <summary></summary>
        public ActionResult Index()
        {
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        /// <summary></summary>
        public ActionResult About()
        {
            //ViewBag.Message = "Your app description page.";

            return View();
        }

        /// <summary></summary>
        public ActionResult Contact()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
