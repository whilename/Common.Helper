using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using System.Data.Entity;

namespace SmartPortal.Areas.Admin.Controllers
{
    
    /// <summary>
    /// 用户管理
    /// 网站管理员、职员、会员账户管理。
    /// </summary>
    [Filter.DefaultAuthorizationFilter]
    [Filter.DefaultLoggerActionFilter]
    [Filter.DefaultExceptionFilter]
    public class UserController : BaseController
    {
        public ActionResult Index()
        {
            var data = from m in smartDB.Users
                       select m;

            ViewBag.QMenu = GetQuickMenu();
            return View(data);
        }

        [HttpGet]
        public ActionResult Add()
        {
            Models.User entity = new Models.User();
            entity.CurrentIPAddress = this.Request.UserHostAddress;
            entity.EmailAddress = "";
            entity.EndLoginDate = DateTime.Now;
            entity.MutileOnLine = false;
            entity.Nick = "";
            entity.OnLine = false;
            entity.Password = "";
            entity.RegisterDate = DateTime.Now;
            entity.UserName = "";
            entity.UserRole = "";

            ViewBag.QMenu = GetQuickMenu();
            return View(entity);
        }

        [HttpPost]
        public ActionResult Add(Models.User model)
        {
            //解决Checkbox无法获取值问题
            model.OnLine = false;
            if (Request.Form["OnLine"].ToString().ToLower() == "true")
            {
                model.OnLine = true;
            }
            model.MutileOnLine = false;
            if (Request.Form["MutileOnLine"].ToString().ToLower() == "true")
            {
                model.MutileOnLine = true;
            }

            smartDB.Users.Add(model);

            ViewBag.ReferenceUrl = Url.Action("Index");
            ViewBag.Title = "添加用户提示";
            ViewBag.Content = "添加用户成功。";
            if (smartDB.SaveChanges() <= 0)
            {
                ViewBag.Content = "添加用户失败。";
                ViewBag.ReferenceUrl = Request.Url.AbsoluteUri;
            }

            ViewBag.QMenu = GetQuickMenu();
            return View("Alert");
        }

        [HttpGet]
        public ActionResult Edit(int uid)
        {
            var data = from m in smartDB.Users
                       where m.UId == uid
                       select m;

            ViewBag.QMenu = GetQuickMenu();
            return View(data.First<Models.User>());
        }

        [HttpPost]
        public ActionResult Edit(Models.User model)
        {
            //解决checkbox值获取不到的问题
            model.OnLine = true;
            if (Request.Form["OnLine"] == null)
            {
                model.OnLine = false;
            }

            model.MutileOnLine = true;
            if (Request.Form["MutileOnLine"] == null)
            {
                model.MutileOnLine = false;
            }

            smartDB.Users.Attach(model);

            //更改缓存修改状态
            smartDB.Entry(model).State = EntityState.Modified;
            
            ViewBag.ReferenceUrl = Url.Action("Index");
            ViewBag.Title = "保存用户提示";
            ViewBag.Content = "保存用户成功。";
            if (smartDB.SaveChanges() <= 0)
            {
                ViewBag.Content = "保存用户失败。";
                ViewBag.ReferenceUrl = Request.Url.AbsoluteUri;
            }

            ViewBag.QMenu = GetQuickMenu();
            return View("Alert");
        }

        [HttpGet]
        public ActionResult Del(int uid)
        {
            var data = from m in smartDB.Users
                       where m.UId == uid
                       select m;

            smartDB.Users.Remove(data.First<Models.User>());
            
            ViewBag.ReferenceUrl = Url.Action("Index");
            ViewBag.Title = "删除用户提示";
            ViewBag.Content = "删除用户成功。";
            if (smartDB.SaveChanges() <= 0)
            {
                ViewBag.Content = "删除用户失败。";
                ViewBag.ReferenceUrl = Request.Url.AbsoluteUri;
            }

            ViewBag.QMenu = GetQuickMenu();
            return View("Alert");
        }

        private List<Models.Custom.QuickMenu> GetQuickMenu()
        {
            ViewBag.Fun = "用户管理";
            ViewBag.Tip = new Models.Custom.Tips() { Title = "提示信息", Content = "这里描述一段实用的便捷描述文字信息。", LinkText = "www.runoob.com", Link = "www.runoob.com" };
            return new List<Models.Custom.QuickMenu>()
            {
                new Models.Custom.QuickMenu(){ Icon="~/Content/img/style.png", Link= Url.Action("Index"), LinkText="用户列表", Description="管理用户信息。"},
                new Models.Custom.QuickMenu(){ Icon="~/Content/img/style.png", Link= Url.Action("Add"), LinkText="添加用户", Description="添加一个用户。"}
            };
        }
    }
}
