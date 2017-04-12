using Common.Utility;
using Common.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WeChat.Models;
using System.Threading;

namespace WeChat
{
    /// <summary>微信公共处理类</summary>
    public class WXCommon
    {
        /// <summary>缓存Token</summary>
        public static WXAccessToken AccessToken { get; private set; }
        /// <summary>缓存Ticket</summary>
        public static WXTicket JsApiTicket { get; private set; }

        /// <summary>缓存微信鉴权信息</summary>
        public static void Start()
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                while (true)
                {
                    try
                    {
                        WXCommon.AccessToken = WXCommon.GetAccessToken();// 缓存AccessToken信息
                        WXCommon.JsApiTicket = WXCommon.GetJSApiTicket();// 缓存Ticket信息
                    }
                    catch (Exception ex) { Log.Error(ex); }
                    finally { Thread.Sleep(7000000); }// 为避免CPU空转，在队列为空时休息2秒
                }
            });
        }

        #region Oauth2 Authorize

        /// <summary>微信oauth2授权请求地址</summary>
        /// <param name="callbackurl">回调地址</param>
        /// <param name="state">传递参数</param>
        /// <param name="silent">是否静默授权，true 静默授权，false 用户确认授权，默认true</param>
        /// <returns></returns>
        public static string Oauth2Authorize(string callbackurl, string state, bool silent = true)
        {
            // 微信授权请求地址
            string oauth2_url = string.Format("{0}?appid={1}&redirect_uri={2}&response_type=code&scope={3}&state={4}#wechat_redirect",
                WXConfig.OAUTH2_AUTHORIZE_URL, WXConfig.APPID, Utils.GetUrlEncode(callbackurl),
                (silent ? "snsapi_base" : "snsapi_userinfo"), Utils.GetUrlEncode(state));
            // snsapi_base （不弹出授权页面，直接跳转，只能获取用户openid）
            // snsapi_userinfo （弹出授权页面，可通过openid拿到昵称、性别、所在地。并且，即使在未关注的情况下，只要用户授权，也能获取其信息）
            Log.Info("Redirect WeChat Oauth2:{0}", oauth2_url);
            return oauth2_url;
        }

        /// <summary>获取微信用户信息</summary>
        /// <param name="acctoken">网页授权接口调用凭证,注意：oauth2授权时此access_token与基础支持的access_token不同</param>
        /// <param name="oauth2">是否微信oauth2授权</param>
        /// <returns></returns>
        public static WXUserInfo GetUserInfo(WXAccessToken acctoken, bool oauth2 = true)
        {
            // 获取微信用户信息请求地址
            string userinfo_url = string.Format("{0}?access_token={1}&openid={2}&lang=zh_CN",
                oauth2 ? WXConfig.OAUTH2_USERINFO_URL : WXConfig.CGIBIN_USERINFO_URL, acctoken.access_token, acctoken.openid);
            // 获取微信用户信息
            WXUserInfo user_info = WebHttp.SendGet<WXUserInfo>(userinfo_url.ToString());
            Log.Info("Get WeChat Oauth2 UserInfo：{0} \n Response：{1}", userinfo_url, Utils.JsonSerialize(user_info));
            return user_info;
        }

        #endregion

        #region Access Token

        /// <summary>获取oauth2授权access_token</summary>
        /// <param name="code">授权代码</param>
        /// <returns></returns>
        public static WXAccessToken GetAccessToken(string code)
        {
            // 微信oauth2授权请求地址及参数
            string access_token_url = string.Format("{0}?appid={1}&secret={2}&code={3}&grant_type=authorization_code",
                WXConfig.OAUTH2_ACCESS_TOKEN_URL, WXConfig.APPID, WXConfig.SECRET, code);
            // 微信获取oauth2授权access_token
            WXAccessToken access_token = WebHttp.SendGet<WXAccessToken>(access_token_url);
            Log.Info("Get WeChat Oauth2 AccessToken：{0} \n Response：{1}", access_token_url, Utils.JsonSerialize(access_token));
            return access_token;
        }

        /// <summary>请求微信获取access_token</summary>
        /// <returns></returns>
        private static WXAccessToken GetAccessToken()
        {
            // 微信授权信息请求地址及参数
            string access_token_url = "{0}&appid={1}&secret={2}".StrFormat(WXConfig.ACCESS_TOKEN_URL, WXConfig.APPID, WXConfig.SECRET);
            // GET请求微信授权，回传AccessToken信息
            WXAccessToken access_token = WebHttp.SendGet<WXAccessToken>(access_token_url.ToString());
            // 记录请求与回传信息
            Log.Info("Get WeChat AccessToken：{0} \n Response：{1}", access_token_url, Utils.JsonSerialize(access_token));
            return access_token;
        }

        #endregion

        #region JS-SDK API

        /// <summary>请求微信获取ticket</summary>
        /// <returns></returns>
        private static WXTicket GetJSApiTicket()
        {
            string getticket_url = "{0}?access_token={1}&type=jsapi".StrFormat(WXConfig.GETTICKET_URL, WXCommon.AccessToken.access_token);
            // GET请求微信接口，回传JS-SDK权限信息
            WXTicket ticket = WebHttp.SendGet<WXTicket>(getticket_url.ToString());
            // 记录请求与回传信息
            Log.Info("Get WeChat Ticket：{0} \n Response：{1}", getticket_url.ToString(), Utils.JsonSerialize(ticket));
            return ticket;
        }

        /// <summary>获取微信js-sdk Web授权Config配置</summary>
        /// <param name="url">使用微信授权的页面地址</param>
        /// <returns></returns>
        public static WXJSConfig GetJsConfig(string url)
        {
            // 组织授权请求加密信息
            WXJSSignature signture = new WXJSSignature();
            signture.noncestr = Guid.NewGuid().ToString();// 随机码
            signture.timestamp = DateTime.Now.ToTimeStamp().ToString();// 时间戳
            signture.url = url; // 当前授权页面地址
            signture.jsapi_ticket = WXCommon.JsApiTicket.ticket;

            WXJSConfig config = new WXJSConfig();
            config.appid = WXConfig.APPID;// 微信Id
            config.noncestr = signture.noncestr;// 随机码
            config.timestamp = signture.timestamp; // 时间戳
            config.signature = Utils.SHA1Hash(Utils.ObjectJoinString(signture, true, true, "{0}={1}&").Trim('&'));
            return config;
        }

        #endregion

        #region Message

        /// <summary>微信模板消息</summary>
        /// <param name="template">消息对象信息</param>
        public static object SendMsg(WXMsgTemplate template)
        {
            string send_msg_url = string.Format("{0}?access_token={1}",
                WXConfig.SEND_MSG_TEMPLATE_URL, AccessToken.access_token);
            string param = Utils.JsonSerialize(template);
            // 发送模板消息
            object result = WebHttp.SendPost<object>(send_msg_url, param);
            Log.Info("Send WeChat Template Msg:{0} \n Params:{1} \n Response：{2}",
                send_msg_url, param, Utils.JsonSerialize(result));
            return result;
        }

        #endregion

    }
}