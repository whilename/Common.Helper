using Common.Caches;
using Common.Extension;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WeChat.Models;

namespace WeChat
{
    /// <summary>微信公众号帮助类</summary>
    public class BaseWechat
    {
        // 是否首次缓存
        protected static bool IsFirstTime = true;
        // 微信授权
        protected const string OAUTH2_AUTHORIZE_URL = "https://open.weixin.qq.com/connect/oauth2/authorize";
        // 微信API_URL
        protected const string API_URL = "https://api.weixin.qq.com";
        // 企业API_URL
        protected const string CORP_URL = "https://qyapi.weixin.qq.com";
        // 微信支付预订单
        protected const string PAY_UNIFIEDORDER_URL = "https://api.mch.weixin.qq.com/pay/unifiedorder";


        /// <summary>微信的公众号APPID/企业ID</summary>
        protected string AppIdCorpId { get; set; }
        /// <summary>支付商户号（必须配置）</summary>
        protected string MCHID { get; set; }
        /// <summary>商户支付密钥，参考开户邮件设置（必须配置）</summary>
        protected string KEY { get; set; }
        /// <summary>微信的公众号密钥/企业密钥</summary>
        protected string CorpSecret { get; set; }
        /// <summary>企业应用的id</summary>
        protected string AgentId { get; set; }
        /// <summary>缓存Token</summary>
        public WXAccessToken AccessToken { get; private set; }
        /// <summary>缓存Ticket</summary>
        public WXTicket JsApiTicket { get; private set; }

        /// <summary>缓存微信鉴权信息</summary>
        protected void Start()
        {
            AccessToken = GetAccessToken();// 缓存AccessToken信息
            JsApiTicket = GetJSApiTicket();// 缓存JsApiTicket信息
            ThreadPool.QueueUserWorkItem(o =>
            {
                while (true)
                {
                    try
                    {
                        if (IsFirstTime) { IsFirstTime = false; }
                        else
                        {
                            AccessToken = GetAccessToken();// 缓存AccessToken信息
                            JsApiTicket = GetJSApiTicket();// 缓存JsApiTicket信息
                        }
                    }
                    catch (Exception ex) { Log.Error(ex); }
                    finally { Thread.Sleep(7100000); }// 为避免CPU空转，在队列为空时休息
                }
            });
        }

        /// <summary>初始化</summary>
        /// <param name="appid"></param>
        /// <param name="mchid"></param>
        /// <param name="key"></param>
        /// <param name="secret"></param>
        /// <param name="agentid"></param>
        protected void Initialize(string appid, string mchid, string key, string secret, string agentid)
        {
            AppIdCorpId = appid;
            MCHID = mchid;
            KEY = key;
            CorpSecret = secret;
            AgentId = agentid;
        }

        #region Access Token

        /// <summary>请求微信获取access_token</summary>
        /// <returns></returns>
        private WXAccessToken GetAccessToken()
        {
            bool iscorp = string.IsNullOrEmpty(AgentId);
            // 微信授权信息请求地址及参数
            string access_token_url = iscorp ?
                "{0}/cgi-bin/token?grant_type=client_credential&appid={1}&secret={2}" :
                "{0}/cgi-bin/gettoken?corpid={1}&corpsecret={2}";
            // GET请求微信授权，回传AccessToken信息
            WXAccessToken access_token = WebHttp.SendGet<WXAccessToken>(
                access_token_url.StrFormat(iscorp ? API_URL : CORP_URL, AppIdCorpId, CorpSecret));
            // 记录请求与回传信息
            Log.Info("Get WeChat AccessToken：{0} \n Response：{1}", access_token_url, Utils.JsonSerialize(access_token));
            return access_token;
        }

        /// <summary>获取oauth2授权access_token</summary>
        /// <param name="code">授权代码</param>
        /// <returns></returns>
        public WXAccessToken GetAccessToken(string code)
        {
            // 微信oauth2授权请求地址及参数
            string access_token_url = "{0}/sns/oauth2/access_token?appid={1}&secret={2}&code={3}&grant_type=authorization_code".StrFormat(
                    API_URL, AppIdCorpId, CorpSecret, code);
            // 微信获取oauth2授权access_token
            WXAccessToken access_token = WebHttp.SendGet<WXAccessToken>(access_token_url);
            Log.Info("Get WeChat Oauth2 AccessToken：{0} \n Response：{1}", access_token_url, Utils.JsonSerialize(access_token));
            return access_token;
        }

        #endregion

        #region JS-SDK API

        /// <summary>请求微信获取ticket</summary>
        /// <returns></returns>
        private WXTicket GetJSApiTicket()
        {
            string getticket_url = string.IsNullOrEmpty(AgentId) ?
                "{0}/cgi-bin/ticket/getticket?access_token={1}&type=jsapi".StrFormat(API_URL, AccessToken.access_token) :
                "{0}/cgi-bin/get_jsapi_ticket?access_token={1}".StrFormat(CORP_URL, AccessToken.access_token);
            // GET请求微信接口，回传JS-SDK权限信息
            WXTicket ticket = WebHttp.SendGet<WXTicket>(getticket_url.ToString());
            // 记录请求与回传信息
            Log.Info("Get WeChat Ticket：{0} \n Response：{1}", getticket_url.ToString(), Utils.JsonSerialize(ticket));
            return ticket;
        }

        /// <summary>获取微信js-sdk Web授权Config配置</summary>
        /// <param name="url">使用微信授权的页面地址</param>
        /// <returns></returns>
        public WXJSConfig GetJsConfig(string url)
        {
            // 组织授权请求加密信息
            WXJSSignature signture = new WXJSSignature();
            signture.noncestr = Guid.NewGuid().ToString();// 随机码
            signture.timestamp = DateTime.Now.ToTimeStamp().ToString();// 时间戳
            signture.url = url; // 当前授权页面地址
            signture.jsapi_ticket = JsApiTicket.ticket;

            WXJSConfig config = new WXJSConfig();
            config.appid = AppIdCorpId;// 微信Id
            config.noncestr = signture.noncestr;// 随机码
            config.timestamp = signture.timestamp; // 时间戳
            config.signature = Utils.SHA1Hash(Utils.ObjectJoinString(signture, "{0}={1}&").Trim('&'));
            return config;
        }

        #endregion

        #region Oauth2 Authorize

        /// <summary>微信oauth2授权请求地址</summary>
        /// <param name="callbackurl">回调地址</param>
        /// <param name="state">传递参数</param>
        /// <param name="silent">是否静默授权，true 静默授权，false 用户确认授权，默认true</param>
        /// <returns></returns>
        public string Oauth2Authorize(string callbackurl, string state, bool silent = true)
        {
            // 微信授权请求地址
            string oauth2_url = "{0}?appid={1}&redirect_uri={2}&response_type=code&scope={3}&agentid={4}&state={5}#wechat_redirect".
                StrFormat(OAUTH2_AUTHORIZE_URL, AppIdCorpId, callbackurl.UrlEncode(), GetScope(silent), AgentId, state.UrlEncode());
            Log.Info("Redirect WeChat Oauth2:{0}", oauth2_url);
            return oauth2_url;
        }

        /// <summary>应用授权作用域</summary>
        /// <param name="silent"></param>
        /// <returns></returns>
        protected string GetScope(bool silent = true)
        {
            bool iscorp = string.IsNullOrEmpty(AgentId);
            // snsapi_base：静默授权，公众号（不弹出授权页面，直接跳转，只能获取用户openid）,企业号（可获取成员的基础信息）；
            // snsapi_userinfo：公众号（弹出授权页面，可通过openid拿到昵称、性别、所在地。并且，即使在未关注的情况下，只要用户授权，也能获取其信息），企业号（静默授权，可获取成员的详细信息，但不包含手机、邮箱）；
            // snsapi_privateinfo：企业号（手动授权，可获取成员的详细信息，包含手机、邮箱）。
            return silent ? (iscorp ? "snsapi_base" : "snsapi_userinfo") : (iscorp ? "snsapi_userinfo" : "snsapi_privateinfo");
        }

        /// <summary>公众号获取微信用户信息</summary>
        /// <param name="acctoken">网页授权接口调用凭证,注意：oauth2授权时此access_token与基础支持的access_token不同</param>
        /// <param name="oauth2">是否微信oauth2授权</param>
        /// <returns></returns>
        public WXUserInfo GetUserInfo(WXAccessToken acctoken, bool oauth2 = true)
        {
            // 获取微信用户信息请求地址
            string userinfo_url = "{0}/{1}?access_token={2}&openid={3}&lang=zh_CN".StrFormat(
                API_URL, oauth2 ? "sns/userinfo" : "cgi-bin/user/info", acctoken.access_token, acctoken.openid);
            // 获取微信用户信息
            WXUserInfo user_info = WebHttp.SendGet<WXUserInfo>(userinfo_url);
            Log.Info("Get WeChat Oauth2 UserInfo：{0} \n Response：{1}", userinfo_url, Utils.JsonSerialize(user_info));
            return user_info;
        }

        /// <summary>根据code获取企业号成员授权信息</summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public MemberTicket GetMemberTicket(string code)
        {
            // 请求地址
            string getuserinfo_url = "{0}/cgi-bin/user/getuserinfo?access_token={1}&code={2}".
                StrFormat(CORP_URL, AccessToken.access_token, code);
            // 获取企业微信成员授权信息
            MemberTicket mticket = WebHttp.SendGet<MemberTicket>(getuserinfo_url);
            Log.Info("Get Corp WeChat Oauth2 Ticket：{0} \n Response：{1}", getuserinfo_url, Utils.JsonSerialize(mticket));
            return mticket;
        }

        /// <summary>根据企业号成员授权票据获取成员信息</summary>
        /// <param name="userticket"></param>
        /// <returns></returns>
        public WXMemberInfo GetMemberInfo(string userticket)
        {
            // 请求地址
            string getuserinfo_url = "{0}/cgi-bin/user/getuserdetail?access_token={1}".
                StrFormat(CORP_URL, AccessToken.access_token);
            // 获取企业微信成员授权信息
            WXMemberInfo member = WebHttp.SendPost<WXMemberInfo>(getuserinfo_url, Utils.JsonSerialize(new { user_ticket = userticket }));
            Log.Info("Get WeChat Oauth2 UserInfo：{0} \n Response：{1}", getuserinfo_url, Utils.JsonSerialize(member));
            return member;
        }

        #endregion

        #region Menu Create

        /// <summary>自定义菜单创建</summary>
        /// <param name="mus">菜单</param>
        public WXErrorMsg MenuCreate(WXMenus mus)
        {
            string menu_manager_url = "{0}/cgi-bin/menu/create?access_token={1}".StrFormat(
                API_URL, AccessToken.access_token);
            string param = Utils.JsonSerialize(mus);
            // 创建自定义菜单
            WXErrorMsg result = WebHttp.SendPost<WXErrorMsg>(menu_manager_url, param);
            Log.Info("Create WeChat Menus:{0} \n Params:{1} \n Response：{2}",
                menu_manager_url, param, Utils.JsonSerialize(result));
            return result;

        }

        #endregion

        #region Message

        /// <summary>微信模板消息</summary>
        /// <param name="template">消息对象信息</param>
        public WXErrorMsg SendTemplateMsg(WXMsgTemplate template)
        {
            string send_msg_url = string.Format("{0}/cgi-bin/message/template/send?access_token={1}",
                API_URL, AccessToken.access_token);
            string param = Utils.JsonSerialize(template);
            // 发送模板消息
            WXErrorMsg result = WebHttp.SendPost<WXErrorMsg>(send_msg_url, param);
            Log.Info("Send WeChat Template Msg:{0} \n Params:{1} \n Response：{2}",
                send_msg_url, param, Utils.JsonSerialize(result));
            return result;
        }

        /// <summary>微信客服消息</summary>
        /// <param name="msg">消息对象信息</param>
        public WXErrorMsg SendCustomMsg(WXMsgCustom msg)
        {
            string send_msg_url = string.Format("{0}/cgi-bin/message/custom/send?access_token={1}",
                API_URL, AccessToken.access_token);
            string param = Utils.JsonSerialize(msg);
            // 发送模板消息
            WXErrorMsg result = WebHttp.SendPost<WXErrorMsg>(send_msg_url, param);
            Log.Info("Send WeChat Custom Msg:{0} \n Params:{1} \n Response：{2}",
                send_msg_url, param, Utils.JsonSerialize(result));
            return result;
        }

        #endregion

        #region Event Message Logic
        /*
        /// <summary>Get</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(string id) { return new HttpResponseMessage(); }

        /// <summary>Post</summary>
        /// <param name="dto">消息数据对象</param>
        /// <returns></returns>
        public HttpResponseMessage Post([FromBody]WXEventMsg dto)
        {
            string content = "success";
            switch (dto.MsgType)
            {
                case "event":
                    content = EventMsg(dto);
                    break;
                case "text":

                    break;
                case "image":

                    break;
                case "voice":

                    break;
                case "video":
                case "shortvideo":

                    break;
                case "location":

                    break;
                case "link":

                    break;
                default: break;
            }
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(content, Encoding.UTF8, "application/xml") };
            return result;
        }

        /// <summary>事件消息处理</summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private string EventMsg(WXEventMsg msg)
        {
            string result = "success";
            switch (msg.Event)
            {
                case "subscribe":
                    result = SubscribeMsg(msg.FromUserName, msg.ToUserName);
                    break;
                case "unsubscribe": break;
                case "SCAN":

                    break;
                case "LOCATION":

                    break;
                case "CLICK":

                    break;
                case "VIEW":

                    break;
                default: break;
            }
            return result;
        }

        /// <summary>用户在关注公众号时响应消息</summary>
        /// <param name="ToUserName">接收方帐号（收到的OpenID）</param>
        /// <param name="FromUserName">开发者微信号</param>
        /// <returns></returns>
        private string SubscribeMsg(string ToUserName, string FromUserName)
        {
            WXEventReplyMsg msg = new WXEventReplyMsg();
            msg.ToUserName = ToUserName;
            msg.FromUserName = FromUserName;
            msg.CreateTime = DateTime.Now.ToTimeStamp();
            msg.MsgType = "news";
            msg.Articles = new List<MsgItem>();
            msg.Articles.Add(new MsgItem
            {
                Title = "Welcome to Thermo Fisher China",
                Description = "Find details and Purchase",
                PicUrl = "http://wx.thermofisher.com/iThermo/api/static/welcome.png",
                Url = "http://runningcloud.h5release.com/h5/95fb4234-88f9-6c8f-1321-f02528e70cdb.html"
            });
            return msg.ToXmlSerialize();
        }
        */
        #endregion
    }
}
