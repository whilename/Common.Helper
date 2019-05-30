using Dapper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.AdoNet
{
    /// <summary></summary>
    public class SqlMapper<T> : IDisposable where T : class
    {
        private static DbHelper _db_helper = null;
        private static readonly object padlock = new object();
        private static string _connectionString;
        private static string _providerName;
        /// <summary>数据库表名</summary>
        private string _table_name;

        private string _columns = "*";
        /// <summary>数据列（默认*所有）</summary>
        protected string Columns { get { return _columns; } set { _columns = value; } }

        /// <summary>缓存对象属性名,用于where条件变量</summary>
        private static ConcurrentDictionary<string, List<string>> _properties = new ConcurrentDictionary<string, List<string>>();
        /// <summary>Db链接对象</summary>
        private static DbHelper db_helper
        {
            get
            {
                if (_db_helper == null)
                {
                    lock (padlock) { if (_db_helper == null) { _db_helper = new DbHelper(_connectionString, _providerName); } }
                }
                return _db_helper;
            }
        }

        /// <summary></summary>
        /// <param name="tableName">数据库表名</param>
        /// <param name="connectionString">数据库链接字符串</param>
        /// <param name="providerName">提供程序名称</param>
        public SqlMapper(string tableName = null, string connectionString = null, string providerName = null)
        {
            _table_name = string.IsNullOrEmpty(tableName) ? this.GetType().Name : tableName;
            _connectionString = connectionString;
            _providerName = providerName;
        }

        #region Dispose

        /// <summary>资源回收事件</summary>
        public EventHandler OnDispose { get; set; }

        /// <summary>关闭连接回收资源</summary>
        public void Dispose()
        {
            // 关闭连接
            if (db_helper != null) { db_helper.Dispose(); _db_helper = null; }
            if (OnDispose != null) { OnDispose(this, new EventArgs()); }
            System.GC.SuppressFinalize(this);
        }

        #endregion

        #region Transaction处理

        /// <summary>Begin DbTransaction</summary>
        public IDbTransaction TrnStart() { db_helper.TrnStart(); return db_helper.dbtrans; }

        /// <summary>Commit DbTransaction</summary>
        public void TrnCommit() { db_helper.TrnCommit(); }

        /// <summary>RollBack DbTransaction</summary>
        public void TrnRollBack() { db_helper.TrnRollBack(); }

        #endregion

        /// <summary>执行sql并返回受影响值</summary>
        /// <param name="sqlstr">执行的SQL</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public int Execute(string sqlstr, object param = null)
        {
            return db_helper.DBConnection.Execute(sqlstr, param, db_helper.dbtrans, db_helper.ComTimeout, CommandType.Text);
        }

        /// <summary>执行sql并返回受影响值</summary>
        /// <param name="sqlstr">执行的SQL</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public O Execute<O>(string sqlstr, object param = null)
        {
            return db_helper.DBConnection.ExecuteScalar<O>(sqlstr, param, db_helper.dbtrans);
        }

        /// <summary>执行sql并返回执行后的结果值</summary>
        /// <param name="sqlstr">执行的SQL</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlstr, object param = null)
        {
            return db_helper.DBConnection.ExecuteScalar(sqlstr, param, db_helper.dbtrans, db_helper.ComTimeout, CommandType.Text);
        }

        /// <summary>执行sql并返回读取数据操作对象</summary>
        /// <param name="sqlstr">执行的SQL</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sqlstr, object param = null)
        {
            return db_helper.DBConnection.ExecuteReader(sqlstr, param, db_helper.dbtrans, db_helper.ComTimeout, CommandType.Text);
        }

        /// <summary>执行sql并返回读取数据表</summary>
        /// <param name="sqlstr">执行的SQL</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public DataTable QueryDataTable(string sqlstr, object param = null)
        {
            DataTable table = new DataTable();
            table.Load(ExecuteReader(sqlstr, param));
            return table;
        }

        /// <summary>统计数量</summary>
        /// <param name="conditions">条件</param>
        /// <returns></returns>
        public int Count(object conditions)
        {
            return db_helper.DBConnection.QuerySingle<int>(QuerySQL("count(*)", conditions, null), conditions, db_helper.dbtrans, db_helper.ComTimeout, CommandType.Text);
        }

        /// <summary>查询符合条件的第一条数据</summary>
        /// <param name="conditions">条件</param>
        /// <param name="orderby">排序条件</param>
        /// <returns></returns>
        public T FindSingle(object conditions, string orderby = "")
        {
            return db_helper.DBConnection.QuerySingle<T>(QuerySQL(Columns, conditions, orderby), conditions, db_helper.dbtrans, db_helper.ComTimeout, CommandType.Text);
        }

        /// <summary>查询符合条件的第一条数据</summary>
        /// <typeparam name="O"></typeparam>
        /// <param name="sqlstr">执行sql脚本</param>
        /// <param name="conditions">条件</param>
        /// <returns>未找到返回null</returns>
        public O FindSingle<O>(string sqlstr, object conditions)
        {
            return db_helper.DBConnection.QuerySingleOrDefault<O>(sqlstr, conditions, db_helper.dbtrans, db_helper.ComTimeout, CommandType.Text);
        }

        /// <summary>查询符合条件的第一条数据</summary>
        /// <param name="conditions">条件</param>
        /// <param name="orderby">排序条件</param>
        /// <returns></returns>
        public T FindSingleOrDefault(object conditions, string orderby = "")
        {
            return db_helper.DBConnection.QuerySingleOrDefault<T>(QuerySQL(Columns, conditions, orderby), conditions, db_helper.dbtrans, db_helper.ComTimeout, CommandType.Text);
        }

        /// <summary>查询符合条件的所有数据</summary>
        /// <param name="conditions">条件</param>
        /// <param name="orderby">排序条件</param>
        /// <returns></returns>
        public IEnumerable<T> FindAll(object conditions, string orderby)
        {
            return db_helper.DBConnection.Query<T>(QuerySQL(Columns, conditions, orderby), conditions, db_helper.dbtrans, true, db_helper.ComTimeout, CommandType.Text);
        }

        /// <summary></summary>
        /// <typeparam name="o"></typeparam>
        /// <param name="sqlstr"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public IEnumerable<o> Query<o>(string sqlstr, object conditions) where o : class
        {
            return db_helper.DBConnection.Query<o>(sqlstr, conditions, db_helper.dbtrans, true, db_helper.ComTimeout, CommandType.Text);
        }

        /// <summary>分页查询符合条件的所有数据</summary>
        /// <param name="page">分页对象</param>
        /// <param name="conditions">条件</param>
        /// <param name="orderby">排序条件</param>
        /// <returns></returns>
        public IEnumerable<T> FindAllByPage(SqlPage page, object conditions, string orderby)
        {
            // 获取总行数
            page.Rows = this.Count(conditions);
            // 获取查询sql
            string query_sql = QuerySQL(Columns, conditions);
            // 是否有排序
            string order_by = string.IsNullOrEmpty(orderby) ? string.Empty : string.Format(" ORDER BY {0} desc ", orderby);
            // 拼接分页查询sql
            string query_page_sql = string.Format("SELECT * FROM({0})T {1} OFFSET {2} ROWS FETCH NEXT {3} ROWS ONLY", query_sql, order_by, page.OffSet, page.Next);
            return db_helper.DBConnection.Query<T>(query_page_sql, conditions, db_helper.dbtrans, true, db_helper.ComTimeout, CommandType.Text);
        }

        /// <summary>SQL ROW_NUMBER方式分页查询符合条件的所有数据</summary>
        /// <param name="page"></param>
        /// <param name="conditions"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public IEnumerable<T> FindAllForRowNumByPage(SqlPage page, object conditions, string orderby)
        {
            // 获取总行数
            page.Rows = this.Count(conditions);
            // 获取查询sql
            string query_sql = QuerySQL(Columns, conditions);
            query_sql.Insert(5, string.Format(" ROW_NUMBER() OVER (ORDER BY {0}) AS RowNumber,", orderby));
            // 是否有排序
            string order_by = string.IsNullOrEmpty(orderby) ? string.Empty : string.Format(" ORDER BY {0} desc ", orderby);
            // 拼接分页查询sql
            string query_page_sql_row_num = string.Format("SELECT TOP({0}) * FROM({1})T WHERE RowNumber>{2} {3}", page.Size, query_sql, page.OffSet, order_by);
            return db_helper.DBConnection.Query<T>(query_page_sql_row_num, conditions, db_helper.dbtrans, true, db_helper.ComTimeout, CommandType.Text);
        }

        /// <summary>执行分页查询sql</summary>
        /// <typeparam name="o"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public IEnumerable<o> QueryPagesBySQL<o>(string sql, object parameters, int size, int index) where o : class
        {
            SqlPage page = new SqlPage { Size = size, Index = index };
            string query_page_sql = string.Format("{0} OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY", sql, page.OffSet, page.Next - page.OffSet);
            return db_helper.DBConnection.Query<o>(query_page_sql, parameters, db_helper.dbtrans, true, db_helper.ComTimeout, CommandType.Text);
        }

        /// <summary>获取执行sql</summary>
        /// <param name="columns">字段列</param>
        /// <param name="conditions">条件</param>
        /// <returns></returns>
        public string QuerySQL(string columns, object conditions)
        {
            string sqlstr = string.Format("SELECT {0} FROM {1} ", columns, _table_name);
            // 获取条件字段名
            List<string> where = GetPropertyInfos(conditions);
            // where 条件
            if (conditions != null && where.Count > 0) { sqlstr += " WHERE " + string.Join(" AND ", where.Select(p => p + " = @" + p)); }
            return sqlstr.ToString();
        }

        /// <summary>获取执行sql</summary>
        /// <param name="columns">字段列</param>
        /// <param name="conditions">条件</param>
        /// <param name="orderby">排序条件</param>
        /// <returns></returns>
        public string QuerySQL(string columns, object conditions, string orderby)
        {
            string sqlstr = QuerySQL(columns, conditions);
            // 排序条件
            if (!string.IsNullOrEmpty(orderby)) { sqlstr += " ORDER BY " + orderby; }
            return sqlstr;
        }

        /// <summary>获取对象属性列表</summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<string> GetPropertyInfos(object obj)
        {
            List<string> properties = new List<string>();
            if (obj == null) { return properties; }
            // 动态类型
            if (obj is DynamicParameters) { return (obj as DynamicParameters).ParameterNames.ToList(); }
            // 查找对象缓存属性
            if (_properties.TryGetValue(obj.GetType().Name, out properties)) { return properties; }
            // 获取对象属性并缓存
            _properties[obj.GetType().Name] = properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Select(x => x.Name).ToList();
            return properties;
        }

        /// <summary>分页对象</summary>
        public class SqlPage
        {
            /// <summary>分页对象</summary>
            public SqlPage() { Index = 1; Size = 20; }
            /// <summary>页码</summary>
            public int Index { get; set; }
            /// <summary>页大小</summary>
            public int Size { get; set; }
            /// <summary>总行数</summary>
            public int Rows { get; internal set; }
            private int _page;
            /// <summary>总页数</summary>
            public int Page
            {
                get
                {
                    if (Size <= 0 || Rows <= 0) { _page = 0; }
                    _page = Rows / Size;
                    // 计算总页数
                    if (Rows % Size > 0) { _page++; }
                    return _page;
                }
            }

            /// <summary>起始行</summary>
            public int OffSet { get { return Index <= 1 ? 0 : ((Index - 1) * Size); } }
            /// <summary>结束行</summary>
            public int Next { get { return Index <= 1 ? Size : Index * Size; } }

        }
    }

    /// <summary></summary>
    public class SqlBuilder
    {
        private readonly Dictionary<string, Clauses> _data = new Dictionary<string, Clauses>();
        private int _seq;

        /// <summary></summary>
        private class Clause
        {
            public string Sql { get; set; }
            public object Parameters { get; set; }
            public bool IsInclusive { get; set; }
        }

        /// <summary></summary>
        private class Clauses : List<Clause>
        {
            private readonly string _joiner, _prefix, _postfix;

            /// <summary></summary>
            /// <param name="joiner"></param>
            /// <param name="prefix"></param>
            /// <param name="postfix"></param>
            public Clauses(string joiner, string prefix = "", string postfix = "")
            {
                _joiner = joiner;
                _prefix = prefix;
                _postfix = postfix;
            }
            /// <summary></summary>
            /// <param name="p"></param>
            /// <returns></returns>
            public string ResolveClauses(DynamicParameters p)
            {
                foreach (var item in this) { p.AddDynamicParams(item.Parameters); }
                string[] conditions = this.Any(a => a.IsInclusive) ?
                    this.Where(a => !a.IsInclusive).Select(c => c.Sql).Union(new[]
                    {
                        " ( " + string.Join(" OR ", this.Where(a => a.IsInclusive).Select(c => c.Sql).ToArray()) + " ) "
                    }).ToArray() : this.Select(c => c.Sql).ToArray();
                return _prefix + string.Join(_joiner, conditions) + _postfix;
            }
        }
        /// <summary></summary>
        public class Template
        {
            private readonly string _sql;
            private readonly SqlBuilder _builder;
            private readonly object _initParams;
            private int _dataSeq = -1; // Unresolved

            /// <summary></summary>
            /// <param name="builder"></param>
            /// <param name="sql"></param>
            /// <param name="parameters"></param>
            public Template(SqlBuilder builder, string sql, dynamic parameters)
            {
                _initParams = parameters;
                _sql = sql;
                _builder = builder;
            }

            private static readonly Regex _regex = new Regex(@"\/\*\*.+?\*\*\/", RegexOptions.Compiled | RegexOptions.Multiline);
            /// <summary></summary>
            private void ResolveSql()
            {
                if (_dataSeq != _builder._seq)
                {
                    var p = new DynamicParameters(_initParams);
                    rawSql = _sql;
                    foreach (var pair in _builder._data)
                    {
                        rawSql = rawSql.Replace("/**" + pair.Key + "**/", pair.Value.ResolveClauses(p));
                    }
                    parameters = p;
                    // replace all that is left with empty
                    rawSql = _regex.Replace(rawSql, "");
                    _dataSeq = _builder._seq;
                }
            }

            private string rawSql;
            private object parameters;
            /// <summary></summary>
            public string RawSql { get { ResolveSql(); return rawSql; } }
            /// <summary></summary>
            public object Parameters { get { ResolveSql(); return parameters; } }
        }
        /// <summary></summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Template AddTemplate(string sql, dynamic parameters = null) { return new Template(this, sql, parameters); }
        /// <summary></summary>
        /// <param name="name"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="joiner"></param>
        /// <param name="prefix"></param>
        /// <param name="postfix"></param>
        /// <param name="isInclusive"></param>
        /// <returns></returns>
        protected SqlBuilder AddClause(string name, string sql, object parameters, string joiner, string prefix = "", string postfix = "", bool isInclusive = false)
        {
            Clauses clauses;
            if (!_data.TryGetValue(name, out clauses))
            {
                clauses = new Clauses(joiner, prefix, postfix);
                _data[name] = clauses;
            }
            clauses.Add(new Clause { Sql = sql, Parameters = parameters, IsInclusive = isInclusive });
            _seq++;
            return this;
        }
        /// <summary></summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public SqlBuilder Intersect(string sql, dynamic parameters = null)
        {
            return AddClause("intersect", sql, parameters, "\nINTERSECT\n ", "\n ", "\n");
        }
        /// <summary></summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public SqlBuilder InnerJoin(string sql, dynamic parameters = null)
        {
            return AddClause("innerjoin", sql, parameters, "\nINNER JOIN ", "\nINNER JOIN ", "\n");
        }
        /// <summary></summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public SqlBuilder LeftJoin(string sql, dynamic parameters = null)
        {
            return AddClause("leftjoin", sql, parameters, "\nLEFT JOIN ", "\nLEFT JOIN ", "\n");
        }
        /// <summary></summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public SqlBuilder RightJoin(string sql, dynamic parameters = null)
        {
            return AddClause("rightjoin", sql, parameters, "\nRIGHT JOIN ", "\nRIGHT JOIN ", "\n");
        }
        /// <summary></summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public SqlBuilder Where(string sql, dynamic parameters = null)
        {
            return AddClause("where", sql, parameters, " AND ", "WHERE ", "\n");
        }
        /// <summary></summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public SqlBuilder OrWhere(string sql, dynamic parameters = null)
        {
            return AddClause("where", sql, parameters, " OR ", "WHERE ", "\n", true);
        }
        /// <summary></summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public SqlBuilder OrderBy(string sql, dynamic parameters = null)
        {
            return AddClause("orderby", sql, parameters, " , ", "ORDER BY ", "\n");
        }
        /// <summary></summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public SqlBuilder Select(string sql, dynamic parameters = null)
        {
            return AddClause("select", sql, parameters, " , ", "", "\n");
        }
        /// <summary></summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public SqlBuilder AddParameters(dynamic parameters)
        {
            return AddClause("--parameters", "", parameters, "", "", "");
        }
        /// <summary></summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public SqlBuilder Join(string sql, dynamic parameters = null)
        {
            return AddClause("join", sql, parameters, "\nJOIN ", "\nJOIN ", "\n");
        }
        /// <summary></summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public SqlBuilder GroupBy(string sql, dynamic parameters = null)
        {
            return AddClause("groupby", sql, parameters, " , ", "\nGROUP BY ", "\n");
        }
        /// <summary></summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public SqlBuilder Having(string sql, dynamic parameters = null)
        {
            return AddClause("having", sql, parameters, "\nAND ", "HAVING ", "\n");
        }
    }
}
