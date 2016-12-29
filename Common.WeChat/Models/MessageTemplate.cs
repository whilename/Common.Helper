using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTZB.WeChat.Models
{
    public class MessageTemplate
    {
        public string touser { get; set; }
        public string template_id { get; set; }
        public string url { get; set; }

        public object data { get; set; }

    }
    public class Content2
    {
        public KeyValue first { get; set; }
        public KeyValue keyword1 { get; set; }
        public KeyValue keyword2 { get; set; }
        public KeyValue remark { get; set; }
    }
    public class KeyValue
    {
        public string value { get; set; }

        public string color { get; set; }
    }
}