﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartPortal.Areas.Admin.Controllers
{
    /// <summary>
    /// 门户基础模块:附件管理
    /// </summary>
    [Filter.DefaultAuthorizationFilter]
    [Filter.DefaultLoggerActionFilter]
    [Filter.DefaultExceptionFilter]
    public class AttachmentController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
