using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eQuartz.Models
{
    [Table("EQ_Log")]
    public class LogEntity: BaseEntity
    {
        /// <summary></summary>
        [Key]
        public int LogId { get; set; }
        /// <summary></summary>
        public int JobId { get; set; }
        /// <summary></summary>
        public string Content { get; set; }
    }
}