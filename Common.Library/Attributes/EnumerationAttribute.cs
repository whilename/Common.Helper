using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Attributes
{
    /// <summary>枚举字段特性</summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumerationAttribute : Attribute
    {
        /// <summary>键</summary>
        public string Key { get; set; }

        /// <summary>描述</summary>
        public string Description { get; set; }

        /// <summary>动作</summary>
        public string Action { get; set; }

        /// <summary></summary>
        public EnumerationAttribute() { }

        /// <summary></summary>
        /// <param name="description"></param>
        public EnumerationAttribute(string description) { this.Description = description; }

        /// <summary></summary>
        /// <param name="key"></param>
        /// <param name="description"></param>
        public EnumerationAttribute(string key, string description) : this(description) { this.Key = key; }

        /// <summary></summary>
        /// <param name="key"></param>
        /// <param name="description"></param>
        /// <param name="action"></param>
        public EnumerationAttribute(string key, string description, string action) : this(key, description) { this.Action = action; }

    }
}
