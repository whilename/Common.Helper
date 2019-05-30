using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wechat.Models
{
    /// <summary>微信js-api配置</summary>
    public class WXJSConfig
    {
        /// <summary>必填，公众号的唯一标识</summary>
        public string appid { get; set; }
        /// <summary>必填，生成签名的时间戳</summary>
        public string timestamp { get; set; }
        /// <summary>必填，生成签名的随机串</summary>
        public string noncestr { get; set; }
        /// <summary>签名</summary>
        public string signature { get; set; }
    }

    /// <summary>微信加密签名</summary>
    public class WXJSSignature
    {
        /// <summary>加密随机字符</summary>
        public string noncestr { get; set; }
        /// <summary>JS-SDK权限验证加密所需的加密串</summary>
        public string jsapi_ticket { get; set; }
        /// <summary>时间戳</summary>
        public string timestamp { get; set; }
        /// <summary>当前页面URL地址</summary>
        public string url { get; set; }
    }
}