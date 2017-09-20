using System.Web.Mvc;

namespace MobileApp.Areas.WeChat
{
    /// <summary></summary>
    public class WeChatAreaRegistration : AreaRegistration
    {
        /// <summary></summary>
        public override string AreaName { get { return "WeChat"; } }

        /// <summary></summary>
        /// <param name="context"></param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "WeChat_default",
                "WeChat/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
