using Common.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Wechat;

namespace Wechat.Models
{
    /// <summary>基类模型</summary>
    public class WXPayBase
    {
        /// <summary>属性集合</summary>
        protected Hashtable properties;

        /// <summary>公众账号ID,微信支付分配的公众账号ID（企业号corpid即为此appId）</summary>
        public string appid { get { return GetVal("appid"); } set { SetVal("appid", value); } }
        /// <summary>商户号,微信支付分配的商户号</summary>
        public string mch_id { get { return GetVal("mch_id"); } set { SetVal("mch_id", value); } }
        /// <summary>设备号(非必要),自定义参数，可以为终端设备号(门店号或收银设备ID)，PC网页或公众号内支付可以传"WEB"</summary>
        public string device_info { get { return GetVal("device_info"); } set { SetVal("device_info", value); } }
        /// <summary>随机字符串,随机字符串，长度要求在32位以内</summary>
        public string nonce_str { get { return GetVal("nonce_str"); } set { SetVal("nonce_str", value); } }

        public string key { get; set; }
        /// <summary>签名,通过签名算法计算得出的签名值</summary>
        public string sign
        {
            get
            {
                StringBuilder sbstr = new StringBuilder();
                ArrayList sort_keys = new ArrayList(properties.Keys);
                sort_keys.Sort();// 参数名ASCII码从小到大排序（字典序）
                foreach (string k in sort_keys)
                {
                    object v = properties[k];
                    // 如果参数的值为空不参与签名
                    if (v == null || string.IsNullOrEmpty(v.ToString()) || k.Equals("sign")) continue;
                    sbstr.AppendFormat("{0}={1}&", k, v);
                }
                sbstr.AppendFormat("key={0}", key);
                properties["sign"] = Utils.MD5Hash(sbstr.ToString()).ToUpper();
                return properties["sign"].ToString();
            }
        }

        /// <summary>获取属性值</summary>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public string GetVal(string key, string def = "") { return properties[key] == null ? def : properties[key].ToString(); }

        /// <summary>设置属性值</summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void SetVal(string key, object val) { properties[key] = val; }

        /// <summary>加载Xml对象</summary>
        /// <param name="xml"></param>
        public void Reload(string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNode root = xmlDoc.SelectSingleNode("xml");
            XmlNodeList xnl = root.ChildNodes;
            foreach (XmlNode xnf in xnl) { properties[xnf.Name] = xnf.InnerText; }
        }

        /// <summary>转换成xml报文</summary>
        /// <returns></returns>
        public string ToXml()
        {
            StringBuilder sbstr = new StringBuilder("<xml>");
            properties["sign"] = sign;
            foreach (string k in properties.Keys) { sbstr.AppendFormat("<{0}><![CDATA[{1}]]></{0}>", k, GetVal(k)); }
            return sbstr.Append("</xml>").ToString();
        }
    }
}
