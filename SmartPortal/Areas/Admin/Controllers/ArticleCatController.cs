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
    /// 门户基础模块:文章类目管理
    /// </summary>
    [Filter.DefaultAuthorizationFilter]
    [Filter.DefaultLoggerActionFilter]
    [Filter.DefaultExceptionFilter]
    public class ArticleCatController : BaseController
    {
        public ActionResult Index()
        {
            var data = from m in smartDB.ArticleCats
                       select m;

            return View(data);
        }

        [HttpGet]
        public ActionResult Add()
        {
            Models.ArticleCat entity = new Models.ArticleCat();

            entity.Status = "normal";
            entity.IsParent = true;

            return View(entity);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Models.ArticleCat entity)
        {
            smartDB.ArticleCats.Add(entity);
            
            ViewBag.ReferenceUrl = Url.Action("Index");
            ViewBag.Title = "添加类目提示";
            ViewBag.Content = "添加类目成功";
            if (smartDB.SaveChanges() <= 0)
            {
                ViewBag.Content = "添加类目失败。";
                ViewBag.ReferenceUrl = Request.Url.AbsoluteUri;
            }

            ViewBag.QMenu = GetQuickMenu();
            return View("Alert");
        }

        [HttpGet]
        public ActionResult Edit(int cid)
        {
            var data = from m in smartDB.ArticleCats
                       where m.Cid == cid
                       select m;

            return View(data.First<Models.ArticleCat>());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Models.ArticleCat entity)
        {
            smartDB.ArticleCats.Attach(entity);
            smartDB.Entry(entity).State = EntityState.Modified;
            
            ViewBag.ReferenceUrl = Url.Action("Index");
            ViewBag.Title = "编辑类目提示";
            ViewBag.Content = "编辑类目成功";
            if (smartDB.SaveChanges() <= 0)
            {
                ViewBag.Content = "编辑类目失败。";
                ViewBag.ReferenceUrl = Request.Url.AbsoluteUri;
            }

            ViewBag.QMenu = GetQuickMenu();
            return View("Alert");
        }

        [HttpGet]
        public ActionResult Del(int cid)
        {
            var data = from m in smartDB.ArticleCats
                       where m.Cid == cid
                       select m;

            smartDB.Entry(data.First<Models.ArticleCat>()).State = EntityState.Deleted;
            
            ViewBag.ReferenceUrl = Url.Action("Index");
            ViewBag.Title = "删除类目提示";
            ViewBag.Content = "删除类目成功";
            if (smartDB.SaveChanges() <= 0)
            {
                ViewBag.Content = "删除类目失败。";
                ViewBag.ReferenceUrl = Request.Url.AbsoluteUri;
            }

            ViewBag.QMenu = GetQuickMenu();
            return View("Alert");
        }
        
        private List<Models.Custom.QuickMenu> GetQuickMenu()
        {
            ViewBag.Fun = "类目管理";
            ViewBag.Tip = new Models.Custom.Tips() { Title = "提示信息", Content = "这里描述一段实用的便捷描述文字信息。", LinkText = "www.runoob.com", Link = "www.runoob.com" };
            return new List<Models.Custom.QuickMenu>()
            {
                new Models.Custom.QuickMenu(){ Icon="~/Content/img/style.png", Link= Url.Action("Index"), LinkText="类目列表", Description="点击链接查看文章类目列表。"},
                new Models.Custom.QuickMenu(){ Icon="~/Content/img/style.png", Link= Url.Action("Add"), LinkText="添加类目", Description="点击链接添加一个类目。"}
            };
        }
    }
}
