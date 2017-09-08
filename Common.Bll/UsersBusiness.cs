using Mobile.Dao;
using Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobile.Bll
{
    /// <summary></summary>
    public class UsersBusiness
    {
        private UsersDao udao;

        public UsersBusiness() { udao = new UsersDao(); }

        /// <summary></summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Save(UsersEntity entity)
        {
            return entity.UserId <= 0 ? udao.Insert(entity) : udao.Update(entity);
        }

    }
}
