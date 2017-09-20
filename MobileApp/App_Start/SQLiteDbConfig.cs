using Common.AdoNet;
using Common.Utility;
using Mobile.Models;
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
            string sql_create_table_users = @"
-- Create table for Users
CREATE TABLE IF NOT EXISTS [Users] (
    [UserId] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    [UserNo] VARCHAR(32) NULL,
    [UserName] VARCHAR(32) NULL,
    [UserMobile] VARCHAR(16) NULL,
    [UserMail] VARCHAR(64) NULL,
    [UserPwd] NVARCHAR(128) NULL,
    [Created] TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    [Updated] TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    [Deleted] INT DEFAULT 0
);
INSERT INTO Users(UserNo) VALUES('');DELETE FROM Users;
-- Set identity sequence number
UPDATE sqlite_sequence SET seq = 100000 WHERE name = 'Users';
-- Initialize a data
INSERT INTO Users(UserNo,UserName,UserMobile,UserMail,UserPwd) VALUES('1103397760','Admin','','qunzhi.ouyang@thermofisher.com','');

-- Create table for WXMembers
CREATE TABLE IF NOT EXISTS [WXMembers] (
    [MembersId] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    [UserId] INTEGER NOT NULL,
    [OpenId] VARCHAR(32) NOT NULL,
    [NickName] NVARCHAR(32) NULL,
    [HeadImgUrl] NVARCHAR(255) NULL,
    [Birthday] TIMESTAMP NULL,
    [SEX] INT DEFAULT 0,
    [Country] NVARCHAR(16) NULL,
    [Province] NVARCHAR(16) NULL,
    [City] NVARCHAR(16) NULL,
    [District] NVARCHAR(16) NULL,
    [Privilege] NVARCHAR(255) NULL,
    [UnionId] VARCHAR(32) NULL,
    [Language] VARCHAR(32) NULL,
    [Subscribe] INT DEFAULT 0,
    [SubscribeTime] BIGINT DEFAULT 0,
    [Remark] NVARCHAR(64) NULL,
    [GroupId] INT DEFAULT 0,
    [TagIdList] VARCHAR(120) NULL,
    [Created] TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    [Updated] TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    [Deleted] INT DEFAULT 0
);
INSERT INTO WXMembers(UserId,OpenId) VALUES(100000,'1103397760');DELETE FROM WXMembers;
-- Set identity sequence number
UPDATE sqlite_sequence SET seq = 200000 WHERE name = 'WXMembers';
INSERT INTO WXMembers(UserId,OpenId,NickName) VALUES(100000,'1103397760','Admin');

";
            try
            {
                int init = mapper.Execute(sql_create_table_users);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }


        }
    }
}