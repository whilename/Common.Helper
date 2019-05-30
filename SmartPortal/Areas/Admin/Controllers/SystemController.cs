using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;


namespace SmartPortal.Areas.Admin.Controllers
{
    /// <summary>
    /// 系统设置
    /// 提供系统安装、维护等功能。
    /// </summary>
    [Filter.DefaultLoggerActionFilter]
    [Filter.DefaultExceptionFilter]
    public class SystemController : BaseController
    {
        /// <summary>
        /// 默认页面，如果系统未安装跳转到登录页面；如果用户未登录，跳转到登录页面。
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Filter.DefaultAuthorizationFilter]
        public ActionResult Index()
        {
            ViewBag.QMenu = GetQuickMenu();
            return View();
        }

        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">登录密码</param>
        /// <param name="code">验证码</param>
        /// <param name="remember">保存用户名到Cookie中</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(string username, string password, string code, string remember)
        {
            //校验验证码
            if (Session["validatecode"] == null || Session["validatecode"].ToString() != code)
            {
                //校验失败,返回登录页面
                ViewBag.Message = "验证码错误。";
                return View("Login");
            }

            //校验用户名和密码
            var data = from m in smartDB.Users
                       where m.UserName == username && m.Password == password
                       select m;

            //用户不存在
            if (data.Count<Models.User>() <= 0)
            {
                ViewBag.Message = "用户名或密码错误，请重新登录";
                return View("Login");
            }

            //一周内免登陆设置
            if (remember == "1")
            {
                Response.Cookies.Add(new HttpCookie("username", username) { Expires = DateTime.Now.AddDays(7) });
            }
            else
            {
                Response.Cookies.Add(new HttpCookie("username", username));
            }

            //登录成功后,跳转到后台默认主页
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        [HttpGet]
        [Filter.DefaultAuthorizationFilter]
        public ActionResult LoginOut(string username)
        {
            Response.Cookies.Add(new HttpCookie("username", ""));

            return RedirectToAction("Login");
        }

        /// <summary>
        /// 获取一张验证码图片
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public void VerificationCode()
        {
            //构造验证码图片
            Smart.Utility.ValidateCode validateCode = new Smart.Utility.ValidateCode();
            validateCode.Border = true;
            validateCode.BorderColor = Color.Red;
            validateCode.BorderWidth = 1;
            validateCode.Chaos = true;
            validateCode.Colors = new Color[] { Color.Red, Color.Gray, Color.Blue, Color.Beige };
            validateCode.FontSize = 13;
            validateCode.Length = 4;
            validateCode.Padding = 1;

            //生成随机数字
            Random rd = new Random();
            string codeText = string.Format("{0}{1}{2}{3}", rd.Next(0, 9).ToString(), rd.Next(0, 9).ToString(), rd.Next(0, 9).ToString(), rd.Next(0, 9).ToString());

            //验证码文本加入Session
            this.Session["validatecode"] = codeText;

            //向客户端返回一张验证码图片
            Bitmap image = validateCode.CreateImageCode(codeText);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            this.Response.ClearContent();
            this.Response.ContentType = "image/png";
            this.Response.BinaryWrite(ms.GetBuffer());
            ms.Close();
            ms = null;
            image.Dispose();
            image = null;
        }
        #region 
        /// <summary>
        /// 安装系统
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Install()
        {
            return View();
        }

        /// <summary>
        /// 安装数据库
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult InstallDataBase()
        {
            return View();
        }

        /// <summary>
        /// 退出系统安装，进入反馈页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NoThanks()
        {
            return View();
        }
        #endregion

        /// <summary></summary>
        /// <returns></returns>
        private List<Models.Custom.QuickMenu> GetQuickMenu()
        {
            ViewBag.Fun = "系统管理";
            ViewBag.Tip = new Models.Custom.Tips() { Title = "提示信息", Content = "这里描述一段实用的便捷描述文字信息。", LinkText = "www.runoob.com", Link = "www.runoob.com" };
            return new List<Models.Custom.QuickMenu>() { };
        }
    }
}
