﻿using Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Common.Library.Attributes
{
    /// <summary>自定义过滤器基类</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
    public abstract class BaseApiActionFilterAttribute : ActionFilterAttribute
    {
        /// <summary>允许的站点</summary>
        public string[] AllowSites { get; set; }

        /// <summary>获取请求提交的JSON表单或数据字典列表中指定的键值</summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual object FromBody(HttpActionContext context, string key)
        {
            try
            {
                // 表单信息字典
                IDictionary<string, object> frombody = FromBody(context);
                return frombody.First(x => x.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase)).Value;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }

        /// <summary>获取请求提交的JSON表单或数据字典列表</summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual IDictionary<string, object> FromBody(HttpActionContext context)
        {
            return Utils.GetFromBody(context);
        }

        /// <summary>检查允许跨域请求站点</summary>
        /// <param name="context"></param>
        public virtual void OnAllowOriginExcute(HttpActionExecutedContext context)
        {
            Dictionary<string, string> headers = context.Request.Headers.ToDictionary(x => x.Key, x => string.Join(" ", x.Value));
            // 获取指定的请求头信息
            string origin = headers.ContainsKey("Origin") ? headers["Origin"] : string.Empty;
            // 判断是否在允许请求站点列表中
            if (AllowSites != null && AllowSites.Contains(origin))
            {
                // 加入允许请求，IE10以下或某些低版本的浏览器可能不支持
                context.Response.Headers.Add("Access-Control-Allow-Origin", origin);
            }
        }

    }
}
