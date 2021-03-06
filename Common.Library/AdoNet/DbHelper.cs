﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Data.Common;
using System.Configuration;
using Newtonsoft.Json;
using System.Data.SQLite;

namespace Common.AdoNet
{
    /// <summary>数据库操作类</summary>
    public class DbHelper : IDisposable
    {
        private const string APPSETTING_KEY = "db_connectionstring";
        private const string SQL_PROVIDERNAME = "System.Data.SqlClient";
        private int commontTimeout = 30;
        /// <summary>Timeout</summary>
        public int ComTimeout { get { return commontTimeout; } set { commontTimeout = value; } }

        /// <summary>身份验证模式</summary>
        public enum enmDBVerifyType
        {
            /// <summary>SQLServer身份</summary>
            SQLServer,
            /// <summary>Windows身份</summary>
            Windows
        }

        /// <summary>数据库事务对象</summary>
        public IDbTransaction dbtrans;
        /// <summary>数据库源实例工厂</summary>
        private DbProviderFactory mfactory;

        #region 构造

        /// <summary>构造一个数据库操作类的实例</summary>
        /// <param name="connectionstring">数据库连接字符串</param>
        /// <param name="providername">连接驱动名</param>
        public DbHelper(string connectionstring = APPSETTING_KEY, string providername = SQL_PROVIDERNAME)
        {
            connection_string = connectionstring;
            provider_name = providername;
            if (string.IsNullOrEmpty(provider_name)) { Reload(); }
            mfactory = DbProviderFactories.GetFactory(ProviderName.IndexOf("sqlite", StringComparison.CurrentCultureIgnoreCase) > 0 ? SQL_PROVIDERNAME : ProviderName);
            dbcon = this.CreateNewConnection();
        }

        #endregion

        #region Dispose

        /// <summary>资源回收事件</summary>
        public EventHandler OnDispose { get; set; }

        /// <summary>关闭连接回收资源</summary>
        public void Dispose()
        {
            // 关闭连接
            this.Close();
            if (OnDispose != null) { OnDispose(this, new EventArgs()); }
            dbcon.Dispose();
            dbcon = null;
            System.GC.SuppressFinalize(this);
        }

        #endregion

        #region property

        private string provider_name;
        /// <summary>提供程序名称</summary>
        public string ProviderName
        {
            get { if (string.IsNullOrEmpty(provider_name)) { Reload(); } return provider_name; }
            set { provider_name = value; }
        }

        private string connection_string;
        /// <summary>数据库连接字符串</summary>
        public string ConnectionString
        {
            get { if (string.IsNullOrEmpty(connection_string)) { Reload(); } return connection_string; }
            set { connection_string = value; }
        }

        /// <summary>获取/设置数据库链接配置/提供程序名称</summary>
        private void Reload()
        {
            if (ConfigurationManager.ConnectionStrings[APPSETTING_KEY] != null)
            {
                connection_string = ConfigurationManager.ConnectionStrings[APPSETTING_KEY].ConnectionString;
                provider_name = ConfigurationManager.ConnectionStrings[APPSETTING_KEY].ProviderName;
            }
            else if (ConfigurationManager.ConnectionStrings.Count > 0)
            {
                connection_string = ConfigurationManager.ConnectionStrings[0].ConnectionString;
                provider_name = ConfigurationManager.ConnectionStrings[0].ProviderName;
            }
            else { throw new Exception("未能在config文件中找到数据库链接配置"); }
        }

        private IDbConnection dbcon;
        /// <summary>当前数据库连接</summary>
        public IDbConnection DBConnection
        {
            get
            {
                if (dbcon.State == ConnectionState.Closed) { this.Open(); }
                return dbcon;
            }
        }

        private string mstrTaihiSQL = String.Empty;
        /// <summary>取得最后一次执行的sql语句</summary>
        public string TaihiSQL { get { return mstrTaihiSQL; } }

        #endregion

        #region CreateNewConnection

        /// <summary>创建新的连接</summary>
        /// <returns></returns>
        public IDbConnection CreateNewConnection()
        {
            IDbConnection con = ProviderName.IndexOf("sqlite", StringComparison.CurrentCultureIgnoreCase) > 0 ? new SQLiteConnection(ConnectionString) : mfactory.CreateConnection();
            con.ConnectionString = ConnectionString;
            return con;
        }

        #endregion

        #region Open/Close

        /// <summary>打开数据库连接</summary>
        /// <returns></returns>
        public void Open()
        {
            dbcon.ConnectionString = ConnectionString;
            //打开连接
            dbcon.Open();
        }

        /// <summary>关闭数据库连接</summary>
        /// <returns></returns>
        public void Close()
        {
            if (dbcon.State == ConnectionState.Open) { dbcon.Close(); }
        }

        #endregion

        #region CreateCommand (private)

        /// <summary>创建执行命令连接</summary>
        /// <returns></returns>
        private IDbCommand CreateCommand()
        {
            if (dbcon.State == System.Data.ConnectionState.Closed) { this.Open(); }
            IDbCommand cmd = dbcon.CreateCommand();
            cmd.CommandTimeout = ComTimeout;
            if (dbtrans != null) { cmd.Transaction = dbtrans; }
            cmd.Parameters.Clear();// 清理执行sql的参数
            return cmd;
        }

        #endregion

        #region CreateParameter

        /// <summary>创建sql参数</summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbParameter CreateParameter(string name, object value)
        {
            DbParameter param = mfactory.CreateParameter();
            param.ParameterName = name;
            param.Value = (value == null ? DBNull.Value : value);
            return param;
        }

        /// <summary>创建sql参数</summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dbtype"></param>
        /// <returns></returns>
        public DbParameter CreateParameter(string name, object value, DbType dbtype)
        {
            DbParameter param = mfactory.CreateParameter();
            param.ParameterName = name;
            param.Value = (value == null ? DBNull.Value : value);
            param.DbType = dbtype;
            return param;
        }

        /// <summary>创建sql参数</summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dbtype"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public DbParameter CreateParameter(string name, object value, DbType dbtype, int size)
        {
            DbParameter param = mfactory.CreateParameter();
            param.ParameterName = name;
            param.Value = (value == null ? DBNull.Value : value);
            param.DbType = dbtype;
            param.Size = size;
            return param;
        }

        #endregion

        #region ExecuteReader

        /// <summary>执行sql语句（select），返回OleDbDataReader</summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql)
        {
            return ExecuteReader(sql, null);
        }

        /// <summary>是否有参数</summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private bool HasParam(DbParameter[] parameters)
        {
            return parameters != null && parameters.Length > 0;
        }

        /// <summary>记录异常与参数日志</summary>
        /// <param name="ex"></param>
        /// <param name="parameters"></param>
        private void LogParamsToEx(DbException ex, DbParameter[] parameters)
        {
            if (!HasParam(parameters))
                return;
            for (int i = 0; i < parameters.Length; i++)
            {
                ex.Data.Add(string.Format("p{0}-{1}", i + 1, parameters[i].ParameterName),
                    JsonConvert.SerializeObject(parameters[i].Value));
            }
        }

        /// <summary>执行sql语句（select），返回OleDbDataReader</summary>
        /// <param name="sql"></param>
        /// <param name="arrparm"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql, DbParameter[] arrparm)
        {
            bool hasParam = HasParam(arrparm);
            try
            {
                DBConnection.Open();
                using (IDbCommand cmd = CreateCommand())
                {
                    cmd.CommandText = sql;
                    if (hasParam)
                    {
                        for (int i = 0; i < arrparm.Length; i++)
                        {
                            cmd.Parameters.Add(arrparm[i]);
                        }
                    }
                    return cmd.ExecuteReader();
                }
            }
            catch (DbException ex)
            {
                ex.Data.Add("sql", sql);
                LogParamsToEx(ex, arrparm);
                throw;
            }
        }

        #endregion

        #region ExecuteNonQuery

        /// <summary>执行sql语句（insert，update，delete）</summary>
        /// <param name="sql"></param>
        /// <returns>sql执行影响的记录数</returns>
        public int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(sql, null);
        }

        /// <summary>执行带参数的sql语句（insert，update，delete）</summary>
        /// <param name="sql"></param>
        /// <param name="arrparm"></param>
        /// <returns>sql执行影响的记录数</returns>
        public int ExecuteNonQuery(string sql, DbParameter[] arrparm)
        {
            bool hasParam = HasParam(arrparm);
            try
            {
                using (IDbCommand cmd = CreateCommand())
                {
                    cmd.CommandText = sql;
                    if (hasParam)
                    {
                        for (int i = 0; i < arrparm.Length; i++)
                        {
                            cmd.Parameters.Add(arrparm[i]);
                        }
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (DbException ex)
            {
                ex.Data.Add("sql", sql);
                LogParamsToEx(ex, arrparm);
                throw;
            }
        }

        #endregion

        #region ExecuteScalar

        /// <summary>执行sql语句，并返回查询所返回的结果集中第一行的第一列的值</summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql)
        {
            return ExecuteScalar(sql, null);
        }

        /// <summary>执行sql语句，并返回查询所返回的结果集中第一行的第一列的值</summary>
        /// <param name="sql"></param>
        /// <param name="arrparm"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, DbParameter[] arrparm)
        {
            bool hasParam = HasParam(arrparm);
            try
            {
                using (IDbCommand cmd = CreateCommand())
                {
                    cmd.CommandText = sql;
                    if (hasParam)
                    {
                        for (int i = 0; i < arrparm.Length; i++)
                        {
                            cmd.Parameters.Add(arrparm[i]);
                        }
                    }
                    return cmd.ExecuteScalar();
                }
            }
            catch (DbException ex)
            {
                ex.Data.Add("sql", sql);
                LogParamsToEx(ex, arrparm);
                throw;
            }
        }

        #endregion

        #region ExecuteProcedure

        /// <summary>执行存储过程，返回DbDataReader</summary>
        /// <param name="procname">存储过程名称</param>
        /// <param name="arrparm">参数</param>
        /// <returns>存储过程执行结果DbDataReader</returns>
        public IDataReader ExeDataReaderProcedure(string procname, DbParameter[] arrparm)
        {
            bool hasParam = HasParam(arrparm);
            try
            {
                DBConnection.Open();
                using (IDbCommand cmd = CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procname;
                    if (hasParam)
                    {
                        for (int i = 0; i < arrparm.Length; i++)
                        {
                            cmd.Parameters.Add(arrparm[i]);
                        }
                    }
                    return cmd.ExecuteReader();
                }
            }
            catch (DbException ex)
            {
                ex.Data.Add("sp", procname);
                LogParamsToEx(ex, arrparm);
                throw;
            }
        }

        /// <summary>执行存储过程，返回受影响行数</summary>
        /// <param name="procname">存储过程名称</param>
        /// <param name="arrparm">参数</param>
        public int ExecuteNonProcedure(string procname, DbParameter[] arrparm)
        {
            bool hasParam = HasParam(arrparm);
            try
            {
                using (IDbCommand cmd = CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procname;
                    if (hasParam)
                    {
                        for (int i = 0; i < arrparm.Length; i++)
                        {
                            cmd.Parameters.Add(arrparm[i]);
                        }
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (DbException ex)
            {
                ex.Data.Add("sp", procname);
                LogParamsToEx(ex, arrparm);
                throw;
            }
        }

        /// <summary>执行存储过程，返回DataSet</summary>
        /// <param name="procname">存储过程名称</param>
        /// <param name="arrparm">参数</param>
        /// <returns>存储过程执行结果DataSet</returns>
        public DataSet ExeDatasetProcedure(string procname, DbParameter[] arrparm)
        {
            bool hasParam = HasParam(arrparm);
            try
            {
                using (IDbCommand cmd = this.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procname;
                    if (hasParam)
                    {
                        for (int i = 0; i < arrparm.Length; i++)
                        {
                            cmd.Parameters.Add(arrparm[i]);
                        }
                    }
                    DataSet ds = new DataSet();
                    IDbDataAdapter da = mfactory.CreateDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (DbException ex)
            {
                ex.Data.Add("sp", procname);
                LogParamsToEx(ex, arrparm);
                throw;
            }
        }

        #endregion

        #region GetDataSet

        /// <summary>执行sql语句返回DataSet</summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string sql)
        {
            return GetDataSet(sql, null);
        }

        /// <summary>执行sql语句返回DataSet</summary>
        /// <param name="sql"></param>
        /// <param name="arrparm"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string sql, DbParameter[] arrparm)
        {
            bool hasParam = HasParam(arrparm);
            try
            {
                using (IDbCommand cmd = this.CreateCommand())
                {
                    cmd.CommandText = sql;
                    if (hasParam)
                    {
                        for (int i = 0; i < arrparm.Length; i++)
                        {
                            cmd.Parameters.Add(arrparm[i]);
                        }
                    }
                    DataSet ds = new DataSet();
                    IDbDataAdapter da = mfactory.CreateDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (DbException ex)
            {
                ex.Data.Add("sql", sql);
                LogParamsToEx(ex, arrparm);
                throw;
            }
        }

        #endregion

        #region Transaction处理

        /// <summary>开始事务处理</summary>
        /// <returns></returns>
        public void TrnStart()
        {
            if (dbcon.State == System.Data.ConnectionState.Closed) { this.Open(); }
            dbtrans = dbcon.BeginTransaction();
        }

        /// <summary>提交事务</summary>
        /// <returns></returns>
        public void TrnCommit()
        {
            if (dbcon.State == System.Data.ConnectionState.Closed) { this.Open(); }
            if (dbtrans != null) { dbtrans.Commit(); }
        }

        /// <summary>撤销事务</summary>
        /// <returns></returns>
        public void TrnRollBack()
        {
            try
            {
                if (dbcon.State == System.Data.ConnectionState.Closed) { this.Open(); }
                if (dbtrans != null) { dbtrans.Rollback(); }
            }
            catch { }
        }

        #endregion

        #region BuildConnectString

        /// <summary>构建连接字符串</summary>
        /// <param name="type"></param>
        /// <param name="server"></param>
        /// <param name="database"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string BuildConnectString(enmDBVerifyType type, string server, string database, string user, string password)
        {
            string connstr = String.Empty;
            if (type == enmDBVerifyType.SQLServer)
                connstr = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};Pooling=false;", server, database, user, password);
            else
                connstr = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True;", server, database);
            return connstr;
        }

        #endregion
    }
}
