using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Common.AdoNet
{
    /// <summary></summary>
    public class SqlMapper<T> where T : class
    {
        /// <summary>数据库表名</summary>
        private string _table_name;
        private DbHelper db_helper;

        /// <summary></summary>
        /// <param name="tableName">数据库表名</param>
        public SqlMapper(string tableName)
        {
            _table_name = tableName;
            db_helper = new DbHelper();
        }

        /// <summary></summary>
        /// <param name="tableName">数据库表名</param>
        /// <param name="connectionString">数据库链接字符串</param>
        public SqlMapper(string tableName, string connectionString)
        {
            _table_name = tableName;
            db_helper = new DbHelper(connectionString);
        }

        /// <summary></summary>
        /// <param name="tableName">数据库表名</param>
        /// <param name="connectionString">数据库链接字符串</param>
        /// <param name="providerName">提供程序名称</param>
        public SqlMapper(string tableName, string connectionString, string providerName)
        {
            _table_name = tableName;
            db_helper = new DbHelper(connectionString, providerName);
        }

        /// <summary>统计数量</summary>
        /// <param name="conditions">条件</param>
        /// <returns></returns>
        public int Count(object conditions)
        {
            return db_helper.DBConnection.QuerySingle<int>(QuerySQL("count(*)", conditions, null));
        }

        /// <summary>查询符合条件的第一条数据</summary>
        /// <param name="conditions">条件</param>
        /// <param name="orderby">排序条件</param>
        /// <returns></returns>
        public IEnumerable<T> FindAll(object conditions, object orderby)
        {
            return db_helper.DBConnection.Query<T>(QuerySQL("*", conditions, orderby));
        }

        /// <summary>获取执行sql</summary>
        /// <param name="columns">字段列</param>
        /// <param name="conditions">条件</param>
        /// <param name="orderby">排序条件</param>
        /// <returns></returns>
        public string QuerySQL(string columns, object conditions,object orderby)
        {
            return string.Format("select {0} from {1} ", columns, _table_name);
        }

    }
}
