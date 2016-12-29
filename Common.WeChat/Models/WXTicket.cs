using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat.Models
{
    /// <summary>微信JS接口的临时票据</summary>
    public class WXTicket
    {
        /// <summary>错误代码</summary>
        public string errcode { get; set; }
        /// <summary>错误消息</summary>
        public string errmsg { get; set; }
        /// <summary>JS-SDK权限验证加密所需的加密串</summary>
        public string ticket { get; set; }
        /// <summary>失效时间</summary>
        public double expires_in { get; set; }
    }
}