using MvcApp.Areas.WebApi.Models;
using MvcApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MvcApp.Areas.WebApi.Controllers
{
    /// <summary></summary>
    public class MobileController : ApiController
    {
        /// <summary></summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [HttpGet, ActionName("gshow"), ApiFilter]
        public string get(string mobile)
        {
            return "您提交的手机号是:" + mobile;
        }

        /// <summary></summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost, ActionName("pshow"), ApiFilter]
        public string post([FromBody]MobileModel obj)
        {
            return "您提交的手机号是:" + obj.mobile;
        }
    }
}
