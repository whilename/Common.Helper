using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NancyApp
{
    /// <summary></summary>
    public class MobileModule : BaseApiModule
    {
        /// <summary></summary>
        public MobileModule()
        {
            Get["/gshow"] = x => { return "Input Mobile: " + Params.mobile; };// Request.Query.mobile;

            Get["/qshow"] = x => { return string.Format("<H1>Input Mobile: {0}</H1>", Request.Query.mobile); };
        }
    }
}