using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobileApp.Models
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
        public string Privilege { get; set; }
        /// <summary></summary>
        public string UnionId { get; set; }
        /// <summary></summary>
        public string Language { get; set; }
        /// <summary></summary>
        public int Subscribe { get; set; }
        /// <summary></summary>	
        public long SubscribeTime { get; set; }
        /// <summary></summary>
        public string Remark { get; set; }
        /// <summary></summary>	
        public int GroupId { get; set; }
        /// <summary></summary>
        public string TagIdList { get; set; }

        /// <summary></summary>
        public DateTime Created { get; set; }
        /// <summary></summary>
        public DateTime Updated { get; set; }
        /// <summary></summary>
        public bool Deleted { get; set; }

    }
}
