using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utility
{
    /// <summary>
    /// WebHttp Helper
    /// </summary>
    public class WebHttp
    {
        #region SendPost

        /// <summary>发送Post请求</summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="url">地址</param>
        /// <param name="param">参数</param>
        /// <returns>本次请求的响应结果</returns>
        public static T SendPost<T>(string url, string param)
        {
            string contenttype = "application/json";
            var result = SendHttpRequeset(url, param, "post", "utf-8", 0, contenttype);
            return (T)DeserializeObject<T>(result);
        }

        #endregion

        #region SendGet

        /// <summary>发送Get请求</summary>
        /// <param name="url">地址</param>
        /// <returns>本次请求的响应结果</returns>
        public static T SendGet<T>(string url)
        {
            var result = SendHttpRequeset(url, "", "get");
            return (T)DeserializeObject<T>(result);
        }

        #endregion

        #region SendPut

        /// <summary>发送Put请求</summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="url">地址</param>
        /// <param name="param">参数</param>
        /// <returns>本次请求的响应结果</returns>
        public static T SendPut<T>(string url, string param)
        {
            var result = SendHttpRequeset(url, param, "put");
            return (T)DeserializeObject<T>(result);
        }

        #endregion

        #region SendDelete

        /// <summary>发送Put请求</summary>
        /// <param name="url">地址</param>
        /// <param name="param">参数</param>
        /// <returns>本次请求的响应结果</returns>
        public static HttpResponseMessage SendDelete(string url, string param)
        {
            return SendHttpRequeset(url, param, "delete");
        }

        #endregion

        #region SendHttpRequeset
        
        /// <summary>发送请求</summary>
        /// <param name="url">地址</param>
        /// <param name="param">参数</param>
        /// <param name="requesttype">类型</param>
        /// <param name="charset">字符集格式</param>
        /// <param name="contenttype">请求相应格式application/x-www-form-urlencoded,或 application/json</param>
        /// <returns>本次请求的响应结果</returns>
        public static string SendRequest(string url, string param, string requesttype, string charset = "utf-8", string contenttype = "application/x-www-form-urlencoded")
        {
            var responseMessage = SendHttpRequeset(url, param, requesttype, charset, 0, contenttype);
            if (responseMessage.IsSuccessStatusCode)
                return responseMessage.Content.ReadAsStringAsync().Result;
            return string.Empty;
        }

        /// <summary>发送Http请求</summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">请求参数</param>
        /// <param name="requesttype">请求方式</param>
        /// <param name="charset">字符集格式</param>
        /// <param name="timeout">请求超时时间</param>
        /// <param name="contenttype">媒体类型</param>
        /// <returns></returns>
        private static HttpResponseMessage SendHttpRequeset(string url, string param, string requesttype, string charset = "utf-8", int timeout = 0, string contenttype = "application/x-www-form-urlencoded")
        {
            //设置HttpClientHandler的AutomaticDecompression
            var handler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip };
            using (var client = new HttpClient(handler))
            {
                if (timeout > 0) { client.Timeout = TimeSpan.FromMilliseconds(timeout); }
                var context = new StringContent(param.ToString());
                context.Headers.ContentType = new MediaTypeHeaderValue(contenttype) { CharSet = charset };

                switch (requesttype)
                {
                    case "get":
                        return client.GetAsync(url).Result;
                    case "post":
                        return client.PostAsync(url, context).Result;
                    case "put":
                        return client.PutAsync(url, context).Result;
                    case "delete":
                        return client.DeleteAsync(url).Result;
                    default:
                        return null;
                }
            }
        }

        #endregion

        #region DeserializeObject

        /// <summary>反序列化返回结果</summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="httpResponseMessage">http 响应消息</param>
        /// <returns></returns>
        private static object DeserializeObject<T>(HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage == null) return default(T);
            var resultJson = httpResponseMessage.IsSuccessStatusCode ? httpResponseMessage.Content.ReadAsStringAsync().Result : string.Empty;
            return string.IsNullOrEmpty(resultJson) ? default(T) : JsonConvert.DeserializeObject<T>(resultJson);
        }

        #endregion

        #region HttpWeb

        /// <summary>发送GET请求，接收返回数据</summary>
        /// <param name="url"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string HttpWebRequestGet(string url, string charset = "ASCII")
        {
            try
            {
                string result;
                // System.Net.ServicePointManager.DefaultConnectionLimit=100; // 设置最大请求连接数
                var request = WebRequest.Create(url);
                using (var response = request.GetResponse())
                {
                    var resReader = new StreamReader(response.GetResponseStream() ?? Stream.Null, Encoding.GetEncoding(charset), false);
                    result = resReader.ReadToEnd();
                    resReader.Close();
                    resReader.Dispose();
                }
                request.Abort();
                return result;
            }
            catch (Exception ex) { Log.Error(ex); return ex.Message; }
        }

        /// <summary>发送POST请求，接受返回数据</summary>
        /// <typeparam name="T">返回的数据对象</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="param">请求提交参数对象</param>
        /// <returns>返回 T 数据对象</returns>
        public static T HttpWebRequestPost<T>(string url, string param)
        {
            string result = HttpWebRequestPost(url, param, 0, "utf-8", "application/json");
            return string.IsNullOrEmpty(result) ? default(T) : JsonConvert.DeserializeObject<T>(result);
        }

        /// <summary>发送POST请求，接受返回数据</summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">请求提交参数对象</param>
        /// <param name="timeout">设置所发送的请求超时时间</param>
        /// <param name="charset">设置所发送的请求提交参数编码格式</param>
        /// <param name="contentType">设置所发送的请求数据的内容类型,默认 application/x-www-form-urlencoded 格式</param>
        /// <returns>返回 T 数据对象</returns>
        public static string HttpWebRequestPost(string url, string param, int timeout, string charset = "ASCII", string contentType = "application/x-www-form-urlencoded")
        {
            try
            {
                string result;
                // System.Net.ServicePointManager.DefaultConnectionLimit=100; // 设置最大请求连接数
                Encoding encode = Encoding.GetEncoding(charset);
                var by = encode.GetBytes(param);
                var request = WebRequest.Create(url);
                if (timeout > 0) { request.Timeout = timeout; }
                request.Method = "POST";
                request.ContentType = contentType;
                request.ContentLength = by.Length;
                //获取请求时写入资源对象
                using (var st = request.GetRequestStream())
                {
                    //写入请求数据
                    st.Write(by, 0, by.Length);
                    //获取请求响应对象
                    var response = request.GetResponse();
                    //获取请求响应信息流
                    var read = new StreamReader(response.GetResponseStream() ?? Stream.Null, encode, false);
                    result = read.ReadToEnd();
                    read.Close();
                    read.Dispose();
                    response.Close();
                }
                request.Abort();
                return result;
            }
            catch (Exception ex) { Log.Error(ex); return string.Empty; }
        }

        #endregion
    }
}
