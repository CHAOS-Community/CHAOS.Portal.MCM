USE [master]
GO
/****** Object:  Database [MCM]    Script Date: 07/13/2011 15:46:23 ******/
CREATE DATABASE [MCM] ON  PRIMARY 
( NAME = N'MCM', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\MCM.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'MCM_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\MCM_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [MCM] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MCM].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MCM] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [MCM] SET ANSI_NULLS OFF
GO
ALTER DATABASE [MCM] SET ANSI_PADDING OFF
GO
ALTER DATABASE [MCM] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [MCM] SET ARITHABORT OFF
GO
ALTER DATABASE [MCM] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [MCM] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [MCM] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [MCM] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [MCM] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [MCM] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [MCM] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [MCM] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [MCM] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [MCM] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [MCM] SET  DISABLE_BROKER
GO
ALTER DATABASE [MCM] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [MCM] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [MCM] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [MCM] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [MCM] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [MCM] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [MCM] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [MCM] SET  READ_WRITE
GO
ALTER DATABASE [MCM] SET RECOVERY FULL
GO
ALTER DATABASE [MCM] SET  MULTI_USER
GO
ALTER DATABASE [MCM] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [MCM] SET DB_CHAINING OFF
GO
USE [MCM]
GO
/****** Object:  ForeignKey [FK_Folder_Folder]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Folder] DROP CONSTRAINT [FK_Folder_Folder]
GO
/****** Object:  ForeignKey [FK_Group_Subscription_Join_Group]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Group_Subscription_Join] DROP CONSTRAINT [FK_Group_Subscription_Join_Group]
GO
/****** Object:  ForeignKey [FK_Group_Subscription_Join_Subscription]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Group_Subscription_Join] DROP CONSTRAINT [FK_Group_Subscription_Join_Subscription]
GO
/****** Object:  ForeignKey [FK_Folder_User_Join_Folder]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Folder_User_Join] DROP CONSTRAINT [FK_Folder_User_Join_Folder]
GO
/****** Object:  ForeignKey [FK_Folder_User_Join_User]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Folder_User_Join] DROP CONSTRAINT [FK_Folder_User_Join_User]
GO
/****** Object:  ForeignKey [FK_Folder_Group_Join_Folder]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Folder_Group_Join] DROP CONSTRAINT [FK_Folder_Group_Join_Folder]
GO
/****** Object:  ForeignKey [FK_Folder_Group_Join_Group]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Folder_Group_Join] DROP CONSTRAINT [FK_Folder_Group_Join_Group]
GO
/****** Object:  ForeignKey [FK_Object_ObjectType]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Object] DROP CONSTRAINT [FK_Object_ObjectType]
GO
/****** Object:  ForeignKey [FK_Object_User]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Object] DROP CONSTRAINT [FK_Object_User]
GO
/****** Object:  ForeignKey [FK_MetadataSchema_User_Join_MetadataSchema]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[MetadataSchema_User_Join] DROP CONSTRAINT [FK_MetadataSchema_User_Join_MetadataSchema]
GO
/****** Object:  ForeignKey [FK_MetadataSchema_User_Join_User]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[MetadataSchema_User_Join] DROP CONSTRAINT [FK_MetadataSchema_User_Join_User]
GO
/****** Object:  ForeignKey [FK_MetadataSchema_Group_Join_Group]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[MetadataSchema_Group_Join] DROP CONSTRAINT [FK_MetadataSchema_Group_Join_Group]
GO
/****** Object:  ForeignKey [FK_MetadataSchema_Group_Join_MetadataSchema]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[MetadataSchema_Group_Join] DROP CONSTRAINT [FK_MetadataSchema_Group_Join_MetadataSchema]
GO
/****** Object:  ForeignKey [FK_AccessProvider_Destination]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[AccessProvider] DROP CONSTRAINT [FK_AccessProvider_Destination]
GO
/****** Object:  ForeignKey [FK_FormatCategory_FormatType]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[FormatCategory] DROP CONSTRAINT [FK_FormatCategory_FormatType]
GO
/****** Object:  ForeignKey [FK_User_Subscription_Join_Subscription]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[User_Subscription_Join] DROP CONSTRAINT [FK_User_Subscription_Join_Subscription]
GO
/****** Object:  ForeignKey [FK_User_Subscription_Join_User]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[User_Subscription_Join] DROP CONSTRAINT [FK_User_Subscription_Join_User]
GO
/****** Object:  ForeignKey [FK_User_Group_Join_Group]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[User_Group_Join] DROP CONSTRAINT [FK_User_Group_Join_Group]
GO
/****** Object:  ForeignKey [FK_User_Group_Join_User]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[User_Group_Join] DROP CONSTRAINT [FK_User_Group_Join_User]
GO
/****** Object:  ForeignKey [FK_Object_Folder_Join_Folder]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[Object_Folder_Join] DROP CONSTRAINT [FK_Object_Folder_Join_Folder]
GO
/****** Object:  ForeignKey [FK_Object_Folder_Join_Object]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[Object_Folder_Join] DROP CONSTRAINT [FK_Object_Folder_Join_Object]
GO
/****** Object:  ForeignKey [FK_Format_FormatCategory]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[Format] DROP CONSTRAINT [FK_Format_FormatCategory]
GO
/****** Object:  ForeignKey [FK_Metadata_Language]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [FK_Metadata_Language]
GO
/****** Object:  ForeignKey [FK_Metadata_MetadataSchema]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [FK_Metadata_MetadataSchema]
GO
/****** Object:  ForeignKey [FK_Metadata_Object]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [FK_Metadata_Object]
GO
/****** Object:  ForeignKey [FK_Metadata_User]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [FK_Metadata_User]
GO
/****** Object:  ForeignKey [FK_File_Destination]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[File] DROP CONSTRAINT [FK_File_Destination]
GO
/****** Object:  ForeignKey [FK_File_File]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[File] DROP CONSTRAINT [FK_File_File]
GO
/****** Object:  ForeignKey [FK_File_Format]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[File] DROP CONSTRAINT [FK_File_Format]
GO
/****** Object:  ForeignKey [FK_File_Object]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[File] DROP CONSTRAINT [FK_File_Object]
GO
/****** Object:  ForeignKey [FK_Conversion_Destination]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[Conversion] DROP CONSTRAINT [FK_Conversion_Destination]
GO
/****** Object:  ForeignKey [FK_Conversion_Format]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[Conversion] DROP CONSTRAINT [FK_Conversion_Format]
GO
/****** Object:  ForeignKey [FK_Conversion_FormatCategory]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[Conversion] DROP CONSTRAINT [FK_Conversion_FormatCategory]
GO
/****** Object:  ForeignKey [FK_AccessPoint_Conversion]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[AccessPoint] DROP CONSTRAINT [FK_AccessPoint_Conversion]
GO
/****** Object:  ForeignKey [FK_AccessPoint_Subscription]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[AccessPoint] DROP CONSTRAINT [FK_AccessPoint_Subscription]
GO
/****** Object:  ForeignKey [FK_AccessPoint_Object_Join_AccessPoint]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[AccessPoint_Object_Join] DROP CONSTRAINT [FK_AccessPoint_Object_Join_AccessPoint]
GO
/****** Object:  ForeignKey [FK_AccessPoint_Object_Join_Object]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[AccessPoint_Object_Join] DROP CONSTRAINT [FK_AccessPoint_Object_Join_Object]
GO
/****** Object:  Table [dbo].[AccessPoint_Object_Join]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[AccessPoint_Object_Join] DROP CONSTRAINT [FK_AccessPoint_Object_Join_AccessPoint]
GO
ALTER TABLE [dbo].[AccessPoint_Object_Join] DROP CONSTRAINT [FK_AccessPoint_Object_Join_Object]
GO
ALTER TABLE [dbo].[AccessPoint_Object_Join] DROP CONSTRAINT [DF_AccessPoint_Object_Join_DateCreated]
GO
ALTER TABLE [dbo].[AccessPoint_Object_Join] DROP CONSTRAINT [DF_AccessPoint_Object_Join_DateModified]
GO
DROP TABLE [dbo].[AccessPoint_Object_Join]
GO
/****** Object:  Table [dbo].[AccessPoint]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[AccessPoint] DROP CONSTRAINT [FK_AccessPoint_Conversion]
GO
ALTER TABLE [dbo].[AccessPoint] DROP CONSTRAINT [FK_AccessPoint_Subscription]
GO
ALTER TABLE [dbo].[AccessPoint] DROP CONSTRAINT [DF_AccessPoint_GUID]
GO
DROP TABLE [dbo].[AccessPoint]
GO
/****** Object:  Table [dbo].[Conversion]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[Conversion] DROP CONSTRAINT [FK_Conversion_Destination]
GO
ALTER TABLE [dbo].[Conversion] DROP CONSTRAINT [FK_Conversion_Format]
GO
ALTER TABLE [dbo].[Conversion] DROP CONSTRAINT [FK_Conversion_FormatCategory]
GO
ALTER TABLE [dbo].[Conversion] DROP CONSTRAINT [DF_Conversion_DateCreated]
GO
DROP TABLE [dbo].[Conversion]
GO
/****** Object:  Table [dbo].[File]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[File] DROP CONSTRAINT [FK_File_Destination]
GO
ALTER TABLE [dbo].[File] DROP CONSTRAINT [FK_File_File]
GO
ALTER TABLE [dbo].[File] DROP CONSTRAINT [FK_File_Format]
GO
ALTER TABLE [dbo].[File] DROP CONSTRAINT [FK_File_Object]
GO
ALTER TABLE [dbo].[File] DROP CONSTRAINT [DF_File_IsMainFile]
GO
ALTER TABLE [dbo].[File] DROP CONSTRAINT [DF_File_DateCreated]
GO
DROP TABLE [dbo].[File]
GO
/****** Object:  Table [dbo].[Metadata]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [FK_Metadata_Language]
GO
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [FK_Metadata_MetadataSchema]
GO
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [FK_Metadata_Object]
GO
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [FK_Metadata_User]
GO
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [DF_Metadata_DateCreated]
GO
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [DF_Metadata_DateModified]
GO
DROP TABLE [dbo].[Metadata]
GO
/****** Object:  Table [dbo].[Format]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[Format] DROP CONSTRAINT [FK_Format_FormatCategory]
GO
DROP TABLE [dbo].[Format]
GO
/****** Object:  Table [dbo].[Object_Folder_Join]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[Object_Folder_Join] DROP CONSTRAINT [FK_Object_Folder_Join_Folder]
GO
ALTER TABLE [dbo].[Object_Folder_Join] DROP CONSTRAINT [FK_Object_Folder_Join_Object]
GO
ALTER TABLE [dbo].[Object_Folder_Join] DROP CONSTRAINT [DF_Object_Folder_Join_DateCreated]
GO
DROP TABLE [dbo].[Object_Folder_Join]
GO
/****** Object:  Table [dbo].[User_Group_Join]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[User_Group_Join] DROP CONSTRAINT [FK_User_Group_Join_Group]
GO
ALTER TABLE [dbo].[User_Group_Join] DROP CONSTRAINT [FK_User_Group_Join_User]
GO
ALTER TABLE [dbo].[User_Group_Join] DROP CONSTRAINT [DF_User_Group_Join_DateCreated]
GO
DROP TABLE [dbo].[User_Group_Join]
GO
/****** Object:  Table [dbo].[User_Subscription_Join]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[User_Subscription_Join] DROP CONSTRAINT [FK_User_Subscription_Join_Subscription]
GO
ALTER TABLE [dbo].[User_Subscription_Join] DROP CONSTRAINT [FK_User_Subscription_Join_User]
GO
ALTER TABLE [dbo].[User_Subscription_Join] DROP CONSTRAINT [DF_User_Subscription_Join_DateCreated]
GO
DROP TABLE [dbo].[User_Subscription_Join]
GO
/****** Object:  Table [dbo].[FormatCategory]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[FormatCategory] DROP CONSTRAINT [FK_FormatCategory_FormatType]
GO
DROP TABLE [dbo].[FormatCategory]
GO
/****** Object:  Table [dbo].[AccessProvider]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[AccessProvider] DROP CONSTRAINT [FK_AccessProvider_Destination]
GO
DROP TABLE [dbo].[AccessProvider]
GO
/****** Object:  Table [dbo].[MetadataSchema_Group_Join]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[MetadataSchema_Group_Join] DROP CONSTRAINT [FK_MetadataSchema_Group_Join_Group]
GO
ALTER TABLE [dbo].[MetadataSchema_Group_Join] DROP CONSTRAINT [FK_MetadataSchema_Group_Join_MetadataSchema]
GO
ALTER TABLE [dbo].[MetadataSchema_Group_Join] DROP CONSTRAINT [DF_MetadataSchema_Group_Join_DateCreated]
GO
DROP TABLE [dbo].[MetadataSchema_Group_Join]
GO
/****** Object:  Table [dbo].[MetadataSchema_User_Join]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[MetadataSchema_User_Join] DROP CONSTRAINT [FK_MetadataSchema_User_Join_MetadataSchema]
GO
ALTER TABLE [dbo].[MetadataSchema_User_Join] DROP CONSTRAINT [FK_MetadataSchema_User_Join_User]
GO
ALTER TABLE [dbo].[MetadataSchema_User_Join] DROP CONSTRAINT [DF_MetadataSchema_User_Join_DateCreated]
GO
DROP TABLE [dbo].[MetadataSchema_User_Join]
GO
/****** Object:  Table [dbo].[Object]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Object] DROP CONSTRAINT [FK_Object_ObjectType]
GO
ALTER TABLE [dbo].[Object] DROP CONSTRAINT [FK_Object_User]
GO
ALTER TABLE [dbo].[Object] DROP CONSTRAINT [DF_Object_GUID]
GO
ALTER TABLE [dbo].[Object] DROP CONSTRAINT [DF_Object_DateCreated]
GO
DROP TABLE [dbo].[Object]
GO
/****** Object:  Table [dbo].[Folder_Group_Join]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Folder_Group_Join] DROP CONSTRAINT [FK_Folder_Group_Join_Folder]
GO
ALTER TABLE [dbo].[Folder_Group_Join] DROP CONSTRAINT [FK_Folder_Group_Join_Group]
GO
ALTER TABLE [dbo].[Folder_Group_Join] DROP CONSTRAINT [DF_Folder_Group_Join_DateCreated]
GO
DROP TABLE [dbo].[Folder_Group_Join]
GO
/****** Object:  Table [dbo].[Folder_User_Join]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Folder_User_Join] DROP CONSTRAINT [FK_Folder_User_Join_Folder]
GO
ALTER TABLE [dbo].[Folder_User_Join] DROP CONSTRAINT [FK_Folder_User_Join_User]
GO
ALTER TABLE [dbo].[Folder_User_Join] DROP CONSTRAINT [DF_Folder_User_Join_DateCreated]
GO
DROP TABLE [dbo].[Folder_User_Join]
GO
/****** Object:  Table [dbo].[Group_Subscription_Join]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Group_Subscription_Join] DROP CONSTRAINT [FK_Group_Subscription_Join_Group]
GO
ALTER TABLE [dbo].[Group_Subscription_Join] DROP CONSTRAINT [FK_Group_Subscription_Join_Subscription]
GO
ALTER TABLE [dbo].[Group_Subscription_Join] DROP CONSTRAINT [DF_Group_Subscription_Join_DateCreated]
GO
DROP TABLE [dbo].[Group_Subscription_Join]
GO
/****** Object:  Table [dbo].[Language]    Script Date: 07/13/2011 15:46:26 ******/
DROP TABLE [dbo].[Language]
GO
/****** Object:  Table [dbo].[Folder]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Folder] DROP CONSTRAINT [FK_Folder_Folder]
GO
ALTER TABLE [dbo].[Folder] DROP CONSTRAINT [DF_Folder_DateCreated]
GO
DROP TABLE [dbo].[Folder]
GO
/****** Object:  Table [dbo].[MetadataSchema]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[MetadataSchema] DROP CONSTRAINT [DF_MetadataSchema_GUID]
GO
DROP TABLE [dbo].[MetadataSchema]
GO
/****** Object:  Table [dbo].[Destination]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Destination] DROP CONSTRAINT [DF_Destination_DateCreated]
GO
DROP TABLE [dbo].[Destination]
GO
/****** Object:  Table [dbo].[FormatType]    Script Date: 07/13/2011 15:46:26 ******/
DROP TABLE [dbo].[FormatType]
GO
/****** Object:  Table [dbo].[Group]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Group] DROP CONSTRAINT [DF_Group_GUID]
GO
ALTER TABLE [dbo].[Group] DROP CONSTRAINT [DF_Group_DateCreated]
GO
DROP TABLE [dbo].[Group]
GO
/****** Object:  Table [dbo].[ObjectType]    Script Date: 07/13/2011 15:46:26 ******/
DROP TABLE [dbo].[ObjectType]
GO
/****** Object:  Table [dbo].[Subscription]    Script Date: 07/13/2011 15:46:26 ******/
DROP TABLE [dbo].[Subscription]
GO
/****** Object:  Table [dbo].[User]    Script Date: 07/13/2011 15:46:25 ******/
ALTER TABLE [dbo].[User] DROP CONSTRAINT [DF_User_CreateDate]
GO
DROP TABLE [dbo].[User]
GO
/****** Object:  Table [dbo].[User]    Script Date: 07/13/2011 15:46:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GUID] [uniqueidentifier] NOT NULL,
	[DateCreated] [smalldatetime] NOT NULL CONSTRAINT [DF_User_CreateDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [QK_User_GUID_A] UNIQUE NONCLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subscription]    Script Date: 07/13/2011 15:46:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subscription](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GUID] [uniqueidentifier] NOT NULL,
	[DateCreated] [nchar](10) NULL,
 CONSTRAINT [PK_Repository] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObjectType]    Script Date: 07/13/2011 15:46:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ObjectType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Value] [varchar](255) NOT NULL,
 CONSTRAINT [PK_ObjectType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Group]    Script Date: 07/13/2011 15:46:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Group](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GUID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Group_GUID]  DEFAULT (newid()),
	[Name] [varchar](255) NOT NULL,
	[DateCreated] [smalldatetime] NOT NULL CONSTRAINT [DF_Group_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [QK_Group_GUID_A] ON [dbo].[Group] 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FormatType]    Script Date: 07/13/2011 15:46:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FormatType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Value] [varchar](255) NOT NULL,
 CONSTRAINT [PK_FormatType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Destination]    Script Date: 07/13/2011 15:46:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Destination](
	[ID] [int] NOT NULL,
	[Title] [varchar](255) NOT NULL,
	[DateCreated] [smalldatetime] NOT NULL CONSTRAINT [DF_Destination_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_Destination] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MetadataSchema]    Script Date: 07/13/2011 15:46:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MetadataSchema](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GUID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_MetadataSchema_GUID]  DEFAULT (newid()),
	[name] [varchar](255) NOT NULL,
	[SchemaXml] [xml] NOT NULL,
	[OwnerUserID] [int] NULL,
 CONSTRAINT [PK_MetadataSchema] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [QK_MetadataSchema_GUID_A] UNIQUE NONCLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Folder]    Script Date: 07/13/2011 15:46:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Folder](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL,
	[FolderTypeID] [int] NOT NULL,
	[Title] [varchar](255) NOT NULL,
	[DateCreated] [smalldatetime] NOT NULL CONSTRAINT [DF_Folder_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_Folder] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Language]    Script Date: 07/13/2011 15:46:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Language](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[LanguageCode] [varchar](10) NOT NULL,
	[CountryName] [varchar](255) NOT NULL,
 CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Group_Subscription_Join]    Script Date: 07/13/2011 15:46:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Group_Subscription_Join](
	[GroupID] [int] NOT NULL,
	[SubscriptionID] [int] NOT NULL,
	[Permission] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_Group_Subscription_Join_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_Group_Subscription_Join] PRIMARY KEY CLUSTERED 
(
	[GroupID] ASC,
	[SubscriptionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Folder_User_Join]    Script Date: 07/13/2011 15:46:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Folder_User_Join](
	[FolderID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[Permission] [int] NOT NULL,
	[DateCreated] [smalldatetime] NOT NULL CONSTRAINT [DF_Folder_User_Join_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_Folder_User_Join] PRIMARY KEY CLUSTERED 
(
	[FolderID] ASC,
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Folder_Group_Join]    Script Date: 07/13/2011 15:46:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Folder_Group_Join](
	[FolderID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[Permission] [int] NOT NULL,
	[DateCreated] [smalldatetime] NOT NULL CONSTRAINT [DF_Folder_Group_Join_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_Folder_Group_Join] PRIMARY KEY CLUSTERED 
(
	[FolderID] ASC,
	[GroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Object]    Script Date: 07/13/2011 15:46:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Object](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GUID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Object_GUID]  DEFAULT (newid()),
	[ObjectTypeID] [int] NOT NULL,
	[OwnerUserID] [int] NOT NULL,
	[MainFileID] [int] NOT NULL,
	[DateCreated] [smalldatetime] NOT NULL CONSTRAINT [DF_Object_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_Object] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [QK_Object_GUID_A] UNIQUE NONCLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MetadataSchema_User_Join]    Script Date: 07/13/2011 15:46:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MetadataSchema_User_Join](
	[MetadataSchemaID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[Permission] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_MetadataSchema_User_Join_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_MetadataSchema_User_Join] PRIMARY KEY CLUSTERED 
(
	[MetadataSchemaID] ASC,
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MetadataSchema_Group_Join]    Script Date: 07/13/2011 15:46:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MetadataSchema_Group_Join](
	[MetadataSchemaID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[Permission] [int] NULL,
	[DateCreated] [smalldatetime] NOT NULL CONSTRAINT [DF_MetadataSchema_Group_Join_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_MetadataSchema_Group_Join] PRIMARY KEY CLUSTERED 
(
	[MetadataSchemaID] ASC,
	[GroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccessProvider]    Script Date: 07/13/2011 15:46:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AccessProvider](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DestinationID] [int] NOT NULL,
	[BasePath] [varchar](max) NOT NULL,
	[StringFormat] [varchar](max) NOT NULL,
	[DateCreated] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_AccessProvider] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FormatCategory]    Script Date: 07/13/2011 15:46:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FormatCategory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FormatTypeID] [int] NOT NULL,
	[Value] [varchar](255) NOT NULL,
 CONSTRAINT [PK_FormatCategory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User_Subscription_Join]    Script Date: 07/13/2011 15:46:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User_Subscription_Join](
	[UserID] [int] NOT NULL,
	[SubscriptionID] [int] NOT NULL,
	[Permission] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_User_Subscription_Join_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_User_Subscription_Join] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[SubscriptionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User_Group_Join]    Script Date: 07/13/2011 15:46:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User_Group_Join](
	[UserID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[Permission] [int] NOT NULL,
	[DateCreated] [smalldatetime] NOT NULL CONSTRAINT [DF_User_Group_Join_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_User_Group_Join] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[GroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Object_Folder_Join]    Script Date: 07/13/2011 15:46:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Object_Folder_Join](
	[ObjectID] [int] NOT NULL,
	[FolderID] [int] NOT NULL,
	[IsShortcut] [bit] NOT NULL,
	[DateCreated] [smalldatetime] NOT NULL CONSTRAINT [DF_Object_Folder_Join_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_Object_Folder_Join] PRIMARY KEY CLUSTERED 
(
	[ObjectID] ASC,
	[FolderID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Format]    Script Date: 07/13/2011 15:46:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Format](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FormatCategoryID] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[FormatXml] [xml] NULL,
	[MimeType] [varchar](255) NOT NULL,
	[FileExtension] [varchar](255) NOT NULL,
 CONSTRAINT [PK_Format] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Metadata]    Script Date: 07/13/2011 15:46:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Metadata](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ObjectID] [int] NOT NULL,
	[LanguageID] [int] NULL,
	[MetadataSchemaID] [int] NOT NULL,
	[MetadataXml] [xml] NOT NULL,
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_Metadata_DateCreated]  DEFAULT (getdate()),
	[DateModified] [datetime] NOT NULL CONSTRAINT [DF_Metadata_DateModified]  DEFAULT (getdate()),
	[DateLocked] [datetime] NOT NULL,
	[LockUserID] [int] NULL,
 CONSTRAINT [PK_Metadata] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[File]    Script Date: 07/13/2011 15:46:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[File](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ObjectID] [int] NOT NULL,
	[ParentID] [int] NULL,
	[FormatID] [int] NOT NULL,
	[DestinationID] [int] NOT NULL,
	[Filename] [varchar](max) NOT NULL,
	[OriginalFilename] [varchar](max) NOT NULL,
	[FolderPath] [varchar](max) NOT NULL,
	[IsMainFile] [bit] NOT NULL CONSTRAINT [DF_File_IsMainFile]  DEFAULT ((0)),
	[DateCreated] [smalldatetime] NOT NULL CONSTRAINT [DF_File_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_File] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Conversion]    Script Date: 07/13/2011 15:46:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Conversion](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AccessPointID] [int] NOT NULL,
	[FormatCategoryID] [int] NOT NULL,
	[FormatID] [int] NOT NULL,
	[DestinationID] [int] NOT NULL,
	[DateCreated] [smalldatetime] NOT NULL CONSTRAINT [DF_Conversion_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_Conversion] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_AccessPointID_FormatCategoryID_FormatID_DestinationID_AAAA] ON [dbo].[Conversion] 
(
	[AccessPointID] ASC,
	[FormatCategoryID] ASC,
	[FormatID] ASC,
	[DestinationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccessPoint]    Script Date: 07/13/2011 15:46:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AccessPoint](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SubscriptionID] [int] NOT NULL,
	[GUID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_AccessPoint_GUID]  DEFAULT (newid()),
	[Name] [varchar](255) NOT NULL,
 CONSTRAINT [PK_AccessPoint] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [QK_AccessPoint_GUID_A] UNIQUE NONCLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AccessPoint_Object_Join]    Script Date: 07/13/2011 15:46:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccessPoint_Object_Join](
	[AccessPointID] [int] NOT NULL,
	[ObjectID] [int] NOT NULL,
	[ModifyingUserID] [int] NOT NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_AccessPoint_Object_Join_DateCreated]  DEFAULT (getdate()),
	[DateModified] [datetime] NOT NULL CONSTRAINT [DF_AccessPoint_Object_Join_DateModified]  DEFAULT (getdate()),
 CONSTRAINT [PK_AccessPoint_Object_Join] PRIMARY KEY CLUSTERED 
(
	[AccessPointID] ASC,
	[ObjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_Folder_Folder]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Folder]  WITH CHECK ADD  CONSTRAINT [FK_Folder_Folder] FOREIGN KEY([ParentID])
REFERENCES [dbo].[Folder] ([ID])
GO
ALTER TABLE [dbo].[Folder] CHECK CONSTRAINT [FK_Folder_Folder]
GO
/****** Object:  ForeignKey [FK_Group_Subscription_Join_Group]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Group_Subscription_Join]  WITH CHECK ADD  CONSTRAINT [FK_Group_Subscription_Join_Group] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Group] ([ID])
GO
ALTER TABLE [dbo].[Group_Subscription_Join] CHECK CONSTRAINT [FK_Group_Subscription_Join_Group]
GO
/****** Object:  ForeignKey [FK_Group_Subscription_Join_Subscription]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Group_Subscription_Join]  WITH CHECK ADD  CONSTRAINT [FK_Group_Subscription_Join_Subscription] FOREIGN KEY([SubscriptionID])
REFERENCES [dbo].[Subscription] ([ID])
GO
ALTER TABLE [dbo].[Group_Subscription_Join] CHECK CONSTRAINT [FK_Group_Subscription_Join_Subscription]
GO
/****** Object:  ForeignKey [FK_Folder_User_Join_Folder]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Folder_User_Join]  WITH CHECK ADD  CONSTRAINT [FK_Folder_User_Join_Folder] FOREIGN KEY([FolderID])
REFERENCES [dbo].[Folder] ([ID])
GO
ALTER TABLE [dbo].[Folder_User_Join] CHECK CONSTRAINT [FK_Folder_User_Join_Folder]
GO
/****** Object:  ForeignKey [FK_Folder_User_Join_User]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Folder_User_Join]  WITH CHECK ADD  CONSTRAINT [FK_Folder_User_Join_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[Folder_User_Join] CHECK CONSTRAINT [FK_Folder_User_Join_User]
GO
/****** Object:  ForeignKey [FK_Folder_Group_Join_Folder]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Folder_Group_Join]  WITH CHECK ADD  CONSTRAINT [FK_Folder_Group_Join_Folder] FOREIGN KEY([FolderID])
REFERENCES [dbo].[Folder] ([ID])
GO
ALTER TABLE [dbo].[Folder_Group_Join] CHECK CONSTRAINT [FK_Folder_Group_Join_Folder]
GO
/****** Object:  ForeignKey [FK_Folder_Group_Join_Group]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Folder_Group_Join]  WITH CHECK ADD  CONSTRAINT [FK_Folder_Group_Join_Group] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Group] ([ID])
GO
ALTER TABLE [dbo].[Folder_Group_Join] CHECK CONSTRAINT [FK_Folder_Group_Join_Group]
GO
/****** Object:  ForeignKey [FK_Object_ObjectType]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Object]  WITH CHECK ADD  CONSTRAINT [FK_Object_ObjectType] FOREIGN KEY([ObjectTypeID])
REFERENCES [dbo].[ObjectType] ([ID])
GO
ALTER TABLE [dbo].[Object] CHECK CONSTRAINT [FK_Object_ObjectType]
GO
/****** Object:  ForeignKey [FK_Object_User]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[Object]  WITH CHECK ADD  CONSTRAINT [FK_Object_User] FOREIGN KEY([OwnerUserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[Object] CHECK CONSTRAINT [FK_Object_User]
GO
/****** Object:  ForeignKey [FK_MetadataSchema_User_Join_MetadataSchema]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[MetadataSchema_User_Join]  WITH CHECK ADD  CONSTRAINT [FK_MetadataSchema_User_Join_MetadataSchema] FOREIGN KEY([MetadataSchemaID])
REFERENCES [dbo].[MetadataSchema] ([ID])
GO
ALTER TABLE [dbo].[MetadataSchema_User_Join] CHECK CONSTRAINT [FK_MetadataSchema_User_Join_MetadataSchema]
GO
/****** Object:  ForeignKey [FK_MetadataSchema_User_Join_User]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[MetadataSchema_User_Join]  WITH CHECK ADD  CONSTRAINT [FK_MetadataSchema_User_Join_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[MetadataSchema_User_Join] CHECK CONSTRAINT [FK_MetadataSchema_User_Join_User]
GO
/****** Object:  ForeignKey [FK_MetadataSchema_Group_Join_Group]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[MetadataSchema_Group_Join]  WITH CHECK ADD  CONSTRAINT [FK_MetadataSchema_Group_Join_Group] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Group] ([ID])
GO
ALTER TABLE [dbo].[MetadataSchema_Group_Join] CHECK CONSTRAINT [FK_MetadataSchema_Group_Join_Group]
GO
/****** Object:  ForeignKey [FK_MetadataSchema_Group_Join_MetadataSchema]    Script Date: 07/13/2011 15:46:26 ******/
ALTER TABLE [dbo].[MetadataSchema_Group_Join]  WITH CHECK ADD  CONSTRAINT [FK_MetadataSchema_Group_Join_MetadataSchema] FOREIGN KEY([MetadataSchemaID])
REFERENCES [dbo].[MetadataSchema] ([ID])
GO
ALTER TABLE [dbo].[MetadataSchema_Group_Join] CHECK CONSTRAINT [FK_MetadataSchema_Group_Join_MetadataSchema]
GO
/****** Object:  ForeignKey [FK_AccessProvider_Destination]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[AccessProvider]  WITH CHECK ADD  CONSTRAINT [FK_AccessProvider_Destination] FOREIGN KEY([DestinationID])
REFERENCES [dbo].[Destination] ([ID])
GO
ALTER TABLE [dbo].[AccessProvider] CHECK CONSTRAINT [FK_AccessProvider_Destination]
GO
/****** Object:  ForeignKey [FK_FormatCategory_FormatType]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[FormatCategory]  WITH CHECK ADD  CONSTRAINT [FK_FormatCategory_FormatType] FOREIGN KEY([FormatTypeID])
REFERENCES [dbo].[FormatType] ([ID])
GO
ALTER TABLE [dbo].[FormatCategory] CHECK CONSTRAINT [FK_FormatCategory_FormatType]
GO
/****** Object:  ForeignKey [FK_User_Subscription_Join_Subscription]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[User_Subscription_Join]  WITH CHECK ADD  CONSTRAINT [FK_User_Subscription_Join_Subscription] FOREIGN KEY([SubscriptionID])
REFERENCES [dbo].[Subscription] ([ID])
GO
ALTER TABLE [dbo].[User_Subscription_Join] CHECK CONSTRAINT [FK_User_Subscription_Join_Subscription]
GO
/****** Object:  ForeignKey [FK_User_Subscription_Join_User]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[User_Subscription_Join]  WITH CHECK ADD  CONSTRAINT [FK_User_Subscription_Join_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[User_Subscription_Join] CHECK CONSTRAINT [FK_User_Subscription_Join_User]
GO
/****** Object:  ForeignKey [FK_User_Group_Join_Group]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[User_Group_Join]  WITH CHECK ADD  CONSTRAINT [FK_User_Group_Join_Group] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Group] ([ID])
GO
ALTER TABLE [dbo].[User_Group_Join] CHECK CONSTRAINT [FK_User_Group_Join_Group]
GO
/****** Object:  ForeignKey [FK_User_Group_Join_User]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[User_Group_Join]  WITH CHECK ADD  CONSTRAINT [FK_User_Group_Join_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[User_Group_Join] CHECK CONSTRAINT [FK_User_Group_Join_User]
GO
/****** Object:  ForeignKey [FK_Object_Folder_Join_Folder]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[Object_Folder_Join]  WITH CHECK ADD  CONSTRAINT [FK_Object_Folder_Join_Folder] FOREIGN KEY([FolderID])
REFERENCES [dbo].[Folder] ([ID])
GO
ALTER TABLE [dbo].[Object_Folder_Join] CHECK CONSTRAINT [FK_Object_Folder_Join_Folder]
GO
/****** Object:  ForeignKey [FK_Object_Folder_Join_Object]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[Object_Folder_Join]  WITH CHECK ADD  CONSTRAINT [FK_Object_Folder_Join_Object] FOREIGN KEY([ObjectID])
REFERENCES [dbo].[Object] ([ID])
GO
ALTER TABLE [dbo].[Object_Folder_Join] CHECK CONSTRAINT [FK_Object_Folder_Join_Object]
GO
/****** Object:  ForeignKey [FK_Format_FormatCategory]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[Format]  WITH CHECK ADD  CONSTRAINT [FK_Format_FormatCategory] FOREIGN KEY([FormatCategoryID])
REFERENCES [dbo].[FormatCategory] ([ID])
GO
ALTER TABLE [dbo].[Format] CHECK CONSTRAINT [FK_Format_FormatCategory]
GO
/****** Object:  ForeignKey [FK_Metadata_Language]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[Metadata]  WITH CHECK ADD  CONSTRAINT [FK_Metadata_Language] FOREIGN KEY([LanguageID])
REFERENCES [dbo].[Language] ([ID])
GO
ALTER TABLE [dbo].[Metadata] CHECK CONSTRAINT [FK_Metadata_Language]
GO
/****** Object:  ForeignKey [FK_Metadata_MetadataSchema]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[Metadata]  WITH CHECK ADD  CONSTRAINT [FK_Metadata_MetadataSchema] FOREIGN KEY([MetadataSchemaID])
REFERENCES [dbo].[MetadataSchema] ([ID])
GO
ALTER TABLE [dbo].[Metadata] CHECK CONSTRAINT [FK_Metadata_MetadataSchema]
GO
/****** Object:  ForeignKey [FK_Metadata_Object]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[Metadata]  WITH CHECK ADD  CONSTRAINT [FK_Metadata_Object] FOREIGN KEY([ObjectID])
REFERENCES [dbo].[Object] ([ID])
GO
ALTER TABLE [dbo].[Metadata] CHECK CONSTRAINT [FK_Metadata_Object]
GO
/****** Object:  ForeignKey [FK_Metadata_User]    Script Date: 07/13/2011 15:46:27 ******/
ALTER TABLE [dbo].[Metadata]  WITH CHECK ADD  CONSTRAINT [FK_Metadata_User] FOREIGN KEY([LockUserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[Metadata] CHECK CONSTRAINT [FK_Metadata_User]
GO
/****** Object:  ForeignKey [FK_File_Destination]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[File]  WITH CHECK ADD  CONSTRAINT [FK_File_Destination] FOREIGN KEY([DestinationID])
REFERENCES [dbo].[Destination] ([ID])
GO
ALTER TABLE [dbo].[File] CHECK CONSTRAINT [FK_File_Destination]
GO
/****** Object:  ForeignKey [FK_File_File]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[File]  WITH CHECK ADD  CONSTRAINT [FK_File_File] FOREIGN KEY([ParentID])
REFERENCES [dbo].[File] ([ID])
GO
ALTER TABLE [dbo].[File] CHECK CONSTRAINT [FK_File_File]
GO
/****** Object:  ForeignKey [FK_File_Format]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[File]  WITH CHECK ADD  CONSTRAINT [FK_File_Format] FOREIGN KEY([FormatID])
REFERENCES [dbo].[Format] ([ID])
GO
ALTER TABLE [dbo].[File] CHECK CONSTRAINT [FK_File_Format]
GO
/****** Object:  ForeignKey [FK_File_Object]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[File]  WITH CHECK ADD  CONSTRAINT [FK_File_Object] FOREIGN KEY([ObjectID])
REFERENCES [dbo].[Object] ([ID])
GO
ALTER TABLE [dbo].[File] CHECK CONSTRAINT [FK_File_Object]
GO
/****** Object:  ForeignKey [FK_Conversion_Destination]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[Conversion]  WITH CHECK ADD  CONSTRAINT [FK_Conversion_Destination] FOREIGN KEY([DestinationID])
REFERENCES [dbo].[Destination] ([ID])
GO
ALTER TABLE [dbo].[Conversion] CHECK CONSTRAINT [FK_Conversion_Destination]
GO
/****** Object:  ForeignKey [FK_Conversion_Format]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[Conversion]  WITH CHECK ADD  CONSTRAINT [FK_Conversion_Format] FOREIGN KEY([FormatID])
REFERENCES [dbo].[Format] ([ID])
GO
ALTER TABLE [dbo].[Conversion] CHECK CONSTRAINT [FK_Conversion_Format]
GO
/****** Object:  ForeignKey [FK_Conversion_FormatCategory]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[Conversion]  WITH CHECK ADD  CONSTRAINT [FK_Conversion_FormatCategory] FOREIGN KEY([FormatCategoryID])
REFERENCES [dbo].[FormatCategory] ([ID])
GO
ALTER TABLE [dbo].[Conversion] CHECK CONSTRAINT [FK_Conversion_FormatCategory]
GO
/****** Object:  ForeignKey [FK_AccessPoint_Conversion]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[AccessPoint]  WITH CHECK ADD  CONSTRAINT [FK_AccessPoint_Conversion] FOREIGN KEY([ID])
REFERENCES [dbo].[Conversion] ([ID])
GO
ALTER TABLE [dbo].[AccessPoint] CHECK CONSTRAINT [FK_AccessPoint_Conversion]
GO
/****** Object:  ForeignKey [FK_AccessPoint_Subscription]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[AccessPoint]  WITH CHECK ADD  CONSTRAINT [FK_AccessPoint_Subscription] FOREIGN KEY([SubscriptionID])
REFERENCES [dbo].[Subscription] ([ID])
GO
ALTER TABLE [dbo].[AccessPoint] CHECK CONSTRAINT [FK_AccessPoint_Subscription]
GO
/****** Object:  ForeignKey [FK_AccessPoint_Object_Join_AccessPoint]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[AccessPoint_Object_Join]  WITH CHECK ADD  CONSTRAINT [FK_AccessPoint_Object_Join_AccessPoint] FOREIGN KEY([AccessPointID])
REFERENCES [dbo].[AccessPoint] ([ID])
GO
ALTER TABLE [dbo].[AccessPoint_Object_Join] CHECK CONSTRAINT [FK_AccessPoint_Object_Join_AccessPoint]
GO
/****** Object:  ForeignKey [FK_AccessPoint_Object_Join_Object]    Script Date: 07/13/2011 15:46:28 ******/
ALTER TABLE [dbo].[AccessPoint_Object_Join]  WITH CHECK ADD  CONSTRAINT [FK_AccessPoint_Object_Join_Object] FOREIGN KEY([ObjectID])
REFERENCES [dbo].[Object] ([ID])
GO
ALTER TABLE [dbo].[AccessPoint_Object_Join] CHECK CONSTRAINT [FK_AccessPoint_Object_Join_Object]
GO
