using System.Web.Mvc;

namespace MvcApp.Areas.Admin
{
    /// <summary></summary>
    public class AdminAreaRegistration : AreaRegistration
    {
        /// <summary></summary>
        public override string AreaName { get { return "Admin"; } }

        /// <summary></summary>
        /// <param name="context"></param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Manage/{controller}/{action}/{id}",
                new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
