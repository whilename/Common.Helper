using SmartPortal.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace SmartPortal.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { Controller="System",action = "Index", id = UrlParameter.Optional }
            );
        }
    }

    /// <summary></summary>
    public class BaseController : Controller
    {
        protected Models.SmartDB smartDB = new Models.SmartDB();

        public BaseController()
        {

        }
    }
}