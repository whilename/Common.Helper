using Common.AdoNet;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApp
{
    /// <summary></summary>
    public class SQLiteDbConfig
    {
        /// <summary></summary>
        public static void Register()
        {
            SqlMapper<object> mapper = new SqlMapper<object>("");

            // Create Table 
            string sql_create_table_users = @"CREATE TABLE IF NOT EXISTS [Users] (
    [UserId] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    [UserNo] VARCHAR(32) NULL,
    [UserName] VARCHAR(32) NULL,
    [UserMobile] VARCHAR(16) NULL,
    [UserMail] VARCHAR(64) NULL,
    [UserPwd] NVARCHAR(128) NULL,
    [Created] TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    [Updated] TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    [Deleted] INT DEFAULT 0
)";
            // Set identity sequence number
            string sql_set_table_users_seq = "UPDATE sqlite_sequence SET seq = 0 WHERE name = 'Users'";
            string sql_insert_table_user = "INSERT INTO Users(UserNo,UserName,UserMobile,UserMail,UserPwd) VALUES('1103397760','OWEN','18221126664','qunzhi.ouyang@thermofisher.com','thermo2010')";

            try
            {
                int create = mapper.Execute(sql_create_table_users, null);
                int setseq = mapper.Execute(sql_set_table_users_seq, null);
                int insert = mapper.Execute(sql_insert_table_user, null);
                object obj = mapper.FindSingle<object>("SELECT * FROM Users WHERE UserNo=@UserNo", new { UserNo = "1103397760" });
            }
            catch (Exception ex) { Log.Error(ex); }


        }
    }
}