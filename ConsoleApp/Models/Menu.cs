using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ConsoleApp.Models
{
    /// <summary>
    /// 菜单实体类定义
    /// </summary>
    [Table("SysMenu")]
    public class Menu 
    {
        /// <summary>
        /// 菜单编号
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(name: "ID", TypeName = "INTEGER")]
        public int ID { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        [StringLength(20)]
        public string MenuName { get; set; }

        /// <summary>
        /// 菜单地址
        /// </summary>
        [StringLength(255)]
        public string MenuLink { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        [StringLength(255)]
        public string MenuIcon { get; set; }

        /// <summary>
        /// 菜单描述
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// 父级菜单
        /// </summary>
        public int ParentID { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public int Category { get; set; }

        /// <summary>
        /// 排列序号
        /// </summary>
        public int SortNum { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary></summary>
        //[Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Created { get { return this._created; } set { _created = value; } }
        private DateTime _created = DateTime.Now;
        /// <summary></summary>
        //[Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Updated { get { return this._updated; } set { _updated = value; } }
        private DateTime _updated = DateTime.Now;
        /// <summary></summary>
        //[Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public bool Deleted { get { return this._deleted; } set { _deleted = value; } }
        private bool _deleted = false;

    }
}
