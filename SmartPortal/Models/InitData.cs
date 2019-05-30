using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SmartPortal.Models
{
    /// <summary>
    /// 初始化数据
    /// </summary>
    public class InitData : DropCreateDatabaseIfModelChanges<SmartDB>
    {
        protected override void Seed(SmartDB context)
        {
            // 初始化前台菜单
            new List<Menu> {
                new Menu{ MenuName = "网站首页" , MenuLink = "~/Home/Index", MenuIcon = "Index", ParentID = 0, Category = 1, SortNum = 1, Enabled = true },
                new Menu{ MenuName = "公司简介" , MenuLink = "~/Home/CompanyProfile", MenuIcon = "CompanyProfile", ParentID = 0, Category = 1, SortNum = 2, Enabled = false },
                new Menu{ MenuName = "产品展示" , MenuLink = "~/Home/Products", MenuIcon = "Products", ParentID = 0, Category = 1, SortNum = 3, Enabled = false },
                new Menu{ MenuName = "新闻资讯" , MenuLink = "~/Home/News", MenuIcon = "News", ParentID = 0, Category = 1, SortNum = 4, Enabled = true },
                new Menu{ MenuName = "产品知识" , MenuLink = "~/Home/ProductsLore", MenuIcon = "ProductsLore", ParentID = 0, Category = 1, SortNum = 5, Enabled = false },
                new Menu{ MenuName = "销售网络" , MenuLink = "~/Home/SalesNetwork", MenuIcon = "SalesNetwork", ParentID = 0, Category = 1, SortNum = 6, Enabled = true },
                new Menu{ MenuName = "服务宗旨" , MenuLink = "~/Home/Service", MenuIcon = "Service", ParentID = 0, Category = 1, SortNum = 7, Enabled = false },
                new Menu{ MenuName = "联系我们" , MenuLink = "~/Home/Contact", MenuIcon = "Contact", ParentID = 0, Category = 1, SortNum = 8, Enabled = true },

                new Menu{ MenuName = "文章管理" , MenuLink = "~/Admin/Article", MenuIcon = "Article", ParentID = 0, Category = 0, SortNum = 1, Enabled = true },
                new Menu{ MenuName = "产品管理" , MenuLink = "~/Admin/Product", MenuIcon = "Product", ParentID = 0, Category = 0, SortNum = 2, Enabled = false },
                new Menu{ MenuName = "图片管理" , MenuLink = "~/Admin/Picture", MenuIcon = "Picture", ParentID = 0, Category = 0, SortNum = 3, Enabled = false },
                new Menu{ MenuName = "附件管理" , MenuLink = "~/Admin/Attachment", MenuIcon = "Attachment", ParentID = 0, Category = 0, SortNum = 4, Enabled = false },
                new Menu{ MenuName = "留言管理" , MenuLink = "~/Admin/LeaveMessage", MenuIcon = "LeaveMessage", ParentID = 0, Category = 0, SortNum = 5, Enabled = true },
                new Menu{ MenuName = "联系方式" , MenuLink = "~/Admin/Contact", MenuIcon = "Contact", ParentID = 0, Category = 0, SortNum = 6, Enabled = true },
                new Menu{ MenuName = "友情链接" , MenuLink = "~/Admin/Link", MenuIcon = "Link", ParentID = 0, Category = 0, SortNum = 7, Enabled = true },
                new Menu{ MenuName = "系统管理" , MenuLink = "~/Admin/System", MenuIcon = "System", ParentID = 0, Category = 0, SortNum = 8, Enabled = true },

                new Menu{ MenuName = "网站设置" , MenuLink = "~/Admin/Configuration", MenuIcon = "Configuration", ParentID = 16, Category = 0, SortNum = 1, Enabled = true },
                new Menu{ MenuName = "菜单管理" , MenuLink = "~/Admin/Menu", MenuIcon = "Menu", ParentID = 16, Category = 0, SortNum = 2, Enabled = true },
                new Menu{ MenuName = "用户管理" , MenuLink = "~/Admin/User", MenuIcon = "User", ParentID = 16, Category = 0, SortNum = 3, Enabled = true },
                new Menu{ MenuName = "图片管理" , MenuLink = "~/Admin/Picture", MenuIcon = "Picture", ParentID = 16, Category = 0, SortNum = 4, Enabled = true },
                new Menu{ MenuName = "访客分析" , MenuLink = "~/Admin/Analysis", MenuIcon = "Analysis", ParentID = 16, Category = 0, SortNum = 5, Enabled = true },
                new Menu{ MenuName = "安全设置" , MenuLink = "~/Admin/Security", MenuIcon = "Security", ParentID = 16, Category = 0, SortNum = 6, Enabled = true },
                new Menu{ MenuName = "开放平台" , MenuLink = "~/Admin/Interface", MenuIcon = "Interface", ParentID = 16, Category = 0, SortNum = 7, Enabled = true },
                new Menu{ MenuName = "更新维护" , MenuLink = "~/Admin/Update", MenuIcon = "Update", ParentID = 16, Category = 0, SortNum = 8, Enabled = true },
                new Menu{ MenuName = "备份恢复" , MenuLink = "~/Admin/Backup", MenuIcon = "Backup", ParentID = 16, Category = 0, SortNum = 9, Enabled = true },
                new Menu{ MenuName = "日志管理" , MenuLink = "~/Admin/Logger", MenuIcon = "Logger", ParentID = 16, Category = 0, SortNum = 10, Enabled = true }
            }.ForEach(m=> context.Menus.Add(m));

            // 初始化用户信息数据
            new List<User> { new User() { UserName = "admin", Password = "admin", Nick = "超级管理员", UserRole = "超级管理员", EmailAddress = "", EndLoginDate = DateTime.Now, MutileOnLine = false,  OnLine = false, RegisterDate = DateTime.Now , CurrentIPAddress = "127.0.0.1" } }
            .ForEach(m => context.Users.Add(m));

            context.SaveChanges();
            base.Seed(context);
        }
    }
}