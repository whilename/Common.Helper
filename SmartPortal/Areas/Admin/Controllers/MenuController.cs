using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartPortal.Areas.Admin.Controllers
{
    [Filter.DefaultLoggerActionFilter]
    [Filter.DefaultExceptionFilter]
    public class MenuController : BaseController
    {
        [HttpGet]
        [Filter.DefaultAuthorizationFilter]
        public ActionResult Index()
        {
            var data = from m in smartDB.Menus where m.Category == 1 orderby m.ID ascending select m;

            ViewBag.QMenu = GetQuickMenu();
            return View(data);
        }

        [HttpGet]
        public ActionResult Add()
        {
            Models.Menu entity = new Models.Menu();
            entity.MenuName = "菜单名字";
            entity.MenuLink = "链接地址";
            entity.MenuIcon = "图标";
            entity.Description = "描述简介";
            entity.Category = 0;
            entity.SortNum = 0;

            ViewBag.QMenu = GetQuickMenu();
            return View(entity);
        }

        [HttpGet]
        public ActionResult Copy(int id)
        {
            Models.Menu entity = (from m in smartDB.Menus where m.ID == id select m).First();
            entity.Created = DateTime.Now;
            smartDB.Menus.Add(entity);
            smartDB.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Add(Models.Menu entity)
        {
            smartDB.Menus.Add(entity);

            ViewBag.ReferenceUrl = Url.Action("Index");
            ViewBag.Title = "添加菜单提示";
            ViewBag.Content = "添加菜单成功";
            if (smartDB.SaveChanges() <= 0)
            {
                ViewBag.Content = "添加菜单失败。";
                ViewBag.ReferenceUrl = Request.Url.AbsoluteUri;
            }

            ViewBag.QMenu = GetQuickMenu();
            return View("Alert");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var data = from m in smartDB.Menus where m.ID == id select m;

            ViewBag.QMenu = GetQuickMenu();
            return View(data.First<Models.Menu>());
        }

        [HttpPost]
        public ActionResult Edit(Models.Menu entity)
        {
            entity.Updated = DateTime.Now;
            smartDB.Menus.Attach(entity);
            smartDB.Entry(entity).State = EntityState.Modified;

            ViewBag.ReferenceUrl = Url.Action("Index");
            ViewBag.Title = "编辑菜单提示";
            ViewBag.Content = "编辑菜单成功";
            if (smartDB.SaveChanges() <= 0)
            {
                ViewBag.Content = "编辑菜单失败。";
                ViewBag.ReferenceUrl = Request.Url.AbsoluteUri;
            }

            ViewBag.QMenu = GetQuickMenu();
            return View("Alert");
        }

        [HttpGet]
        public ActionResult Del(int id)
        {
            var data = from m in smartDB.Menus where m.ID == id select m;

            smartDB.Entry(data.First<Models.Menu>()).State = EntityState.Deleted;

            ViewBag.ReferenceUrl = Url.Action("Index");
            ViewBag.Title = "删除菜单提示";
            ViewBag.Content = "删除菜单成功";
            if (smartDB.SaveChanges() <= 0)
            {
                ViewBag.Content = "删除菜单失败。";
                ViewBag.ReferenceUrl = Request.Url.AbsoluteUri;
            }

            ViewBag.QMenu = GetQuickMenu();
            return View("Alert");
        }

        private List<Models.Custom.QuickMenu> GetQuickMenu()
        {
            ViewBag.Fun = "菜单管理";
            ViewBag.Tip = new Models.Custom.Tips() { Title = "提示信息", Content = "这里描述一段实用的便捷描述文字信息。", LinkText = "www.runoob.com", Link = "www.runoob.com" };
            return new List<Models.Custom.QuickMenu>()
            {
                new Models.Custom.QuickMenu(){ Icon="~/Content/img/style.png", Link= Url.Action("Index"), LinkText="菜单列表", Description="管理已发布的菜单。"},
                new Models.Custom.QuickMenu(){ Icon="~/Content/img/style.png", Link= Url.Action("Add"), LinkText="添加菜单", Description="添加一个菜单。"}
            };
        }
    }
}