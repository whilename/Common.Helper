using MobileApp.Bll;
using MobileApp.Models;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wechat;
using Wechat.Models;

namespace MobileApp.Areas.WeChat.Controllers
{
    /// <summary></summary>
    public class AuthController : Controller
    {
        /// <summary></summary>
        public RedirectResult Index()
        {
            string state = Request.QueryString["state"];
            string login_url = Request.Url.AbsoluteUri + "/Login";
            return Redirect(WXCommon.Instance.Oauth2Authorize(login_url, state, false)); ;
        }

        /// <summary></summary>
        public RedirectToRouteResult Login()
        {
            string code = Request.QueryString["code"];
            string state = Request.QueryString["state"];
            // 获取微信授权信息
            WXAccessToken access_token = WXCommon.Instance.GetAccessToken(code);
            if (access_token.scope.Equals("snsapi_userinfo", StringComparison.CurrentCultureIgnoreCase))
            {
                WXUserInfo wxuser = WXCommon.Instance.GetUserInfo(access_token);
                UsersEntity user = new UsersEntity();
                MembersEntity member = new MembersEntity();
                UsersBusiness ubll = new UsersBusiness();
                MembersBusiness mbll = new MembersBusiness();
                try
                {
                    bool add = ubll.Save(user);
                    if (add)
                    {
                        user.UserName = "wx" + user.UserId.ToString().PadLeft(6, '0');
                        ubll.Save(user);
                    }
                    member.UserId = user.UserId;
                    member.OpenId = wxuser.openid;
                    member.NickName = wxuser.nickname;
                    member.HeadImgUrl = wxuser.headimgurl;
                    member.Sex = wxuser.sex;
                    member.Country = wxuser.country;
                    member.Province = wxuser.province;
                    member.City = wxuser.city;
                    add = mbll.Save(member);
                }
                catch (Exception ex) { Log.Error(ex); }
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
