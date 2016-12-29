using Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat
{
    /// <summary>
    /// 配置信息
    /// </summary>
    public class WXConfig
    {
        /************ 微信公众号信息配置 **********************
         * APPID：绑定支付的APPID（必须配置）
         * MCHID：商户号（必须配置）
         * KEY：  商户支付密钥，参考开户邮件设置（必须配置）
         * SECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
         */
        internal static string APPID { get; private set; }
        internal static string MCHID { get; private set; }
        internal static string KEY { get; private set; }
        internal static string SECRET { get; private set; }

        // 微信授权地址
        internal static string OAUTH2_AUTHORIZE_URL = "https://open.weixin.qq.com/connect/oauth2/authorize";
        // 微信授权获取access token地址
        internal static string OAUTH2_ACCESS_TOKEN_URL = "https://api.weixin.qq.com/sns/oauth2/access_token";
        // 获取授权用户信息地址
        internal static string OAUTH2_USERINFO_URL = "https://api.weixin.qq.com/sns/userinfo";
        // 获取微信用户信息地址
        internal static string CGIBIN_USERINFO_URL = "https://api.weixin.qq.com/cgi-bin/user/info";
        // 获取一般access token地址
        internal static string ACCESS_TOKEN_URL = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential";
        // 获取微信JS-SDK权限票据地址
        internal static string GETTICKET_URL = "https://api.weixin.qq.com/cgi-bin/ticket/getticket";
        // 微信支付预订单请求地址
        internal static string PAY_UNIFIEDORDER_URL = "https://api.weixin.qq.com/cgi-bin/ticket/getticket";
        // 微信模板消息推送请求地址
        internal static string SEND_MSG_TEMPLATE_URL = "https://api.weixin.qq.com/cgi-bin/message/template/send";

        /// <summary>系统初始化配置</summary>
        public static void Register()
        {
            var e = Utils.GetSettingValue("Environment");
            switch (e)
            {
                case "dev":
                    Initialize("wx165579c7fca21c14", "", "", "dcd8424687ebdf63e116aca973ddc645");
                    break;
                case "test":
                    Initialize("wx165579c7fca21c14", "", "", "dcd8424687ebdf63e116aca973ddc645");
                    break;
                case "pro":
                    Initialize("wx165579c7fca21c14", "", "", "dcd8424687ebdf63e116aca973ddc645");
                    break;
                default:
                    Initialize("wx165579c7fca21c14", "", "", "dcd8424687ebdf63e116aca973ddc645");
                    break;
            }
        }

        /// <summary>初始化</summary>
        /// <param name="appid"></param>
        /// <param name="mchid"></param>
        /// <param name="key"></param>
        /// <param name="secret"></param>
        private static void Initialize(string appid, string mchid, string key, string secret)
        {
            APPID = appid;
            MCHID = mchid;
            KEY = key;
            SECRET = secret;
            // 启动缓存微信鉴权信息，7000秒刷新一次
            WXCommon.Start();
        }

    }
}