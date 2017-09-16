using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Controllers;
using System.Xml.Serialization;

namespace Common.Utility
{
    /// <summary>
    /// 公共函数类
    /// </summary>
    public class Utils
    {
        /// <summary>获取web.config中AppSetting的设置</summary>
        /// <param name="key">键名称</param>
        /// <returns>返回指定key对应的value值，否则返回Empty。</returns>
        public static string GetSettingValue(string key)
        {
            string value = ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
        }

        /// <summary>移动电话运营商</summary>
        /// <param name="numbers">手机号</param>
        /// <returns>YD: 移动，LT: 联通，DX: 电信</returns>
        public static string MobilePhoneOperator(string numbers)
        {
            Regex regex = new Regex("^(1(3[4-9]|4[7]|5[012789]|7[8]|8[23478])\\d{8})|(1(70[5])\\d{7})$");
            if (regex.IsMatch(numbers)) { return "YD"; }
            regex = new Regex("^(1(3[012]|4[5]|5[56]|7[6]|8[56])\\d{8})|(1(70[9])\\d{7})$");
            if (regex.IsMatch(numbers)) { return "LT"; }
            regex = new Regex("^(1(3[3]|5[3]|7[7]|8[019])\\d{8})|(1(70[0])\\d{7})$");
            if (regex.IsMatch(numbers)) { return "DX"; }
            return string.Empty;
        }

        /// <summary>匹配是否是十进制整数值或小数值，^[0-9]+([.]{1}[0-9]+){0,1}$</summary>
        /// <param name="dm">匹配字符数值</param>
        /// <returns>如果正则表达式找到匹配项，则为 true；否则，为 false。</returns>
        public static bool RegexDecimal(string dm) { return new Regex("^[0-9]+([.]{1}[0-9]+){0,1}$").IsMatch(dm); }

        /*
        /// <summary>图文识别</summary>
        /// <param name="imgPath"></param>
        /// <returns></returns>
        public static string ImageToString(string imgPath)
        {
            MODI.Document doc = new MODI.Document(); // 需要office 2007 Microsoft Office Document Imaging 组件
            doc.Create(imgPath);
            MODI.Image image;
            MODI.Layout layout;
            doc.OCR(MODI.MiLANGUAGES.miLANG_CHINESE_SIMPLIFIED, true, true); // 识别简体中文

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < doc.Images.Count; i++)
            {
                image = (MODI.Image)doc.Images[i];
                layout = image.Layout;
                sb.Append(layout.Text);
            }
            return sb.ToString();
        }
         * */

        #region 加密处理（MD5）（SHA1）

        /// <summary>MD5加密</summary>
        /// <param name="str">加密字符串</param>
        /// <param name="encoding">编码格式,default utf-8</param>
        /// <returns></returns>
        public static string MD5Hash(string str, string encoding = "UTF-8")
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] by = new byte[256];
            by = md5.ComputeHash(Encoding.GetEncoding(encoding).GetBytes(str));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in by)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>SHA1加密</summary>
        /// <param name="str">加密字符串</param>
        /// <param name="encoding">编码格式,default ASCII</param>
        /// <returns></returns>
        public static string SHA1Hash(string str, string encoding = "ASCII")
        {
            HashAlgorithm sha1 = SHA1.Create();
            byte[] by = new byte[256];
            by = sha1.ComputeHash(Encoding.GetEncoding(encoding).GetBytes(str));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in by)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        #endregion

        #region HttpHelper

        /// <summary>获取本地IPv4地址</summary>
        public static string GetHostIPV4()
        {
            string strHostName = Dns.GetHostName(); //得到本机的主机名
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName); //取得本机IP            
            return ipEntry.AddressList.Where(x => x.ToString().IndexOf(".") > 0).First().ToString();
        }

        /// <summary>获取IP地址</summary>
        public static string GetCustomerIp()
        {
            if (HttpContext.Current == null) { return ""; }
            // string srcIp = HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
            //获取实际客户端IP地址
            string srcIp = HttpContext.Current.Request.Headers["X-Forwarded-For"];
            if (string.IsNullOrEmpty(srcIp))
            {
                srcIp = HttpContext.Current.Request.UserHostAddress;
            }
            return srcIp;
        }

        /// <summary>获取当前请求的完整地址</summary>
        /// <returns>返回当前请求的完整地址</returns>
        public static string GetUrl()
        {
            if (HttpContext.Current == null) { return ""; }
            return HttpContext.Current.Request.Url.ToString();
        }

        /// <summary>获取当前站点的域名地址</summary>
        /// <returns>返回当前站点的域名地址，不包含请求协议(http://)</returns>
        public static string GetUrlHost()
        {
            if (HttpContext.Current == null) { return ""; }
            return HttpContext.Current.Request.Url.Host;
        }

        /// <summary>获取Url字符串中的请求参数字典</summary>
        /// <param name="url">Url字符串</param>
        /// <returns>请求参数字典</returns>
        public static Dictionary<string, object> GetQueryString(string url)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            int i = url.IndexOf("?") < 0 ? 0 : url.IndexOf("?");
            string[] dic_arr = url.Substring(++i).Split('&');
            foreach (var item in dic_arr)
            {
                string[] vals = item.Split('=');
                if (vals.Length > 0) { dic.Add(vals[0], vals[1]); }
            }
            return dic;
        }

        /// <summary>获取请求提交的表单字典列表</summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetFromBody(HttpActionContext context, string key)
        {
            // 表单信息字典
            Dictionary<string, object> frombody = new Dictionary<string, object>();
            try
            {
                if (context.ActionArguments.Count <= 0 || context.Request.Method.Method == "GET")
                {
                    frombody = context.ActionArguments;
                    if (frombody == null || frombody.Count <= 0)
                    {
                        frombody = GetQueryString(context.Request.RequestUri.Query);
                    }
                }
                else
                {
                    object obj = context.ActionArguments[key];// 获取指定的请求参数数据
                    if (obj == null) { return frombody; }
                    //获取对象的所有公共属性
                    var props = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    // 获得表单字典
                    for (int i = 0; i < props.Length; i++) { frombody.Add(props[i].Name, props[i].GetValue(obj, null)); }
                }
                return frombody;
            }
            catch (Exception ex) { Log.Error(ex); return frombody; }
        }

        /// <summary>获取请求提交的表单数据流字符串</summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetFromBodyStream(HttpActionContext context)
        {
            var task = context.Request.Content.ReadAsStreamAsync();
            var content = string.Empty;
            using (System.IO.Stream sm = task.Result)
            {
                if (sm != null)
                {
                    sm.Seek(0, System.IO.SeekOrigin.Begin);
                    int len = (int)sm.Length;
                    byte[] inputByts = new byte[len];
                    sm.Read(inputByts, 0, len);
                    sm.Close();
                    content = Encoding.UTF8.GetString(inputByts);
                }
            }
            return content;
        }

        /// <summary>解析Cookie</summary>
        public static Dictionary<string, object> GetRequestCookie()
        {
            var request = HttpContext.Current.Request;
            if (request.Cookies["monitor"] == null) { return null; }
            var url = string.IsNullOrWhiteSpace(request.Cookies["monitor"]["url"]) ? "" : request.Cookies["monitor"]["url"];
            if (string.IsNullOrWhiteSpace(url)) { return null; }
            var gb = Encoding.GetEncoding("gb2312");
            var utf = Encoding.GetEncoding("utf-8");
            if (url.IndexOf("baidu.com", StringComparison.Ordinal) > -1 && url.IndexOf("ie=utf-8", StringComparison.Ordinal) == -1)
                url = HttpUtility.UrlDecode(HttpUtility.UrlDecode(url, gb), gb);
            else if (url.IndexOf("sogou.com", StringComparison.Ordinal) > -1)
                url = HttpUtility.UrlDecode(HttpUtility.UrlDecode(url, gb), gb);
            else
                url = HttpUtility.UrlDecode(HttpUtility.UrlDecode(url, utf), utf);

            var jumpDateTime = string.IsNullOrWhiteSpace(request.Cookies["monitor"]["time"])
                    ? DateTime.Now
                    : Convert.ToDateTime(request.Cookies["monitor"]["time"]);
            var refererUrl = string.IsNullOrWhiteSpace(url) ? "" : url.Replace("$", "&");
            var dic = new Dictionary<string, object> { { "JumpDateTime", jumpDateTime }, { "RefererUrl", refererUrl } };
            return dic;
        }

        #endregion

        #region ObjectHelper

        /// <summary>对象同名同类型属性值复制</summary>
        /// <param name="oFrom">源对象</param>
        /// <param name="oTo">目标对象</param>
        public static void ObjectCopyValue(object oFrom, object oTo)
        {
            // 获取对象的公共属性(带get/set访问器)
            PropertyInfo[] propsF = oFrom.GetType().GetProperties();
            PropertyInfo[] propsT = oTo.GetType().GetProperties();
            foreach (PropertyInfo pFrom in propsF)
            {
                foreach (PropertyInfo pTo in propsT)
                {
                    if (pFrom.Name.Equals(pTo.Name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        try { pTo.SetValue(oTo, pFrom.GetValue(oFrom, null), null); }
                        catch (Exception ex) { Log.Error(ex); }
                        break;
                    }
                }
            }
        }

        /// <summary>对象同名同类型字段值复制</summary>
        /// <param name="oFrom">源对象</param>
        /// <param name="oTo">目标对象</param>
        public static void ObjectCopyFieldValue(object oFrom, object oTo)
        {
            // 获取对象的所有公共字段
            FieldInfo[] fieldsF = oFrom.GetType().GetFields();
            FieldInfo[] fieldsT = oTo.GetType().GetFields();
            foreach (FieldInfo fFrom in fieldsF)
            {
                foreach (FieldInfo fTo in fieldsT)
                {
                    if (fFrom.Name.Equals(fTo.Name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        try { fTo.SetValue(oTo, fFrom.GetValue(oFrom)); }
                        catch (Exception ex) { Log.Error(ex); }
                        break;
                    }
                }
            }
        }

        /// <summary>将指定对象转换成url参数字符串</summary>
        /// <param name="obj">需要转换的对象</param>
        /// <param name="trimEmpty">是否去除空值属性字段，默认true去除，false不去除</param>
        /// <param name="keyValue">是否字段名加性值，默认true，false单值</param>
        /// <param name="fromat">拼接格式，配合keyvalue使用，默认字段名加值,Fromat("{0}{1}",fieldname,value)</param>
        /// <returns></returns>
        public static string ObjectJoinString(object obj, bool trimEmpty = true, bool keyValue = true, string fromat = "{0}{1}")
        {
            StringBuilder url_str = new StringBuilder();
            // 按键排序的键/值对集合
            SortedDictionary<string, object> sort_dic = new SortedDictionary<string, object>();
            // 对象属性集合
            var props = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            object value;
            for (int i = 0; i < props.Length; i++)
            {
                // 获取属性值
                value = props[i].GetValue(obj, null);
                // 去除值为空
                if (trimEmpty && value == null) { continue; }
                sort_dic.Add(props[i].Name, value);
            }
            foreach (var item in sort_dic)
            {
                // 拼接字符串
                if (keyValue) { url_str.AppendFormat(fromat, item.Key, item.Value); }
                else { url_str.Append(item.Value); }
            }
            return url_str.ToString();
        }

        /// <summary>Json序列化</summary>
        /// <param name="value">序列化的对象</param>
        /// <returns>返回序列化后的Json格式字符串</returns>
        public static string JsonSerialize(object value)
        {
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            return JsonConvert.SerializeObject(value, timeConverter);
        }

        /// <summary>Json反序列化</summary>
        /// <typeparam name="T">指定对象类型</typeparam>
        /// <param name="value">要序列化的Json格式字符串</param>
        /// <returns>返回序列化成功后的指定对象类型</returns>
        public static T JsonDeserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>序列化对象为Xml字符串</summary>
        /// <param name="o">序列化对象</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>返回序列后的Xml字符串</returns>
        public static string XmlSerialize(Object o, string encoding = "ASCII")
        {
            string resultXml = string.Empty;
            if (o == null) return resultXml;
            XmlSerializer serialiazer = new XmlSerializer(o.GetType());
            MemoryStream memStream = new MemoryStream();
            using (System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(memStream, Encoding.GetEncoding(encoding)))
            {
                serialiazer.Serialize(writer, o);
                memStream.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(memStream);
                resultXml = sr.ReadToEnd();
                memStream.Dispose();
            }
            return resultXml;
        }

        /// <summary>序列化Xml字符串为指定对象</summary>
        /// <typeparam name="T">指定对象类型</typeparam>
        /// <param name="value">Xml字符串</param>
        /// <returns>返回序列化成功后的指定对象类型</returns>
        public static T XmlDeserialize<T>(string value)
        {
            try
            {
                using (StringReader sr = new StringReader(value))
                {
                    XmlSerializer xmldoc = new XmlSerializer(typeof(T));
                    return (T)xmldoc.Deserialize(sr);
                }
            }
            catch (Exception ex) { Log.Error(ex); return default(T); }
        }
        
        #endregion

        #region DateHelper

        /// <summary>获取指定日期时间的当年第几周</summary>
        /// <param name="date">日期时间</param>
        /// <returns></returns>
        public static int WeekOfYear(DateTime date)
        {
            GregorianCalendar gc = new GregorianCalendar();
            return gc.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday) - 1;
        }

        /// <summary>获取指定年份指定周的起止时间</summary>
        /// <param name="year">年</param>
        /// <param name="week">周</param>
        /// <param name="first">起始日期</param>
        /// <param name="last">结束日期</param>
        /// <returns></returns>
        public static bool DaysOfWeeks(int year, int week, out DateTime first, out DateTime last)
        {
            DateTime first_date = new DateTime(year, 1, 1);
            DateTime last_date = new DateTime(year + 1, 1, 1).AddMilliseconds(-1);
            int dayofweek = Convert.ToInt32(first_date.DayOfWeek.ToString("d"));
            if (week == 1)
            {
                first = first_date;
                if (dayofweek == 6) { last = first; }
                else { last = first_date.AddDays(7 - dayofweek); }
            }
            else
            {
                // 因为下标是从0开始所以周-1
                first = first_date.AddDays((7 - (dayofweek - 1)) + week * 7);
                last = first.AddDays(6);
                if (last > last_date) { last = last_date; }
            }
            return true;
        }

        #endregion
    }
}
