using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eQuartz.Models
{
    [Table("EQ_Job")]
    public class JobEntity: BaseEntity
    {
        /// <summary></summary>
        [Key]
        public int JobId { get; set; }
        /// <summary></summary>
        public string JobName { get; set; }
        /// <summary></summary>
        public string RunTime { get; set; }
        /// <summary></summary>
        public int JobType { get; set; }
        /// <summary></summary>
        public string Content { get; set; }
        /// <summary></summary>
        public bool IsEnabled { get; set; }
    }
}