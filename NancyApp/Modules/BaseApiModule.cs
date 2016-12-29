using Nancy;
using Nancy.ModelBinding;
using NancyApp.Models;
using System;

namespace NancyApp
{
    /// <summary> API 请求路由基类</summary>
    public class BaseApiModule : NancyModule
    {
        private Params _params;
        /// <summary>请求参数对象</summary>
        public Params Params { get { return this._params; } }

        /// <summary> API 请求路由基类</summary>
        public BaseApiModule()
            : base("api")
        {
            Before += ValidBefore;

        }

        /// <summary> API 请求路由基类</summary>
        /// <param name="path">二级目录</param>
        public BaseApiModule(string path)
            : base("api/" + path.TrimStart('/'))
        {
            Before += ValidBefore;

        }

        /// <summary>进入请求方法前验证</summary>
        /// <param name="context">请求内容</param>
        /// <returns></returns>
        public Response ValidBefore(NancyContext context)
        {
            if (context.Request.Method.Equals("options", StringComparison.CurrentCultureIgnoreCase)) 
                return new Response() { StatusCode = HttpStatusCode.OK };

            this._params = this.Bind<Params>();
            return null;
        }
    }

}
