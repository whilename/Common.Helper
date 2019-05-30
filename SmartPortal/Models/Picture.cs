using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SmartPortal.Models
{
    /// <summary></summary>
    public class Picture : BaseEntity
    {
        /// <summary></summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary></summary>
        [StringLength(255)]
        public string PictureSrc { get; set; }
        /// <summary>摘要描述</summary>
        [StringLength(255)]
        public string Summary { get; set; }
        /// <summary>类别</summary>
        public int Category { get; set; }
        /// <summary>排列序号</summary>
        public int SortNum { get; set; }
        /// <summary>是否启用</summary>
        public bool Enabled { get; set; }
        /// <summary></summary>
        public int ReferenceId { get; set; }
        /// <summary></summary>
        public string OpsName { get; set; }
    }
}