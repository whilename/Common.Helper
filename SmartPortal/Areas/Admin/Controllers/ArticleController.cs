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
    /// 门户基础模块:文章管理
    /// </summary>
    [Filter.DefaultAuthorizationFilter]
    [Filter.DefaultLoggerActionFilter]
    [Filter.DefaultExceptionFilter]
    public class ArticleController : BaseController
    {
        public ActionResult Index()
        {
            var data = from m in smartDB.Articles
                       orderby m.PublishTime descending
                       select m;

            ViewBag.QMenu = GetQuickMenu();
            return View(data);
        }

        [HttpGet]
        public ActionResult Add()
        {
            Models.Article entity = new Models.Article();

            entity.PublishTime = DateTime.Now;

            ViewBag.QMenu = GetQuickMenu();
            return View(entity);
        }

        [HttpGet]
        public ActionResult Copy(int id)
        {
            Models.Article entity = (from m in smartDB.Articles
                                     where m.ID == id
                                     select m).First();
            entity.PublishTime = DateTime.Now;
            smartDB.Articles.Add(entity);
            smartDB.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Models.Article entity)
        {
            smartDB.Articles.Add(entity);
            
            ViewBag.ReferenceUrl = Url.Action("Index");
            ViewBag.Title = "添加文章提示";
            ViewBag.Content = "添加文章成功";
            if (smartDB.SaveChanges() <= 0)
            {
                ViewBag.Content = "添加文章失败。";
                ViewBag.ReferenceUrl = Request.Url.AbsoluteUri;
            }

            ViewBag.QMenu = GetQuickMenu();
            return View("Alert");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var data = from m in smartDB.Articles
                       where m.ID == id
                       select m;

            ViewBag.QMenu = GetQuickMenu();
            return View(data.First<Models.Article>());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Models.Article entity)
        {
            smartDB.Articles.Attach(entity);
            smartDB.Entry(entity).State = EntityState.Modified;
            
            ViewBag.ReferenceUrl = Url.Action("Index");
            ViewBag.Title = "编辑文章提示";
            ViewBag.Content = "编辑文章成功";
            if (smartDB.SaveChanges() <= 0)
            {
                ViewBag.Content = "编辑文章失败。";
                ViewBag.ReferenceUrl = Request.Url.AbsoluteUri;
            }

            ViewBag.QMenu = GetQuickMenu();
            return View("Alert");
        }

        [HttpGet]
        public ActionResult Del(int id)
        {
            var data = from m in smartDB.Articles
                       where m.ID == id
                       select m;

            smartDB.Entry(data.First<Models.Article>()).State = EntityState.Deleted;

            ViewBag.ReferenceUrl = Url.Action("Index");
            ViewBag.Title = "删除文章提示";
            ViewBag.Content = "删除文章成功";
            if (smartDB.SaveChanges() <= 0)
            {
                ViewBag.Content = "删除文章失败。";
                ViewBag.ReferenceUrl = Request.Url.AbsoluteUri;
            }

            ViewBag.QMenu = GetQuickMenu();
            return View("Alert");
        }

        private List<Models.Custom.QuickMenu> GetQuickMenu()
        {
            ViewBag.Fun = "文章管理";
            ViewBag.Tip = new Models.Custom.Tips() { Title = "提示信息", Content = "这里描述一段实用的便捷描述文字信息。", LinkText = "www.runoob.com", Link = "www.runoob.com" };
            return new List<Models.Custom.QuickMenu>()
            {
                new Models.Custom.QuickMenu(){ Icon="~/Content/img/style.png", Link= Url.Action("Index"), LinkText="文章列表", Description="管理已发布的文章。"},
                new Models.Custom.QuickMenu(){ Icon="~/Content/img/style.png", Link= Url.Action("Add"), LinkText="添加文章", Description="添加一篇文章。"}
            };
        }
    }
}