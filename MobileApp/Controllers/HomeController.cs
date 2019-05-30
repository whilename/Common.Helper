using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wechat;
using Wechat.Models;

namespace MobileApp.Controllers
{
    /// <summary></summary>
    public class HomeController : Controller
    {
        /// <summary>Authorize redirect url.</summary>
        /// <param name="state">Paramenters</param>
        /// <param name="silent">Is Silent Authorize,0 silent,1 use authorize</param>
        /// <returns>Redirect Authorize Page</returns>
        public RedirectResult Auth(string state = "", int silent = 1)
        {
            string back_url = Request.Url.AbsoluteUri + "/Index";
            return Redirect(CorpCommon.Instance.Oauth2Authorize(back_url, state, silent == 0));
        }

        /// <summary>Index page</summary>
        /// <param name="code">Authorization code</param>
        /// <param name="state">Paramenters</param>
        /// <returns></returns>
        public ActionResult Index(string code, string state = "")
        {
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            CorpMemberTicket ticket = CorpCommon.Instance.GetMemberTicket(code);
            CorpMemberInfo member = CorpCommon.Instance.GetMemberInfo(ticket.user_ticket);
            //var errmsg = CorpCommon.Instance.DeleteMember("ouyang");

            return View(member);
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
