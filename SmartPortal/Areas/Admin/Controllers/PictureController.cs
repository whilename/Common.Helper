using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartPortal.Areas.Admin.Controllers
{
    /// <summary>
    /// 门户基础模块:图片管理
    /// </summary>
    [Filter.DefaultAuthorizationFilter]
    [Filter.DefaultLoggerActionFilter]
    [Filter.DefaultExceptionFilter]
    public class PictureController : BaseController
    {
        public ActionResult Index()
        {
            var data = from m in smartDB.Pictures
                       select m;

            ViewBag.QMenu = GetQuickMenu();
            return View(data);
        }

        [HttpGet]
        public ActionResult Add()
        {
            Models.Picture entity = new Models.Picture();
            entity.SortNum = 0;
            entity.Enabled = true;
            entity.OpsName = Request.Cookies["username"].ToString();

            ViewBag.QMenu = GetQuickMenu();
            return View(entity);
        }

        [HttpPost]
        public ActionResult Add(Models.Picture model)
        {
            ViewBag.QMenu = GetQuickMenu();
            if (Request.Files.Count <= 0 || Request.Files[0].ContentLength <= 0)
            {
                Response.Write("<script>alert('未检测到要上传的图片资源，请从新选择要上传的图片！')</script>");
                return View(model);
            }
            string path = "Resource\\" + DateTime.Now.ToString("yyyyMMdd");
            // 检查文件目录，如果不存在则创建
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + path))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + path);
            model.PictureSrc = path + "\\" + Request.Files[0].FileName;
            string extension = Path.GetExtension(model.PictureSrc);
            // 检查文件是否存在，如果存在则重命名
            if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + path + "\\" + model.PictureSrc))
                model.PictureSrc = model.PictureSrc.Replace(extension, "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension);
            // 保存图片文件
            Request.Files[0].SaveAs(AppDomain.CurrentDomain.BaseDirectory + model.PictureSrc);
            smartDB.Pictures.Add(model);

            ViewBag.ReferenceUrl = Url.Action("Index");
            ViewBag.Title = "上传提示";
            ViewBag.Content = "上传图片成功。";
            if (smartDB.SaveChanges() <= 0)
            {
                ViewBag.Content = "上传图片失败。";
                ViewBag.ReferenceUrl = Request.Url.AbsoluteUri;
            }

            return View("Alert");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var data = from m in smartDB.Pictures where m.Id == id select m;

            ViewBag.QMenu = GetQuickMenu();
            return View(data.First<Models.Picture>());
        }

        [HttpPost]
        public ActionResult Edit(Models.Picture model)
        {
            if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
            {
                string path = "Resource\\" + DateTime.Now.ToString("yyyyMMdd");
                // 检查文件目录，如果不存在则创建
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + path))
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + path);
                model.PictureSrc = path + "\\" + Request.Files[0].FileName;
                string extension = Path.GetExtension(model.PictureSrc);
                // 检查文件是否存在，如果存在则重命名
                if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + path + "\\" + model.PictureSrc))
                    model.PictureSrc = model.PictureSrc.Replace(extension, "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension);
                // 保存图片文件
                Request.Files[0].SaveAs(AppDomain.CurrentDomain.BaseDirectory + model.PictureSrc);
            }

            smartDB.Pictures.Attach(model);
            //更改缓存修改状态
            smartDB.Entry(model).State = EntityState.Modified;

            ViewBag.ReferenceUrl = Url.Action("Index");
            ViewBag.Title = "保存提示";
            ViewBag.Content = "保存编辑成功。";
            if (smartDB.SaveChanges() <= 0)
            {
                ViewBag.Content = "保存编辑失败。";
                ViewBag.ReferenceUrl = Request.Url.AbsoluteUri;
            }

            ViewBag.QMenu = GetQuickMenu();
            return View("Alert");
        }

        [HttpGet]
        public ActionResult Del(int id)
        {
            var data = from m in smartDB.Pictures where m.Id == id select m;

            smartDB.Pictures.Remove(data.First<Models.Picture>());

            ViewBag.ReferenceUrl = Url.Action("Index");
            ViewBag.Title = "删除提示";
            ViewBag.Content = "删除图片成功。";
            if (smartDB.SaveChanges() <= 0)
            {
                ViewBag.Content = "删除图片失败。";
                ViewBag.ReferenceUrl = Request.Url.AbsoluteUri;
            }

            ViewBag.QMenu = GetQuickMenu();
            return View("Alert");
        }

        private List<Models.Custom.QuickMenu> GetQuickMenu()
        {
            ViewBag.Fun = "图片管理";
            ViewBag.Tip = new Models.Custom.Tips() { Title = "提示信息", Content = "这里描述一段实用的便捷描述文字信息。", LinkText = "www.runoob.com", Link = "www.runoob.com" };
            return new List<Models.Custom.QuickMenu>()
            {
                new Models.Custom.QuickMenu(){ Icon="~/Content/img/style.png", Link= Url.Action("Index"), LinkText="图片列表", Description="图片资源信息。"},
                new Models.Custom.QuickMenu(){ Icon="~/Content/img/style.png", Link= Url.Action("Add"), LinkText="上传图片", Description="上传一张图片。"}
            };
        }
    }
}
