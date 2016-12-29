using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Common.Utility
{
    /// <summary>获取配置文件中的Message信息</summary>
    public sealed class Message
    {
        static Message instance = null;
        static readonly object padlock = new object();
        private IDictionary<string, SysMessage> messagelist;

        private Message()
        {
            string configfile = AppDomain.CurrentDomain.BaseDirectory + "Message.config";
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(configfile);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmldoc.NameTable);
            nsmgr.AddNamespace("messageConfig", "http://www.duileme.cn/messageConfig");
            XmlNodeList messageNodes = xmldoc.SelectNodes("//messageConfig:Message", nsmgr);
            messagelist = new Dictionary<string, SysMessage>();
            string key, value;
            foreach (XmlNode messageNode in messageNodes)
            {
                key = messageNode.Attributes["id"].Value;
                value = messageNode.Attributes["value"].Value;
                messagelist.Add(key, new SysMessage(key, value));
            }
        }

        /// <summary>重新加载配置文件</summary>
        public static void Reload()
        {
            lock (padlock)
            {
                instance = null;
                instance = new Message();
            }
        }

        /// <summary>得到Message对象的全局访问点方法</summary>
        public static Message Instance
        {
            get
            {
                if (instance == null) { lock (padlock) { if (instance == null) { instance = new Message(); } } }
                return instance;
            }
        }

        /// <summary>根据消息Code获取消息对象</summary>
        /// <param name="code">消息代码</param>
        /// <param name="args">消息附加信息</param>
        /// <returns>SysMessage消息对象</returns>
        public SysMessage GetMessage(string code, params string[] args)
        {
            SysMessage message = null;
            message = (SysMessage)messagelist[code];
            message.data = null;
            if (message == null) { message = SysMessage.UnkownMessage; }
            else { message.args = args; }
            return message;
        }

        /// <summary>根据message的id值得到message的value</summary>
        /// <param name="code">message的id值</param>
        /// <param name="args">message的替换参数值</param>
        /// <returns>message的value</returns>
        public string GetMessageText(string code, params string[] args)
        {
            SysMessage message = GetMessage(code, args);
            return string.Format(message.msg, message.args);
        }

    }

    /// <summary>消息对象</summary>
    public class SysMessage
    {
        /// <summary></summary>
        public static SysMessage UnkownMessage = new SysMessage("E999", "当前服务不可用，请稍后重试");

        /// <summary></summary>
        public SysMessage() { }
        /// <summary></summary>
        public SysMessage(string key, string text) { this._code = key; this._msg = text; }

        private string _code;
        /// <summary>
        /// 消息编码
        /// </summary>
        public string code { get { return this._code.StartsWith("E") || this._code.Equals("-1") ? this._code : "0"; } set { _code = value; } }

        private string _msg;
        /// <summary>
        /// 消息文本
        /// </summary>
        public string msg { get { return string.Format(this._msg, this._args); } set { _msg = value; } }

        private object _data;
        /// <summary>
        /// 附加对象
        /// </summary>
        public object data { get { return _data; } set { _data = value; } }

        private string[] _args;
        /// <summary>
        /// 参数
        /// </summary>
        internal string[] args { get { return _args; } set { _args = value; } }

        /// <summary></summary>
        /// <returns></returns>
        public override string ToString() { return string.Format(this._msg, this._args); }

    }
}
