using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wechat.Models
{
    /// <summary>错误消息</summary>
    public class WXErrorMsg
    {
        /// <summary>错误代码</summary>
        public string errcode { get; set; }
        /// <summary>错误描述</summary>
        public string errmsg { get; set; }
        /// <summary>消息编号</summary>
        public string msgid { get; set; }

        /// <summary>用户的唯一标识</summary>
        public string openid { get; set; }
        /// <summary>应用的appid，若请求包中不包含agentid则不返回appid；该appid在使用微信红包时会用到</summary>
        public string appid { get; set; }
        /// <summary>成员列表</summary>
        public string userlist { get; set; }

    }
}
