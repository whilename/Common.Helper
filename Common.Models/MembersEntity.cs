using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobile.Models
{
    /// <summary></summary>
    public class MembersEntity
    {
        /// <summary></summary>
        public int MembersId { get; set; }
        /// <summary></summary>
        public int UserId { get; set; }
        /// <summary></summary>
        public string OpenId { get; set; }
        /// <summary></summary>
        public string NickName { get; set; }
        /// <summary></summary>
        public string HeadImgUrl { get; set; }
        /// <summary></summary>
        public DateTime Birthday { get; set; }
        /// <summary></summary>
        public int Sex { get; set; }
        /// <summary></summary>
        public string Country { get; set; }
        /// <summary></summary>
        public string Province { get; set; }
        /// <summary></summary>
        public string City { get; set; }
        /// <summary></summary>
        public string District { get; set; }
        /// <summary></summary>
        public DateTime Created { get; set; }
        /// <summary></summary>
        public DateTime Updated { get; set; }
        /// <summary></summary>
        public bool Deleted { get; set; }
    }
}
