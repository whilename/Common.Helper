using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eQuartz.Models
{
    /// <summary></summary>
    [Table("EQ_Email")]
    public class EmailEntity : BaseEntity
    {
        /// <summary></summary>
        [Key]
        public int EmailId { get; set; }
        /// <summary></summary>
        [Required]
        [DataType(DataType.MultilineText)]
        [RegularExpression(@"^(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+([;.](([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+)*$", ErrorMessage = "Invalid Email List")]
        public string MailTo { get; set; }
        /// <summary></summary>
        public string Subject { get; set; }
        /// <summary></summary>
        public string MailBody { get; set; }
        /// <summary></summary>
        public string Comment { get; set; }

    }
}