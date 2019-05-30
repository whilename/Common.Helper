using Common.AdoNet;
using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobileApp.Dao
{
    /// <summary></summary>
    public class MembersDao : SqlMapper<MembersEntity>
    {
        /// <summary></summary>
        public MembersDao() : base("Members") { }

        /// <summary></summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert(MembersEntity entity)
        {
            string sqlstr = @"INSERT INTO Members(UserId,OpenId,NickName,HeadImgUrl,Birthday,Sex,Country,Province,City,District) 
    VALUES(@UserId,@OpenId,@NickName,@HeadImgUrl,@Birthday,@Sex,@Country,@Province,@City,@District);SELECT @@IDENTITY;";
            entity.MembersId = this.Execute<int>(sqlstr, entity);
            return entity.MembersId > 0;
        }

        /// <summary></summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(MembersEntity entity)
        {
            string sqlstr = @"UPDATE Members SET UserId=@UserId,OpenId=@OpenId,NickName=@NickName,HeadImgUrl=@HeadImgUrl,Birthday=@Birthday,
Sex=@Sex,Country=@Country,Province=@Province,City=@City,District=@District,Updated=GETDATE() WHERE MemberId=@MemberId;";
            int upd = this.Execute<int>(sqlstr, entity);
            return upd > 0;
        }

    }
}
