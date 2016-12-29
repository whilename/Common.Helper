using Common.Utility;
using Nancy;
using Nancy.IO;
using Nancy.Extensions;
using Nancy.ErrorHandling;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace NancyApp
{
    /// <summary>状态码处理</summary>
    public class StatusCodeHandler : IStatusCodeHandler
    {
        private readonly HttpStatusCode[] supportedStatusCodes = new[] { HttpStatusCode.NotFound, HttpStatusCode.InternalServerError };

        /// <summary></summary>
        /// <param name="statusCode"></param>
        /// <param name="context"></param>
        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            if (context.Response != null && context.Response.Contents != null && !ReferenceEquals(context.Response.Contents, Response.NoBody)) { return; }
            if (context.Response == null)
            {
                context.Response = new Response() { StatusCode = HttpStatusCode.OK };
            }
            context.Response.WithContentType("application/json");
            context.Response.Contents = s =>
            {
                using (var writer = new StreamWriter(new UnclosableStreamWrapper(s), Encoding.UTF8))
                {
                    var msg = StaticConfiguration.DisableErrorTraces ? "" : context.GetExceptionDetails();
                    Log.Error(msg);
                    writer.Write(Utils.JsonSerialize(new
                    {
                        data = new { },
                        code = "1",
                        msg = "网络不给力啊！"
                    }));
                }
            };
        }

        /// <summary>重写处理状态码</summary>
        /// <param name="statusCode"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return this.supportedStatusCodes.Any(s => s == statusCode);
        }
    }
}