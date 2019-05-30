using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eQuartz.Models
{
    /// <summary></summary>
    [Table("EQ_Dict")]
    public class DictEntity : BaseEntity
    {
        /// <summary></summary>
        [Key]
        public int DictId { get; set; }
        public int DictType { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int ParentId { get; set; }
        public string Tag { get; set; }

    }
}