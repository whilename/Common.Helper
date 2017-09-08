using Mobile.Dao;
using Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobile.Bll
{
    /// <summary></summary>
    public class MembersBusiness
    {
        public MembersDao mdao;

        public MembersBusiness() { mdao = new MembersDao(); }

        /// <summary></summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Save(MembersEntity entity)
        {
            return entity.UserId <= 0 ? mdao.Insert(entity) : mdao.Update(entity);
        }

    }
}
