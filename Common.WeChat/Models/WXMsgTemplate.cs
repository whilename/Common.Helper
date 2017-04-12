using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat.Models
{
    /// <summary>微信模板消息对象</summary>
    public class WXMsgTemplate
    {
        /// <summary>接收者openid</summary>
        public string touser { get; set; }
        /// <summary>模板ID</summary>
        public string template_id { get; set; }
        /// <summary>模板跳转链接</summary>
        public string url { get; set; }
        /// <summary>模板数据</summary>
        public object data { get; set; }

    }
    /// <summary>消息内容</summary>
    public class KeyValue
    {
        /// <summary>消息内容</summary>
        public string value { get; set; }
        /// <summary>颜色值(十六进制)</summary>
        public string color { get; set; }
    }

    /// <summary>模板1</summary>
    public class TemplateA
    {
        /// <summary></summary>
        public KeyValue first { get; set; }
        /// <summary></summary>
        public KeyValue keyword1 { get; set; }
        /// <summary></summary>
        public KeyValue keyword2 { get; set; }
        /// <summary></summary>
        public KeyValue keyword3 { get; set; }
        /// <summary></summary>
        public KeyValue remark { get; set; }
    }
}