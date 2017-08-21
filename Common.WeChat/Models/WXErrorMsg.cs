using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeChat.Models
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

    }
}
