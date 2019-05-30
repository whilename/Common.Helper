using eQuartz.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Design;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;

namespace eQuartz.Services
{
    public class ORMContext: DbContext
    {
        public ORMContext() : base("ConnectionString") { }

        /// <summary></summary>
        public DbSet<JobEntity> Job { get; set; }
        /// <summary></summary>
        public DbSet<DictEntity> Dict { get; set; }
        /// <summary></summary>
        public DbSet<EmailEntity> Email { get; set; }
        /// <summary></summary>
        public DbSet<LogEntity> Log { get; set; }

    }

    /*
    internal sealed class Configuration : DbMigrationsConfiguration<ORMContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("System.Data.SqlClient", new CustomSqlServerMigrationSqlGenerator());
        }
    }

    internal class CustomSqlServerMigrationSqlGenerator : SqlServerMigrationSqlGenerator
    {
        protected override void Generate(AddColumnOperation addColumnOperation)
        {
            SetCreatedUtcColumn(addColumnOperation.Column);

            base.Generate(addColumnOperation);
        }

        protected override void Generate(CreateTableOperation createTableOperation)
        {
            SetCreatedUtcColumn(createTableOperation.Columns);

            base.Generate(createTableOperation);
        }

        private static void SetCreatedUtcColumn(IEnumerable<ColumnModel> columns)
        {
            foreach (var columnModel in columns)
            {
                SetCreatedUtcColumn(columnModel);
            }
        }

        private static void SetCreatedUtcColumn(PropertyModel column)
        {
            if (column.Name == "Created"||column.Name== "Updated")
            {
                column.DefaultValueSql = "GETDATE()";
            }
        }
    }
    */

    /*
    internal sealed class Configuration : DbMigrationsConfiguration<ORMContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"Migrations";
            this.CodeGenerator = new ExtendedMigrationCodeGenerator();
        }
    }

    public class ExtendedMigrationCodeGenerator : MigrationCodeGenerator
    {
        public override ScaffoldedMigration Generate(string migrationId, IEnumerable<MigrationOperation> operations, string sourceModel, string targetModel, string @namespace, string className)
        {
            foreach (MigrationOperation operation in operations)
            {
                if (operation is CreateTableOperation)
                {
                    foreach (var column in ((CreateTableOperation)operation).Columns)
                        if (column.ClrType == typeof(DateTime) && column.IsNullable.HasValue && !column.IsNullable.Value && string.IsNullOrEmpty(column.DefaultValueSql))
                            column.DefaultValueSql = "GETDATE()";
                }
                else if (operation is AddColumnOperation)
                {
                    ColumnModel column = ((AddColumnOperation)operation).Column;

                    if (column.ClrType == typeof(DateTime) && column.IsNullable.HasValue && !column.IsNullable.Value && string.IsNullOrEmpty(column.DefaultValueSql))
                        column.DefaultValueSql = "GETDATE()";
                }
            }

            CSharpMigrationCodeGenerator generator = new CSharpMigrationCodeGenerator();

            return generator.Generate(migrationId, operations, sourceModel, targetModel, @namespace, className);
        }
    }
    */

}