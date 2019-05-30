using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobileApp.Models
{
    /// <summary></summary>
    public class UsersEntity
    {
        /// <summary>唯一标识</summary>
        public int UserId { get; set; }
        /// <summary>用户编号</summary>
        public string UserNo { get; set; }
        /// <summary>登录名</summary>
        public string UserName { get; set; }
        /// <summary>User</summary>
        public string UserMobile { get; set; }
        /// <summary>登录邮箱</summary>
        public string UserMail { get; set; }
        /// <summary>登录密码</summary>
        public string UserPwd { get; set; }
        /// <summary>创建时间</summary>
        public DateTime Created { get; set; }
        /// <summary>更新时间</summary>
        public DateTime Updated { get; set; }
        /// <summary>是否删除</summary>
        public bool Deleted { get; set; }

    }
}
