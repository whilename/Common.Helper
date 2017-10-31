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

    /// <summary>企业号成员授权信息</summary>
    public class MemberTicket
    {
        /// <summary>返回码</summary>
        public string errcode { get; set; }
        /// <summary>对返回码的文本描述内容</summary>
        public string errmsg { get; set; }
        /// <summary>成员UserID</summary>
        public string UserId { get; set; }
        /// <summary>手机设备号(由企业微信在安装时随机生成，删除重装会改变，升级不受影响)</summary>
        public string DeviceId { get; set; }
        /// <summary>成员票据，最大为512字节。scope为snsapi_userinfo或snsapi_privateinfo，且用户在应用可见范围之内时返回此参数。后续利用该参数可以获取用户信息或敏感信息。</summary>
        public string user_ticket { get; set; }
        /// <summary>user_token的有效时间（秒），随user_ticket一起返回</summary>
        public string expires_in { get; set; }
    }

}