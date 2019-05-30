using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartPortal.Models
{
    /// <summary>
    /// 菜单实体类定义
    /// </summary>
    public class Menu : BaseEntity
    {
        /// <summary>
        /// 菜单编号
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

    }
}