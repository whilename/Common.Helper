using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobile.Models
{
    /// <summary></summary>
    public class UsersEntity
    {
        /// <summary></summary>
        public int UserId { get; set; }
        /// <summary></summary>
        public string UserName { get; set; }
        /// <summary></summary>
        public string UserMobile { get; set; }
        /// <summary></summary>
        public string UserMail { get; set; }
        /// <summary></summary>
        public string UserPwd { get; set; }
        /// <summary></summary>
        public DateTime Created { get; set; }
        /// <summary></summary>
        public DateTime Updated { get; set; }
        /// <summary></summary>
        public bool Deleted { get; set; }

    }
}
