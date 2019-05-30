using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SmartPortal.Models
{
    public class BaseEntity 
    {
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