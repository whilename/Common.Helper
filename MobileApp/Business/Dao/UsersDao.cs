using Common.AdoNet;
using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobileApp.Dao
{
    /// <summary></summary>
    public class UsersDao : SqlMapper<UsersEntity>
    {
        /// <summary></summary>
        public UsersDao() : base("Account") { }

        /// <summary></summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert(UsersEntity entity)
        {
            string sqlstr = "INSERT INTO Account(UserName,UserMobile,UserMail,UserPwd) VALUES(@UserName,@UserMobile,@UserMail,@UserPwd);SELECT @@IDENTITY;";
            entity.UserId = this.Execute<int>(sqlstr, entity);
            return entity.UserId > 0;
        }

        /// <summary></summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(UsersEntity entity)
        {
            string sqlstr = "UPDATE Account SET UserName=@UserName,UserMobile=@UserMobile,UserMail=@UserMail,UserPwd=@UserPwd,Updated=GETDATE() WHERE UserId=@UserId;";
            int upd = this.Execute(sqlstr, entity);
            return upd > 0;
        }

    }
}
