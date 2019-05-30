---------
select * from [dbo].[EQ_Log]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EQ_Log]') AND type in (N'U'))
DROP TABLE [dbo].[EQ_Log]
GO
CREATE TABLE [dbo].[EQ_Log]
(
    [LogId]				[int]	Primary key IDENTITY(100000,1) NOT NULL,
    [JobId]             [int]			 NOT NULL,
    [Content]			[nvarchar](max)  not null,
    [Created]			[datetime] default(getdate())	not null,
    [Updated]			[datetime] default(getdate())	not null,
    [Deleted]			[bit] default(0) not null
)

---------
select * from [dbo].[EQ_Job]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EQ_Job]') AND type in (N'U'))
DROP TABLE [dbo].[EQ_Job]
GO
CREATE TABLE [dbo].[EQ_Job]
(
    [JobId]             [int] Primary key IDENTITY(100000,1) NOT NULL,
    [JobName]			[nvarchar](255)  not null,
    [RunTime]			[nvarchar](255)  not null,
    [JobType]           [int]			 not null,
    [Content]			[nvarchar](max)  not null,
    [IsEnabled]         [bit] default(1) not null,
    [Created]			[datetime] default(getdate())	not null,
    [Updated]			[datetime] default(getdate())	not null,
    [Deleted]			[bit] default(0) not null
)

---------
select * from [dbo].[EQ_Dict]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EQ_Dict]') AND type in (N'U'))
DROP TABLE [dbo].[EQ_Dict]
GO
CREATE TABLE [dbo].[EQ_Dict]
(
    [DictId]			[int] Primary key IDENTITY(100000,1) NOT NULL,
    [DictType]			[int]				not null,
    [Key]				[nvarchar](255)		not null,
    [Value]				[nvarchar](1500)	not null,
    [ParentId]			[int]				not null,
    [Tag]				[nvarchar](1500)	not null,
    [Created]			[datetime] default(getdate())	not null,
    [Updated]			[datetime] default(getdate())	not null,
    [Deleted]			[bit] default(0) not null
)

---------
select * from [dbo].[EQ_Email]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EQ_Email]') AND type in (N'U'))
DROP TABLE [dbo].[EQ_Email]
GO
CREATE TABLE [dbo].[EQ_Email]
(
    [EmailId]           [int] Primary key IDENTITY(100000,1) NOT NULL,
    [MailTo]			[nvarchar](1500)  not null,
    [Subject]			[nvarchar](255)   not null,
    [MailBody]          [nvarchar](max)   not null,
    [Comment]			[nvarchar](2000)   not null,
    [Created]			[datetime] default(getdate())	not null,
    [Updated]			[datetime] default(getdate())	not null,
    [Deleted]			[bit] default(0) not null
)



