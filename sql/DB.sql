USE [master]
GO
/****** Object:  Database [MCM]    Script Date: 09/06/2011 17:22:03 ******/
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
/****** Object:  ForeignKey [FK_Object_ObjectType]    Script Date: 09/06/2011 17:22:06 ******/
ALTER TABLE [dbo].[Object] DROP CONSTRAINT [FK_Object_ObjectType]
GO
/****** Object:  ForeignKey [FK_MetadataSchema_User_Join_MetadataSchema]    Script Date: 09/06/2011 17:22:06 ******/
ALTER TABLE [dbo].[MetadataSchema_User_Join] DROP CONSTRAINT [FK_MetadataSchema_User_Join_MetadataSchema]
GO
/****** Object:  ForeignKey [FK_MetadataSchema_Group_Join_MetadataSchema]    Script Date: 09/06/2011 17:22:07 ******/
ALTER TABLE [dbo].[MetadataSchema_Group_Join] DROP CONSTRAINT [FK_MetadataSchema_Group_Join_MetadataSchema]
GO
/****** Object:  ForeignKey [FK_AccessProvider_Destination]    Script Date: 09/06/2011 17:22:07 ******/
ALTER TABLE [dbo].[AccessProvider] DROP CONSTRAINT [FK_AccessProvider_Destination]
GO
/****** Object:  ForeignKey [FK_Folder_Folder]    Script Date: 09/06/2011 17:22:07 ******/
ALTER TABLE [dbo].[Folder] DROP CONSTRAINT [FK_Folder_Folder]
GO
/****** Object:  ForeignKey [FK_Folder_FolderType]    Script Date: 09/06/2011 17:22:07 ******/
ALTER TABLE [dbo].[Folder] DROP CONSTRAINT [FK_Folder_FolderType]
GO
/****** Object:  ForeignKey [FK_FormatCategory_FormatType]    Script Date: 09/06/2011 17:22:07 ******/
ALTER TABLE [dbo].[FormatCategory] DROP CONSTRAINT [FK_FormatCategory_FormatType]
GO
/****** Object:  ForeignKey [FK_Format_FormatCategory]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Format] DROP CONSTRAINT [FK_Format_FormatCategory]
GO
/****** Object:  ForeignKey [FK_Folder_User_Join_Folder]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Folder_User_Join] DROP CONSTRAINT [FK_Folder_User_Join_Folder]
GO
/****** Object:  ForeignKey [FK_Folder_Group_Join_Folder]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Folder_Group_Join] DROP CONSTRAINT [FK_Folder_Group_Join_Folder]
GO
/****** Object:  ForeignKey [FK_Object_Object_Join_Object]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Object_Object_Join] DROP CONSTRAINT [FK_Object_Object_Join_Object]
GO
/****** Object:  ForeignKey [FK_Object_Object_Join_Object1]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Object_Object_Join] DROP CONSTRAINT [FK_Object_Object_Join_Object1]
GO
/****** Object:  ForeignKey [FK_Object_Object_Join_ObjectRelationType]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Object_Object_Join] DROP CONSTRAINT [FK_Object_Object_Join_ObjectRelationType]
GO
/****** Object:  ForeignKey [FK_Object_Folder_Join_Folder]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Object_Folder_Join] DROP CONSTRAINT [FK_Object_Folder_Join_Folder]
GO
/****** Object:  ForeignKey [FK_Object_Folder_Join_Object]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Object_Folder_Join] DROP CONSTRAINT [FK_Object_Folder_Join_Object]
GO
/****** Object:  ForeignKey [FK_Metadata_Language]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [FK_Metadata_Language]
GO
/****** Object:  ForeignKey [FK_Metadata_MetadataSchema]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [FK_Metadata_MetadataSchema]
GO
/****** Object:  ForeignKey [FK_Metadata_Object]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [FK_Metadata_Object]
GO
/****** Object:  ForeignKey [FK_Conversion_Destination]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[Conversion] DROP CONSTRAINT [FK_Conversion_Destination]
GO
/****** Object:  ForeignKey [FK_Conversion_Format]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[Conversion] DROP CONSTRAINT [FK_Conversion_Format]
GO
/****** Object:  ForeignKey [FK_Conversion_FormatCategory]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[Conversion] DROP CONSTRAINT [FK_Conversion_FormatCategory]
GO
/****** Object:  ForeignKey [FK_File_Destination]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[File] DROP CONSTRAINT [FK_File_Destination]
GO
/****** Object:  ForeignKey [FK_File_File]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[File] DROP CONSTRAINT [FK_File_File]
GO
/****** Object:  ForeignKey [FK_File_Format]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[File] DROP CONSTRAINT [FK_File_Format]
GO
/****** Object:  ForeignKey [FK_File_Object]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[File] DROP CONSTRAINT [FK_File_Object]
GO
/****** Object:  ForeignKey [FK_AccessPoint_Conversion]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[AccessPoint] DROP CONSTRAINT [FK_AccessPoint_Conversion]
GO
/****** Object:  ForeignKey [FK_AccessPoint_User_Join_AccessPoint]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[AccessPoint_User_Join] DROP CONSTRAINT [FK_AccessPoint_User_Join_AccessPoint]
GO
/****** Object:  ForeignKey [FK_AccessPoint_Object_Join_AccessPoint]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[AccessPoint_Object_Join] DROP CONSTRAINT [FK_AccessPoint_Object_Join_AccessPoint]
GO
/****** Object:  ForeignKey [FK_AccessPoint_Object_Join_Object]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[AccessPoint_Object_Join] DROP CONSTRAINT [FK_AccessPoint_Object_Join_Object]
GO
/****** Object:  ForeignKey [FK_AccessPoint_Group_Join_AccessPoint]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[AccessPoint_Group_Join] DROP CONSTRAINT [FK_AccessPoint_Group_Join_AccessPoint]
GO
/****** Object:  StoredProcedure [dbo].[PopulateDefaultData]    Script Date: 09/06/2011 17:22:09 ******/
DROP PROCEDURE [dbo].[PopulateDefaultData]
GO
/****** Object:  Table [dbo].[AccessPoint_Group_Join]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[AccessPoint_Group_Join] DROP CONSTRAINT [FK_AccessPoint_Group_Join_AccessPoint]
GO
ALTER TABLE [dbo].[AccessPoint_Group_Join] DROP CONSTRAINT [DF_AccessPoint_Group_Join_DateCreated]
GO
DROP TABLE [dbo].[AccessPoint_Group_Join]
GO
/****** Object:  Table [dbo].[AccessPoint_Object_Join]    Script Date: 09/06/2011 17:22:09 ******/
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
/****** Object:  Table [dbo].[AccessPoint_User_Join]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[AccessPoint_User_Join] DROP CONSTRAINT [FK_AccessPoint_User_Join_AccessPoint]
GO
ALTER TABLE [dbo].[AccessPoint_User_Join] DROP CONSTRAINT [DF_AccessPoint_User_Join_DateCreated]
GO
DROP TABLE [dbo].[AccessPoint_User_Join]
GO
/****** Object:  Table [dbo].[AccessPoint]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[AccessPoint] DROP CONSTRAINT [FK_AccessPoint_Conversion]
GO
ALTER TABLE [dbo].[AccessPoint] DROP CONSTRAINT [DF_AccessPoint_GUID]
GO
ALTER TABLE [dbo].[AccessPoint] DROP CONSTRAINT [DF_AccessPoint_DateCreated]
GO
DROP TABLE [dbo].[AccessPoint]
GO
/****** Object:  StoredProcedure [dbo].[Folder_Create]    Script Date: 09/06/2011 17:22:09 ******/
DROP PROCEDURE [dbo].[Folder_Create]
GO
/****** Object:  StoredProcedure [dbo].[Folder_Delete]    Script Date: 09/06/2011 17:22:09 ******/
DROP PROCEDURE [dbo].[Folder_Delete]
GO
/****** Object:  StoredProcedure [dbo].[Folder_Update]    Script Date: 09/06/2011 17:22:09 ******/
DROP PROCEDURE [dbo].[Folder_Update]
GO
/****** Object:  StoredProcedure [dbo].[FolderInfo_Get]    Script Date: 09/06/2011 17:22:09 ******/
DROP PROCEDURE [dbo].[FolderInfo_Get]
GO
/****** Object:  UserDefinedFunction [dbo].[Folder_FindHighestUserPermission]    Script Date: 09/06/2011 17:22:09 ******/
DROP FUNCTION [dbo].[Folder_FindHighestUserPermission]
GO
/****** Object:  Table [dbo].[File]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[File] DROP CONSTRAINT [FK_File_Destination]
GO
ALTER TABLE [dbo].[File] DROP CONSTRAINT [FK_File_File]
GO
ALTER TABLE [dbo].[File] DROP CONSTRAINT [FK_File_Format]
GO
ALTER TABLE [dbo].[File] DROP CONSTRAINT [FK_File_Object]
GO
ALTER TABLE [dbo].[File] DROP CONSTRAINT [DF_File_DateCreated]
GO
DROP TABLE [dbo].[File]
GO
/****** Object:  Table [dbo].[Conversion]    Script Date: 09/06/2011 17:22:09 ******/
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
/****** Object:  UserDefinedFunction [dbo].[Folder_IsFolderHighestLevel]    Script Date: 09/06/2011 17:22:08 ******/
DROP FUNCTION [dbo].[Folder_IsFolderHighestLevel]
GO
/****** Object:  StoredProcedure [dbo].[FolderType_Create]    Script Date: 09/06/2011 17:22:08 ******/
DROP PROCEDURE [dbo].[FolderType_Create]
GO
/****** Object:  StoredProcedure [dbo].[FolderType_Delete]    Script Date: 09/06/2011 17:22:08 ******/
DROP PROCEDURE [dbo].[FolderType_Delete]
GO
/****** Object:  StoredProcedure [dbo].[FormatType_Create]    Script Date: 09/06/2011 17:22:08 ******/
DROP PROCEDURE [dbo].[FormatType_Create]
GO
/****** Object:  StoredProcedure [dbo].[FormatType_Delete]    Script Date: 09/06/2011 17:22:08 ******/
DROP PROCEDURE [dbo].[FormatType_Delete]
GO
/****** Object:  StoredProcedure [dbo].[Language_Create]    Script Date: 09/06/2011 17:22:08 ******/
DROP PROCEDURE [dbo].[Language_Create]
GO
/****** Object:  StoredProcedure [dbo].[Language_Delete]    Script Date: 09/06/2011 17:22:08 ******/
DROP PROCEDURE [dbo].[Language_Delete]
GO
/****** Object:  StoredProcedure [dbo].[Language_Update]    Script Date: 09/06/2011 17:22:08 ******/
DROP PROCEDURE [dbo].[Language_Update]
GO
/****** Object:  Table [dbo].[Metadata]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [FK_Metadata_Language]
GO
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [FK_Metadata_MetadataSchema]
GO
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [FK_Metadata_Object]
GO
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [DF_Metadata_DateCreated]
GO
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [DF_Metadata_DateModified]
GO
DROP TABLE [dbo].[Metadata]
GO
/****** Object:  Table [dbo].[Object_Folder_Join]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Object_Folder_Join] DROP CONSTRAINT [FK_Object_Folder_Join_Folder]
GO
ALTER TABLE [dbo].[Object_Folder_Join] DROP CONSTRAINT [FK_Object_Folder_Join_Object]
GO
ALTER TABLE [dbo].[Object_Folder_Join] DROP CONSTRAINT [DF_Object_Folder_Join_DateCreated]
GO
DROP TABLE [dbo].[Object_Folder_Join]
GO
/****** Object:  Table [dbo].[Object_Object_Join]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Object_Object_Join] DROP CONSTRAINT [FK_Object_Object_Join_Object]
GO
ALTER TABLE [dbo].[Object_Object_Join] DROP CONSTRAINT [FK_Object_Object_Join_Object1]
GO
ALTER TABLE [dbo].[Object_Object_Join] DROP CONSTRAINT [FK_Object_Object_Join_ObjectRelationType]
GO
ALTER TABLE [dbo].[Object_Object_Join] DROP CONSTRAINT [DF_Object_Object_Join_DateCreated]
GO
DROP TABLE [dbo].[Object_Object_Join]
GO
/****** Object:  StoredProcedure [dbo].[FormatType_Update]    Script Date: 09/06/2011 17:22:08 ******/
DROP PROCEDURE [dbo].[FormatType_Update]
GO
/****** Object:  Table [dbo].[Folder_Group_Join]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Folder_Group_Join] DROP CONSTRAINT [FK_Folder_Group_Join_Folder]
GO
ALTER TABLE [dbo].[Folder_Group_Join] DROP CONSTRAINT [DF_Folder_Group_Join_DateCreated]
GO
DROP TABLE [dbo].[Folder_Group_Join]
GO
/****** Object:  Table [dbo].[Folder_User_Join]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Folder_User_Join] DROP CONSTRAINT [FK_Folder_User_Join_Folder]
GO
ALTER TABLE [dbo].[Folder_User_Join] DROP CONSTRAINT [DF_Folder_User_Join_DateCreated]
GO
DROP TABLE [dbo].[Folder_User_Join]
GO
/****** Object:  View [dbo].[FolderInfo]    Script Date: 09/06/2011 17:22:08 ******/
DROP VIEW [dbo].[FolderInfo]
GO
/****** Object:  StoredProcedure [dbo].[FolderType_Update]    Script Date: 09/06/2011 17:22:08 ******/
DROP PROCEDURE [dbo].[FolderType_Update]
GO
/****** Object:  Table [dbo].[Format]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Format] DROP CONSTRAINT [FK_Format_FormatCategory]
GO
DROP TABLE [dbo].[Format]
GO
/****** Object:  StoredProcedure [dbo].[ObjectRelationType_Update]    Script Date: 09/06/2011 17:22:07 ******/
DROP PROCEDURE [dbo].[ObjectRelationType_Update]
GO
/****** Object:  StoredProcedure [dbo].[ObjectRelationType_Create]    Script Date: 09/06/2011 17:22:07 ******/
DROP PROCEDURE [dbo].[ObjectRelationType_Create]
GO
/****** Object:  StoredProcedure [dbo].[ObjectRelationType_Delete]    Script Date: 09/06/2011 17:22:07 ******/
DROP PROCEDURE [dbo].[ObjectRelationType_Delete]
GO
/****** Object:  StoredProcedure [dbo].[ObjectType_Delete]    Script Date: 09/06/2011 17:22:07 ******/
DROP PROCEDURE [dbo].[ObjectType_Delete]
GO
/****** Object:  StoredProcedure [dbo].[ObjectType_Insert]    Script Date: 09/06/2011 17:22:07 ******/
DROP PROCEDURE [dbo].[ObjectType_Insert]
GO
/****** Object:  StoredProcedure [dbo].[ObjectType_Update]    Script Date: 09/06/2011 17:22:07 ******/
DROP PROCEDURE [dbo].[ObjectType_Update]
GO
/****** Object:  StoredProcedure [dbo].[ObjectType_Get]    Script Date: 09/06/2011 17:22:07 ******/
DROP PROCEDURE [dbo].[ObjectType_Get]
GO
/****** Object:  StoredProcedure [dbo].[ObjectRelationType_Get]    Script Date: 09/06/2011 17:22:07 ******/
DROP PROCEDURE [dbo].[ObjectRelationType_Get]
GO
/****** Object:  Table [dbo].[FormatCategory]    Script Date: 09/06/2011 17:22:07 ******/
ALTER TABLE [dbo].[FormatCategory] DROP CONSTRAINT [FK_FormatCategory_FormatType]
GO
DROP TABLE [dbo].[FormatCategory]
GO
/****** Object:  Table [dbo].[Folder]    Script Date: 09/06/2011 17:22:07 ******/
ALTER TABLE [dbo].[Folder] DROP CONSTRAINT [FK_Folder_Folder]
GO
ALTER TABLE [dbo].[Folder] DROP CONSTRAINT [FK_Folder_FolderType]
GO
ALTER TABLE [dbo].[Folder] DROP CONSTRAINT [DF_Folder_DateCreated]
GO
DROP TABLE [dbo].[Folder]
GO
/****** Object:  Table [dbo].[AccessProvider]    Script Date: 09/06/2011 17:22:07 ******/
ALTER TABLE [dbo].[AccessProvider] DROP CONSTRAINT [FK_AccessProvider_Destination]
GO
DROP TABLE [dbo].[AccessProvider]
GO
/****** Object:  UserDefinedFunction [dbo].[GetPermissionForAction]    Script Date: 09/06/2011 17:22:07 ******/
DROP FUNCTION [dbo].[GetPermissionForAction]
GO
/****** Object:  Table [dbo].[MetadataSchema_Group_Join]    Script Date: 09/06/2011 17:22:07 ******/
ALTER TABLE [dbo].[MetadataSchema_Group_Join] DROP CONSTRAINT [FK_MetadataSchema_Group_Join_MetadataSchema]
GO
ALTER TABLE [dbo].[MetadataSchema_Group_Join] DROP CONSTRAINT [DF_MetadataSchema_Group_Join_DateCreated]
GO
DROP TABLE [dbo].[MetadataSchema_Group_Join]
GO
/****** Object:  Table [dbo].[MetadataSchema_User_Join]    Script Date: 09/06/2011 17:22:06 ******/
ALTER TABLE [dbo].[MetadataSchema_User_Join] DROP CONSTRAINT [FK_MetadataSchema_User_Join_MetadataSchema]
GO
ALTER TABLE [dbo].[MetadataSchema_User_Join] DROP CONSTRAINT [DF_MetadataSchema_User_Join_DateCreated]
GO
DROP TABLE [dbo].[MetadataSchema_User_Join]
GO
/****** Object:  Table [dbo].[Object]    Script Date: 09/06/2011 17:22:06 ******/
ALTER TABLE [dbo].[Object] DROP CONSTRAINT [FK_Object_ObjectType]
GO
ALTER TABLE [dbo].[Object] DROP CONSTRAINT [DF_Object_GUID]
GO
ALTER TABLE [dbo].[Object] DROP CONSTRAINT [DF_Object_DateCreated]
GO
DROP TABLE [dbo].[Object]
GO
/****** Object:  StoredProcedure [dbo].[Language_Get]    Script Date: 09/06/2011 17:22:06 ******/
DROP PROCEDURE [dbo].[Language_Get]
GO
/****** Object:  StoredProcedure [dbo].[FormatType_Get]    Script Date: 09/06/2011 17:22:06 ******/
DROP PROCEDURE [dbo].[FormatType_Get]
GO
/****** Object:  StoredProcedure [dbo].[FolderType_Get]    Script Date: 09/06/2011 17:22:06 ******/
DROP PROCEDURE [dbo].[FolderType_Get]
GO
/****** Object:  Table [dbo].[MetadataSchema]    Script Date: 09/06/2011 17:22:06 ******/
ALTER TABLE [dbo].[MetadataSchema] DROP CONSTRAINT [DF_MetadataSchema_GUID]
GO
ALTER TABLE [dbo].[MetadataSchema] DROP CONSTRAINT [DF_MetadataSchema_DateCreated]
GO
DROP TABLE [dbo].[MetadataSchema]
GO
/****** Object:  Table [dbo].[ObjectRelationType]    Script Date: 09/06/2011 17:22:06 ******/
DROP TABLE [dbo].[ObjectRelationType]
GO
/****** Object:  UserDefinedTableType [dbo].[GUIDList]    Script Date: 09/06/2011 17:22:05 ******/
DROP TYPE [dbo].[GUIDList]
GO
/****** Object:  Table [dbo].[Language]    Script Date: 09/06/2011 17:22:05 ******/
DROP TABLE [dbo].[Language]
GO
/****** Object:  Table [dbo].[Destination]    Script Date: 09/06/2011 17:22:05 ******/
ALTER TABLE [dbo].[Destination] DROP CONSTRAINT [DF_Destination_DateCreated]
GO
DROP TABLE [dbo].[Destination]
GO
/****** Object:  Table [dbo].[FormatType]    Script Date: 09/06/2011 17:22:05 ******/
DROP TABLE [dbo].[FormatType]
GO
/****** Object:  Table [dbo].[FolderType]    Script Date: 09/06/2011 17:22:05 ******/
ALTER TABLE [dbo].[FolderType] DROP CONSTRAINT [DF_FolderType_DateCreated]
GO
DROP TABLE [dbo].[FolderType]
GO
/****** Object:  Table [dbo].[Permission]    Script Date: 09/06/2011 17:22:05 ******/
DROP TABLE [dbo].[Permission]
GO
/****** Object:  Table [dbo].[ObjectType]    Script Date: 09/06/2011 17:22:05 ******/
DROP TABLE [dbo].[ObjectType]
GO
/****** Object:  Table [dbo].[ObjectType]    Script Date: 09/06/2011 17:22:05 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [QK_ObjectType_Value_A] UNIQUE NONCLUSTERED 
(
	[Value] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Permission]    Script Date: 09/06/2011 17:22:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Permission](
	[TableIdentifier] [varchar](16) NOT NULL,
	[RightName] [varchar](64) NOT NULL,
	[Permission] [binary](4) NOT NULL,
	[Description] [varchar](max) NULL,
 CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED 
(
	[TableIdentifier] ASC,
	[Permission] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FolderType]    Script Date: 09/06/2011 17:22:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FolderType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_FolderType_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_FolderType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FormatType]    Script Date: 09/06/2011 17:22:05 ******/
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
/****** Object:  Table [dbo].[Destination]    Script Date: 09/06/2011 17:22:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Destination](
	[ID] [int] NOT NULL,
	[SubscriptionGUID] [uniqueidentifier] NOT NULL,
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
/****** Object:  Table [dbo].[Language]    Script Date: 09/06/2011 17:22:05 ******/
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
/****** Object:  UserDefinedTableType [dbo].[GUIDList]    Script Date: 09/06/2011 17:22:05 ******/
CREATE TYPE [dbo].[GUIDList] AS TABLE(
	[GUID] [uniqueidentifier] NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  Table [dbo].[ObjectRelationType]    Script Date: 09/06/2011 17:22:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ObjectRelationType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Value] [varchar](max) NOT NULL,
 CONSTRAINT [PK_ObjectRelationType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MetadataSchema]    Script Date: 09/06/2011 17:22:06 ******/
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
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_MetadataSchema_DateCreated]  DEFAULT (getdate()),
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
/****** Object:  StoredProcedure [dbo].[FolderType_Get]    Script Date: 09/06/2011 17:22:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.23
--				This SP is used to get folder
-- =============================================
Create PROCEDURE [dbo].[FolderType_Get]
	@ID		int				= null,
	@Name	varchar(255)	= null
AS
BEGIN

	SELECT	*
	  FROM	FolderType
	 WHERE	(@ID IS NULL OR ID = @ID) AND 
			(@Name IS NULL OR Name = @Name)
			
END
GO
/****** Object:  StoredProcedure [dbo].[FormatType_Get]    Script Date: 09/06/2011 17:22:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.23
--				This SP is used to get folder
-- =============================================
CREATE PROCEDURE [dbo].[FormatType_Get]
	@ID		int				= null,
	@Value	varchar(255)	= null
AS
BEGIN

	SELECT	*
	  FROM	FormatType
	 WHERE	(@ID IS NULL OR ID = @ID) AND 
			(@Value IS NULL OR Value = @Value)
			
END
GO
/****** Object:  StoredProcedure [dbo].[Language_Get]    Script Date: 09/06/2011 17:22:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.23
--				This SP is used to get Languages
-- =============================================
CREATE PROCEDURE [dbo].[Language_Get]
	@ID				int				= null,
	@Name			varchar(255)	= null,
	@LanguageCode	varchar(10)		= null,
	@CountryName	varchar(255)	= null
AS
BEGIN

	SET NOCOUNT ON;

    SELECT	*
      FROM	[Language]
     WHERE	(@ID IS NULL OR ID = @ID) AND
			(@Name IS NULL OR Name = @Name) AND
			(@LanguageCode IS NULL OR LanguageCode = @LanguageCode) AND
			(@CountryName IS NULL OR CountryName = @CountryName)
	
END
GO
/****** Object:  Table [dbo].[Object]    Script Date: 09/06/2011 17:22:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Object](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GUID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Object_GUID]  DEFAULT (newid()),
	[ObjectTypeID] [int] NOT NULL,
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
/****** Object:  Table [dbo].[MetadataSchema_User_Join]    Script Date: 09/06/2011 17:22:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MetadataSchema_User_Join](
	[MetadataSchemaID] [int] NOT NULL,
	[UserGUID] [uniqueidentifier] NOT NULL,
	[Permission] [int] NOT NULL,
	[DateCreated] [smalldatetime] NOT NULL CONSTRAINT [DF_MetadataSchema_User_Join_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_MetadataSchema_User_Join] PRIMARY KEY CLUSTERED 
(
	[MetadataSchemaID] ASC,
	[UserGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MetadataSchema_Group_Join]    Script Date: 09/06/2011 17:22:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MetadataSchema_Group_Join](
	[MetadataSchemaID] [int] NOT NULL,
	[GroupGUID] [uniqueidentifier] NOT NULL,
	[Permission] [int] NOT NULL,
	[DateCreated] [smalldatetime] NOT NULL CONSTRAINT [DF_MetadataSchema_Group_Join_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_MetadataSchema_Group_Join] PRIMARY KEY CLUSTERED 
(
	[MetadataSchemaID] ASC,
	[GroupGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[GetPermissionForAction]    Script Date: 09/06/2011 17:22:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2010.08.17
--				This function is used to select a permission
-- =============================================
CREATe FUNCTION [dbo].[GetPermissionForAction]
(
	@TableIdentifier	varchar(16),
	@RightName	    	varchar(64)
)
RETURNS int
AS
BEGIN
	DECLARE @Permission int

	SELECT	@Permission = [Permission].Permission
	  FROM	[Permission]
	 WHERE	[Permission].TableIdentifier = @TableIdentifier AND
			[Permission].RightName = @RightName

	RETURN @Permission

END
GO
/****** Object:  Table [dbo].[AccessProvider]    Script Date: 09/06/2011 17:22:07 ******/
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
/****** Object:  Table [dbo].[Folder]    Script Date: 09/06/2011 17:22:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Folder](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NULL,
	[FolderTypeID] [int] NOT NULL,
	[SubscriptionGUID] [uniqueidentifier] NOT NULL,
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
/****** Object:  Table [dbo].[FormatCategory]    Script Date: 09/06/2011 17:22:07 ******/
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
/****** Object:  StoredProcedure [dbo].[ObjectRelationType_Get]    Script Date: 09/06/2011 17:22:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.23
--				This SP is used to get object relation types
-- =============================================
CREATE PROCEDURE [dbo].[ObjectRelationType_Get]
	@ID		int				= null,
	@Value	varchar(255)	= null
AS
BEGIN

	SELECT	*
	  FROM	ObjectRelationType
	 WHERE	(@ID IS NULL OR ID = @ID) AND 
			(@Value IS NULL OR Value = @Value)
			
END
GO
/****** Object:  StoredProcedure [dbo].[ObjectType_Get]    Script Date: 09/06/2011 17:22:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr	Knudsen
-- Create date: 2010.08.17
--				This SP is used tp GET an ObjectType
-- =============================================
CREATE PROCEDURE [dbo].[ObjectType_Get]
	@ID		int				= null,
	@Value	varchar(255)	= null
AS
	SELECT	*
	  FROM	ObjectType
	WHERE	( @ID IS NULL OR @ID = ID ) AND
			( @Value IS NULL OR @Value = Value )
GO
/****** Object:  StoredProcedure [dbo].[ObjectType_Update]    Script Date: 09/06/2011 17:22:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.22
--				This SP is used to update an object type
-- =============================================
CREATE PROCEDURE [dbo].[ObjectType_Update]
	@ID					int,
	@Value				varchar(255),
	@SystemPermission	int
AS
BEGIN

	DECLARE @RequiredPermission INT
	SET @RequiredPermission = dbo.GetPermissionForAction( 'System', 'Manage Type' )

	IF( @RequiredPermission & @SystemPermission <> @RequiredPermission )
		RETURN -100
		
	UPDATE [ObjectType]
	   SET [Value] = @Value
	 WHERE ID = @ID
	 
	 RETURN	@@ROWCOUNT

END
GO
/****** Object:  StoredProcedure [dbo].[ObjectType_Insert]    Script Date: 09/06/2011 17:22:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr	Knudsen
-- Create date: 2010.08.17
--				This SP creates an ObjectType
-- =============================================
CREATE PROCEDURE [dbo].[ObjectType_Insert]
	@Value				varchar(255),
	@SystemPermission	int
AS

	DECLARE @RequiredPermission INT
	SET @RequiredPermission = dbo.GetPermissionForAction( 'System', 'Manage Type' )

	IF( @RequiredPermission & @SystemPermission <> @RequiredPermission )
		RETURN -100

	INSERT INTO [ObjectType] ([Value])
    VALUES ( @Value )

	RETURN @@IDENTITY
GO
/****** Object:  StoredProcedure [dbo].[ObjectType_Delete]    Script Date: 09/06/2011 17:22:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.23
--				This SP deletes an ObjectType, either ID or Value must be set
-- =============================================
CREATE PROCEDURE [dbo].[ObjectType_Delete]
	@ID					int				= null,
	@Value				varchar(255)	= null,
	@SystemPermission	int
AS
BEGIN

	IF( @ID IS NULL AND @Value IS NULL )
		RETURN -10

	DECLARE @RequiredPermission INT
	SET @RequiredPermission = dbo.GetPermissionForAction( 'System', 'Manage Type' )

	IF( @RequiredPermission & @SystemPermission <> @RequiredPermission )
		RETURN -100
		
	DELETE	[ObjectType]
	 WHERE	( @ID IS NULL OR ID = @ID ) AND
			( @Value IS NULL OR Value = @Value )
	 
	 RETURN	@@ROWCOUNT
END
GO
/****** Object:  StoredProcedure [dbo].[ObjectRelationType_Delete]    Script Date: 09/06/2011 17:22:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.23
--				This SP is used to Delete object relation types
-- =============================================
CREATE PROCEDURE [dbo].[ObjectRelationType_Delete]
	@ID					int,
	@SystemPermission	int
AS
BEGIN

	DECLARE @RequiredPermission INT
	SET @RequiredPermission = dbo.GetPermissionForAction( 'System', 'Manage Type' )

	IF( @RequiredPermission & @SystemPermission <> @RequiredPermission )
		RETURN -100

	DELETE	[ObjectRelationType]
     WHERE	ID = @ID
     
    RETURN @@ROWCOUNT
	
END
GO
/****** Object:  StoredProcedure [dbo].[ObjectRelationType_Create]    Script Date: 09/06/2011 17:22:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.23
--				This SP is used to create object relation types
-- =============================================
CREATE PROCEDURE [dbo].[ObjectRelationType_Create]
	@Value				varchar(255),
	@SystemPermission	int
AS
BEGIN

	DECLARE @RequiredPermission INT
	SET @RequiredPermission = dbo.GetPermissionForAction( 'System', 'Manage Type' )

	IF( @RequiredPermission & @SystemPermission <> @RequiredPermission )
		RETURN -100

	INSERT INTO [ObjectRelationType]
           ([Value])
     VALUES
           (@Value)
           
    RETURN @@IDENTITY
	
END
GO
/****** Object:  StoredProcedure [dbo].[ObjectRelationType_Update]    Script Date: 09/06/2011 17:22:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.23
--				This SP is used to Update object relation types
-- =============================================
CREATE PROCEDURE [dbo].[ObjectRelationType_Update]
	@ID					int,
	@Value				varchar(255),
	@SystemPermission	int
AS
BEGIN

	DECLARE @RequiredPermission INT
	SET @RequiredPermission = dbo.GetPermissionForAction( 'System', 'Manage Type' )

	IF( @RequiredPermission & @SystemPermission <> @RequiredPermission )
		RETURN -100

	UPDATE	[ObjectRelationType]
       SET	[Value] = @Value
     WHERE	ID = @ID
     
    RETURN @@ROWCOUNT
	
END
GO
/****** Object:  Table [dbo].[Format]    Script Date: 09/06/2011 17:22:08 ******/
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
/****** Object:  StoredProcedure [dbo].[FolderType_Update]    Script Date: 09/06/2011 17:22:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.23
--				This SP is used to Update folder types
-- =============================================
create PROCEDURE [dbo].[FolderType_Update]
	@ID					int,
	@Name				varchar(255),
	@SystemPermission	int
AS
BEGIN

	DECLARE @RequiredPermission INT
	SET @RequiredPermission = dbo.GetPermissionForAction( 'System', 'Manage Type' )

	IF( @RequiredPermission & @SystemPermission <> @RequiredPermission )
		RETURN -100

	UPDATE	FolderType
       SET	[Name] = @Name
     WHERE	ID = @ID
     
    RETURN @@ROWCOUNT
	
END
GO
/****** Object:  View [dbo].[FolderInfo]    Script Date: 09/06/2011 17:22:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[FolderInfo]
AS
SELECT     ID, ParentID, FolderTypeID, SubscriptionGUID, Title, DateCreated,
                          (SELECT     COUNT(*) AS Expr1
                            FROM          dbo.Folder
                            WHERE      (ParentID = f.ID)) AS NumberOfSubFolders
FROM         dbo.Folder AS f
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "f"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 215
               Right = 210
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 480
         Width = 840
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1725
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'FolderInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'FolderInfo'
GO
/****** Object:  Table [dbo].[Folder_User_Join]    Script Date: 09/06/2011 17:22:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Folder_User_Join](
	[FolderID] [int] NOT NULL,
	[UserGUID] [uniqueidentifier] NOT NULL,
	[Permission] [binary](4) NOT NULL,
	[DateCreated] [smalldatetime] NOT NULL CONSTRAINT [DF_Folder_User_Join_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_Folder_User_Join] PRIMARY KEY CLUSTERED 
(
	[FolderID] ASC,
	[UserGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Folder_Group_Join]    Script Date: 09/06/2011 17:22:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Folder_Group_Join](
	[FolderID] [int] NOT NULL,
	[GroupGUID] [uniqueidentifier] NOT NULL,
	[Permission] [binary](4) NOT NULL,
	[DateCreated] [smalldatetime] NOT NULL CONSTRAINT [DF_Folder_Group_Join_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_Folder_Group_Join] PRIMARY KEY CLUSTERED 
(
	[FolderID] ASC,
	[GroupGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[FormatType_Update]    Script Date: 09/06/2011 17:22:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.23
--				This SP is used to Update folder types
-- =============================================
CREATE PROCEDURE [dbo].[FormatType_Update]
	@ID					int,
	@Value				varchar(255),
	@SystemPermission	int
AS
BEGIN

	DECLARE @RequiredPermission INT
	SET @RequiredPermission = dbo.GetPermissionForAction( 'System', 'Manage Type' )

	IF( @RequiredPermission & @SystemPermission <> @RequiredPermission )
		RETURN -100

	UPDATE	FormatType
       SET	[Value] = @Value
     WHERE	ID = @ID
     
    RETURN @@ROWCOUNT
	
END
GO
/****** Object:  Table [dbo].[Object_Object_Join]    Script Date: 09/06/2011 17:22:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Object_Object_Join](
	[ObjectID1] [int] NOT NULL,
	[ObjectID2] [int] NOT NULL,
	[ObjectRelationTypeID] [int] NOT NULL,
	[Sequence] [int] NULL,
	[DateCreated] [smalldatetime] NOT NULL CONSTRAINT [DF_Object_Object_Join_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_Object_Object_Join] PRIMARY KEY CLUSTERED 
(
	[ObjectID1] ASC,
	[ObjectID2] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Object_Folder_Join]    Script Date: 09/06/2011 17:22:08 ******/
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
/****** Object:  Table [dbo].[Metadata]    Script Date: 09/06/2011 17:22:08 ******/
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
/****** Object:  StoredProcedure [dbo].[Language_Update]    Script Date: 09/06/2011 17:22:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.23
--				This SP Updates a language
-- =============================================
CREATE PROCEDURE [dbo].[Language_Update]
	@ID					int,
	@Name				varchar(255),
	@LanguageCode		varchar(10),
	@CountryName		varchar(255),
	@SystemPermission	int
AS
BEGIN
	
	DECLARE @RequiredPermission INT
	SET @RequiredPermission = dbo.GetPermissionForAction( 'System', 'Manage Type' )

	IF( @RequiredPermission & @SystemPermission <> @RequiredPermission )
		RETURN -100
	
	UPDATE	[Language]
	   SET	[Name]         = @Name
		   ,[LanguageCode] = @LanguageCode
		   ,[CountryName]  = @CountryName
	 WHERE	ID = @ID

	RETURN @@ROWCOUNT

END
GO
/****** Object:  StoredProcedure [dbo].[Language_Delete]    Script Date: 09/06/2011 17:22:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.23
--				This SP Deletes a language
-- =============================================
CREATE PROCEDURE [dbo].[Language_Delete]
	@ID					int,
	@SystemPermission	int
AS
BEGIN
	
	DECLARE @RequiredPermission INT
	SET @RequiredPermission = dbo.GetPermissionForAction( 'System', 'Manage Type' )

	IF( @RequiredPermission & @SystemPermission <> @RequiredPermission )
		RETURN -100
	
	DELETE	[Language]
	 WHERE	ID = @ID

	RETURN @@ROWCOUNT

END
GO
/****** Object:  StoredProcedure [dbo].[Language_Create]    Script Date: 09/06/2011 17:22:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.23
--				This SP Creates a language
-- =============================================
CREATE PROCEDURE [dbo].[Language_Create]
	@Name				varchar(255),
	@LanguageCode		varchar(10),
	@CountryName		varchar(255),
	@SystemPermission	int
AS
BEGIN
	
	DECLARE @RequiredPermission INT
	SET @RequiredPermission = dbo.GetPermissionForAction( 'System', 'Manage Type' )

	IF( @RequiredPermission & @SystemPermission <> @RequiredPermission )
		RETURN -100
	
	INSERT INTO [Language] ([Name],[LanguageCode],[CountryName])
         VALUES	(@Name, @LanguageCode, @CountryName)

	RETURN @@IDENTITY

END
GO
/****** Object:  StoredProcedure [dbo].[FormatType_Delete]    Script Date: 09/06/2011 17:22:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.23
--				This SP is used to Delete folder types
-- =============================================
CREATE PROCEDURE [dbo].[FormatType_Delete]
	@ID					int,
	@SystemPermission	int
AS
BEGIN

	DECLARE @RequiredPermission INT
	SET @RequiredPermission = dbo.GetPermissionForAction( 'System', 'Manage Type' )

	IF( @RequiredPermission & @SystemPermission <> @RequiredPermission )
		RETURN -100

	DELETE	[FormatType]
     WHERE	ID = @ID
     
    RETURN @@ROWCOUNT
	
END
GO
/****** Object:  StoredProcedure [dbo].[FormatType_Create]    Script Date: 09/06/2011 17:22:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.23
--				This SP is used to create folder types
-- =============================================
CREATE PROCEDURE [dbo].[FormatType_Create]
	@Value				varchar(255),
	@SystemPermission	int
AS
BEGIN

	DECLARE @RequiredPermission INT
	SET @RequiredPermission = dbo.GetPermissionForAction( 'System', 'Manage Type' )

	IF( @RequiredPermission & @SystemPermission <> @RequiredPermission )
		RETURN -100

	INSERT INTO FormatType
           ([Value])
     VALUES
           (@Value)
           
    RETURN @@IDENTITY
	
END
GO
/****** Object:  StoredProcedure [dbo].[FolderType_Delete]    Script Date: 09/06/2011 17:22:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.23
--				This SP is used to Delete folder types
-- =============================================
CREATE PROCEDURE [dbo].[FolderType_Delete]
	@ID					int,
	@SystemPermission	int
AS
BEGIN

	DECLARE @RequiredPermission INT
	SET @RequiredPermission = dbo.GetPermissionForAction( 'System', 'Manage Type' )

	IF( @RequiredPermission & @SystemPermission <> @RequiredPermission )
		RETURN -100

	DELETE	[FolderType]
     WHERE	ID = @ID
     
    RETURN @@ROWCOUNT
	
END
GO
/****** Object:  StoredProcedure [dbo].[FolderType_Create]    Script Date: 09/06/2011 17:22:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.23
--				This SP is used to create folder types
-- =============================================
Create PROCEDURE [dbo].[FolderType_Create]
	@Name				varchar(255),
	@SystemPermission	int
AS
BEGIN

	DECLARE @RequiredPermission INT
	SET @RequiredPermission = dbo.GetPermissionForAction( 'System', 'Manage Type' )

	IF( @RequiredPermission & @SystemPermission <> @RequiredPermission )
		RETURN -100

	INSERT INTO FolderType
           ([Name])
     VALUES
           (@Name)
           
    RETURN @@IDENTITY
	
END
GO
/****** Object:  UserDefinedFunction [dbo].[Folder_IsFolderHighestLevel]    Script Date: 09/06/2011 17:22:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.26
--				This Function return 1 if a subfolder has parent folders with permissions otherwise 0
-- =============================================
CREATE FUNCTION [dbo].[Folder_IsFolderHighestLevel]
(
	@UserGUID	uniqueidentifier,
	@GroupGUIDs	GUIDList READONLY,
	@FolderID	INT
)
RETURNS bit
AS
BEGIN

	DECLARE	@CurrentFolderID INT

	SET @CurrentFolderID = @FolderID
	
	DECLARE @Folders TABLE
	(
		FolderID	INT,
		ParentID	INT
	)
	
	INSERT INTO @Folders(FolderID,ParentID)
		SELECT	ID, ParentID
		 FROM	Folder INNER JOIN
				(
					SELECT	FolderID
					  FROM	Folder_Group_Join
					 WHERE	Folder_Group_Join.GroupGUID IN (SELECT	[GUID]
															  FROM	@GroupGUIDs)
					 UNION ALL
					SELECT	FolderID
					  FROM	Folder_User_Join
					 WHERE	Folder_User_Join.UserGUID = @UserGUID
				) as q ON Folder.ID = q.FolderID
				
	-- Check if Folder has a direct permission at all
	IF NOT EXISTS( SELECT * FROM @Folders WHERE	FolderID = @FolderID )
		RETURN 0	

	-- Check if FolderID it self is a topFolder
	IF EXISTS( SELECT * FROM @Folders WHERE	FolderID = @CurrentFolderID AND ParentID IS NULL )
		RETURN 1			

	-- Traverse through folder tree, and determine if FolderID is a parent of SubfolderID
	WHILE( @CurrentFolderID IS NOT NULL )
	BEGIN

		SELECT	@CurrentFolderID = Folder.ParentID
		  FROM	Folder 
		 WHERE	Folder.ID = @CurrentFolderID
		 
		 IF EXISTS( SELECT	FolderID
					  FROM	@Folders
					 WHERE	FolderID = @CurrentFolderID )
			RETURN 0

	END
	
	RETURN 1

END
GO
/****** Object:  Table [dbo].[Conversion]    Script Date: 09/06/2011 17:22:09 ******/
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
/****** Object:  Table [dbo].[File]    Script Date: 09/06/2011 17:22:09 ******/
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
	[DateCreated] [smalldatetime] NOT NULL CONSTRAINT [DF_File_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_File] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  UserDefinedFunction [dbo].[Folder_FindHighestUserPermission]    Script Date: 09/06/2011 17:22:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.25
--				This function given a folderID will return the User and Groups accumulated permission
-- =============================================
CREATE FUNCTION [dbo].[Folder_FindHighestUserPermission]
(
	@UserGUID	uniqueidentifier,
	@GroupGUIDs	GUIDList READONLY,
	@FolderID	INT
)
RETURNS INT
AS
BEGIN
	
	DECLARE @FolderPermissions TABLE
	(
	  FolderID int,
	  Permission int
	)
	
	-- Find all folders the users / groups has direct access to
	INSERT INTO	@FolderPermissions
		SELECT	FolderID, Permission
		   FROM	(
					SELECT	FolderID, Permission
					  FROM	Folder_Group_Join
					 WHERE	Folder_Group_Join.GroupGUID IN (SELECT	[GUID]
					                                          FROM	@GroupGUIDs)
					 UNION ALL
					SELECT	FolderID, Permission
					  FROM	Folder_User_Join
					 WHERE	Folder_User_Join.UserGUID = @UserGUID
				) AS query
	
	DECLARE	@Permission	INT
	SET @Permission = 0
	
	-- Find start folder permission
	SELECT	@Permission = Permission
	  FROM	@FolderPermissions 
	 WHERE	FolderID = @FolderID
	
	-- Traverse through folder tree, and "OR" all permissions, to find the highest
	WHILE( @FolderID IS NOT NULL )
	BEGIN

		SELECT	@FolderID = Folder.ParentID
		  FROM	Folder 
		 WHERE	Folder.ID = @FolderID
		                      
		SELECT	@Permission = Permission | @Permission
		  FROM	@FolderPermissions
		  WHERE	FolderID = @FolderID
		  
	END

	RETURN @Permission

END
GO
/****** Object:  StoredProcedure [dbo].[FolderInfo_Get]    Script Date: 09/06/2011 17:22:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.26
--				This SP is used to get a FolderInfo, if no search criteria are given it will find the top most folders
-- =============================================
CREATE PROCEDURE [dbo].[FolderInfo_Get] 
	@GroupGUIDs		GUIDList READONLY,
	@UserGUID		uniqueidentifier,
	@FolderID		int = null,
	@FolderTypeID	int = null,
	@ParentID		int = null
AS
BEGIN

	DECLARE	@RequiredPermission	int
	SET @RequiredPermission = dbo.GetPermissionForAction( 'Folder', 'GET' )
	
	IF( @FolderID IS NULL AND @ParentID IS NULL )
	BEGIN
		SELECT	*
		  FROM	FolderInfo
		 WHERE  dbo.Folder_IsFolderHighestLevel(@UserGUID,@GroupGUIDs,FolderInfo.ID) = 1 AND
				dbo.[Folder_FindHighestUserPermission] (@UserGUID,@GroupGUIDs,FolderInfo.ID) & @RequiredPermission = @RequiredPermission AND
				( @FolderTypeID IS NULL OR @FolderTypeID = FolderInfo.FolderTypeID )
	END
	ELSE
	BEGIN
		SELECT	*
		  FROM	FolderInfo
		 WHERE  dbo.[Folder_FindHighestUserPermission] (@UserGUID,@GroupGUIDs,FolderInfo.ID) & @RequiredPermission = @RequiredPermission AND
				( @FolderID IS NULL OR @FolderID = FolderInfo.ID ) AND
				( @FolderTypeID IS NULL OR @FolderTypeID = FolderInfo.FolderTypeID ) AND
				( @ParentID IS NULL OR @ParentID = FolderInfo.ParentID )
	END

END
GO
/****** Object:  StoredProcedure [dbo].[Folder_Update]    Script Date: 09/06/2011 17:22:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.09.06
--				This SP is used to update a folder
-- =============================================
CREATE PROCEDURE [dbo].[Folder_Update]
	@GroupGUIDs			GUIDList READONLY,
	@UserGUID			uniqueidentifier,
	@ID					int,
	@NewTitle			varchar(255) = null,
	@NewParentID		int          = null,
	@NewFolderTypeID	int			 = null
AS
BEGIN

	if( @NewTitle IS NULL AND @NewFolderTypeID IS NULL AND @NewParentID IS NULL )
		RETURN -10

	DECLARE	@RequiredPermission	int
	SET @RequiredPermission = 0
	
	-- OR with general UPDATE permission if applies
	if( @NewTitle IS NOT NULL OR @NewFolderTypeID IS NOT NULL )
		SET @RequiredPermission = @RequiredPermission | dbo.GetPermissionForAction( 'Folder', 'UPDATE' )
	
	-- OR with MOVE permission if applies
	if( @NewParentID IS NOT NULL )
		SET @RequiredPermission = @RequiredPermission | dbo.GetPermissionForAction( 'Folder', 'MOVE' )
	
	IF( dbo.[Folder_FindHighestUserPermission]( @UserGUID,@GroupGUIDs,@ID ) & @RequiredPermission <> @RequiredPermission ) 
		RETURN -100

	UPDATE	Folder
	   SET	[ParentID] = @NewParentID
			,[FolderTypeID] = ISNULL(@NewFolderTypeID,[FolderTypeID])
			,[Title] = ISNULL(@NewTitle,[Title])
	 WHERE	Folder.ID = @ID

	RETURN @@ROWCOUNT
	
END
GO
/****** Object:  StoredProcedure [dbo].[Folder_Delete]    Script Date: 09/06/2011 17:22:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.31
--				This SP is used to delete a folder
-- =============================================
CREATE PROCEDURE [dbo].[Folder_Delete]
	@GroupGUIDs		GUIDList READONLY,
	@UserGUID		uniqueidentifier,
	@ID				int
AS
BEGIN

	DECLARE	@RequiredPermission	int
	SET @RequiredPermission = dbo.GetPermissionForAction( 'Folder', 'DELETE' )

	IF( dbo.[Folder_FindHighestUserPermission]( @UserGUID,@GroupGUIDs,@ID ) & @RequiredPermission <> @RequiredPermission ) 
		RETURN -100

	IF EXISTS( SELECT * FROM Folder WHERE Folder.ParentID = @ID )
		RETURN -50
		
	IF EXISTS( SELECT * FROM Object_Folder_Join WHERE FolderID = @ID )
		RETURN -50
		
	BEGIN TRANSACTION Delete_Folder
	
	DELETE 
	  FROM	[Folder_Group_Join]
     WHERE	FolderID = @ID

	DELETE 
	  FROM	[Folder_User_Join]
     WHERE	FolderID = @ID
     
     DELETE 
       FROM	[Folder]
      WHERE	ID = @ID

	IF( @@ERROR = 0 )
		COMMIT TRANSACTION Delete_Folder
	ELSE
		ROLLBACK TRANSACTION Delete_Folder
		
	RETURN 1
	
END
GO
/****** Object:  StoredProcedure [dbo].[Folder_Create]    Script Date: 09/06/2011 17:22:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.09.06
--				This SP is used to create folders
-- =============================================
CREATE PROCEDURE [dbo].[Folder_Create]
	@GroupGUIDs				GUIDList READONLY,
	@UserGUID				uniqueidentifier,
	@SubscriptionGUID		uniqueidentifier	= null,
	@Title					varchar(255),
	@ParentID				int					= null,
	@FolderTypeID			int,
	@SubscriptionPermission	int					= null
AS
BEGIN

	DECLARE	@RequiredPermission	int
	
	-- If SubscriptionGUID is NOT NULL ParentID must be null, ELSE SubscriptionGUID is inherited from the parent
	IF( @SubscriptionGUID IS NULL AND @ParentID IS NULL AND @SubscriptionPermission IS NULL )
		RETURN -10
	
	IF( @ParentID IS NULL )
	BEGIN
		-- If ParentID is null, check permission on subscription
		SET @RequiredPermission = dbo.GetPermissionForAction( 'Subscription', 'CREATE TOPFOLDER' )
		
		IF( @RequiredPermission & @SubscriptionPermission <> @RequiredPermission )
			RETURN -100
	END
	ELSE
	BEGIN
		-- Check Create permission to ParentID
		SET @RequiredPermission = dbo.GetPermissionForAction( 'Folder', 'CREATE' )
		
		IF( @RequiredPermission & dbo.Folder_FindHighestUserPermission( @UserGUID,@GroupGUIDs,@ParentID ) <> @RequiredPermission )
			RETURN -100
	END

	IF( @SubscriptionGUID IS NULL AND @ParentID IS NOT NULL )
		SELECT	@SubscriptionGUID = SubscriptionGUID 
		  FROM	Folder
		 WHERE	Folder.ID = @ParentID
	
	BEGIN TRANSACTION Create_Folder
	
	INSERT INTO	[Folder] ([ParentID] ,[FolderTypeID] ,[SubscriptionGUID] ,[Title] ,[DateCreated])
         VALUES (@ParentID ,@FolderTypeID ,@SubscriptionGUID ,@Title ,GETDATE())
    
    DECLARE @FolderID INT
    SET @FolderID = @@IDENTITY       
    
    INSERT INTO [Folder_User_Join] ([FolderID],[UserGUID],[Permission],[DateCreated])
         VALUES	(@FolderID,@UserGUID,0x7FFFFFFF,GETDATE())
        
    IF( @@ERROR = 0 )
		COMMIT TRANSACTION Create_Folder
	ELSE
		ROLLBACK TRANSACTION Create_Folder

	RETURN @FolderID
END
GO
/****** Object:  Table [dbo].[AccessPoint]    Script Date: 09/06/2011 17:22:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AccessPoint](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GUID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_AccessPoint_GUID]  DEFAULT (newid()),
	[SubscriptionGUID] [uniqueidentifier] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_AccessPoint_DateCreated]  DEFAULT (getdate()),
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
/****** Object:  Table [dbo].[AccessPoint_User_Join]    Script Date: 09/06/2011 17:22:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccessPoint_User_Join](
	[AccessPointID] [int] NOT NULL,
	[UserGUID] [uniqueidentifier] NOT NULL,
	[Permission] [int] NOT NULL,
	[DateCreated] [smalldatetime] NOT NULL CONSTRAINT [DF_AccessPoint_User_Join_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_AccessPoint_User_Join] PRIMARY KEY CLUSTERED 
(
	[AccessPointID] ASC,
	[UserGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccessPoint_Object_Join]    Script Date: 09/06/2011 17:22:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccessPoint_Object_Join](
	[AccessPointID] [int] NOT NULL,
	[ObjectID] [int] NOT NULL,
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
/****** Object:  Table [dbo].[AccessPoint_Group_Join]    Script Date: 09/06/2011 17:22:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccessPoint_Group_Join](
	[AcessPointID] [int] NOT NULL,
	[GroupGUID] [uniqueidentifier] NOT NULL,
	[Permission] [int] NOT NULL,
	[DateCreated] [smalldatetime] NOT NULL CONSTRAINT [DF_AccessPoint_Group_Join_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_AccessPoint_Group_Join] PRIMARY KEY CLUSTERED 
(
	[AcessPointID] ASC,
	[GroupGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[PopulateDefaultData]    Script Date: 09/06/2011 17:22:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jesper Fyhr	Knudsen
-- Create date: 2010.08.17
--				This SP Pupulate MCM with Default data
-- =============================================
CREATE PROCEDURE [dbo].[PopulateDefaultData]

AS
	IF( 1 = 1 )
	BEGIN

		DELETE FROM AccessPoint_User_Join
		DELETE FROM AccessPoint_Group_Join
		DELETE FROM AccessPoint_Object_Join
		DELETE FROM AccessPoint
		DELETE FROM AccessProvider
		DELETE FROM Destination
		DELETE FROM [File]
		DELETE FROM Folder_Group_Join
		DELETE FROM Folder_User_Join
		DELETE FROM Folder
		DELETE FROM FolderType
		DELETE FROM Format
		DELETE FROM FormatCategory
		DELETE FROM FormatType
		DELETE FROM [Language]
		DELETE FROM Metadata
		DELETE FROM MetadataSchema_Group_Join
		DELETE FROM MetadataSchema_User_Join
		DELETE FROM MetadataSchema
		DELETE FROM Object_Folder_Join
		DELETE FROM Object_Object_Join
		DELETE FROM [Object]
		DELETE FROM ObjectType
		DELETE FROM Permission
		DELETE FROM ObjectRelationType

		DBCC CHECKIDENT ("AccessPoint", RESEED,0)
		DBCC CHECKIDENT ("AccessProvider", RESEED,0)
		DBCC CHECKIDENT ("[File]", RESEED,0)
		DBCC CHECKIDENT ("FolderType", RESEED,0)
		DBCC CHECKIDENT ("Folder", RESEED,0)
		DBCC CHECKIDENT ("Format", RESEED,0)
		DBCC CHECKIDENT ("FormatCategory", RESEED,0)
		DBCC CHECKIDENT ("FormatType", RESEED,0)
		DBCC CHECKIDENT ("[Language]", RESEED,0)
		DBCC CHECKIDENT ("Metadata", RESEED,0)
		DBCC CHECKIDENT ("MetadataSchema", RESEED,0)
		DBCC CHECKIDENT ("Object", RESEED,0)
		DBCC CHECKIDENT ("ObjectType", RESEED,0)
		DBCC CHECKIDENT ("ObjectRelationType", RESEED,0)

		INSERT INTO [Permission]([TableIdentifier],[RightName],[Permission],[Description]) VALUES ('System','Manage Type',4,'Permissoin to manage Types')
		INSERT INTO [Permission]([TableIdentifier],[RightName],[Permission],[Description]) VALUES ('Folder','GET',1,'Permissoin to GET Folders')
		INSERT INTO [Permission]([TableIdentifier],[RightName],[Permission],[Description]) VALUES ('Folder','DELETE',2,'Permissoin to DELETE Folders')
		INSERT INTO [Permission]([TableIdentifier],[RightName],[Permission],[Description]) VALUES ('Folder','UPDATE',4,'Permissoin to UPDATE Folders')
		INSERT INTO [Permission]([TableIdentifier],[RightName],[Permission],[Description]) VALUES ('Folder','MOVE',8,'Permissoin to MOVE Folders')
		INSERT INTO [Permission]([TableIdentifier],[RightName],[Permission],[Description]) VALUES ('Folder','CREATE',16,'Permissoin to Create Folders')
		INSERT INTO [Permission]([TableIdentifier],[RightName],[Permission],[Description]) VALUES ('Subscription','CREATE TOPFOLDER',16,'Permissoin to Create top folders')
		INSERT INTO [ObjectType] ([Value])VALUES ('Asset')
		INSERT INTO [ObjectRelationType]([Value])VALUES('Contains')
		INSERT INTO [FolderType]([Name],[DateCreated]) VALUES('TEST',GETDATE())
		INSERT INTO [FolderType]([Name],[DateCreated]) VALUES('Folder',GETDATE()) DECLARE @FolderTypeID INT SET @FolderTypeID = @@IDENTITY		
		INSERT INTO [FormatType]([Value])VALUES('Video')
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(null,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','Geckon',GETDATE()) DECLARE @TopFolderID INT SET @TopFolderID = @@IDENTITY
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@TopFolderID,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','Public',GETDATE())
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@TopFolderID,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','Users',GETDATE())
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@@IDENTITY,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','Private',GETDATE()) DECLARE @PrivateFolder INT SET @PrivateFolder = @@IDENTITY
		INSERT INTO [Folder_Group_Join]([FolderID],[GroupGUID],[Permission],[DateCreated])VALUES(@PrivateFolder,'A0B231E9-7D98-4F52-885E-AAAAAAAAAAAA',  0x00000001,GETDATE())
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@PrivateFolder,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','sub',GETDATE())
		INSERT INTO [Folder_Group_Join]([FolderID],[GroupGUID],[Permission],[DateCreated])VALUES(@TopFolderID,'A0B231E9-7D98-4F52-885E-AAAAAAAAAAAA',0x7FFFFFFF,GETDATE())
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(null,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','Test',GETDATE())
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@@IDENTITY,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','sub Test',GETDATE())
		INSERT INTO [Folder_Group_Join]([FolderID],[GroupGUID],[Permission],[DateCreated])VALUES(@@IDENTITY,'A0B231E9-7D98-4F52-885E-AAAAAAAAAAAA',  0x0000000E,GETDATE())
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Afrikaans','af-ZA','South Africa')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Albanian','sq-AL','Albania')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-DZ','Algeria')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-BH','Bahrain')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-EG','Egypt')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-IQ','Iraq')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-JO','Jordan')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-KW','Kuwait')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-LB','Lebanon')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-LY','Libya')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-MA','Morocco')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-OM','Oman')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-QA','Qatar')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-SA','Saudi Arabia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-SY','Syria')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-TN','Tunisia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-AE','United Arab Emirates')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-YE','Yemen')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Armenian','hy-AM','Armenia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Cyrillic','az-AZ-Cyrl','Azerbaijan')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Latin','az-AZ-Latn','Azerbaijan')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Basque','eu-ES','Basque')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Belarusian','be-BY','Belarus')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Bulgarian','bg-BG','Bulgaria')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Catalan','ca-ES','Catalan')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Chinese','zh-HK','Hong Kong SAR')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Chinese','zh-MO','Macau SAR')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Chinese','zh-CN','China')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Chinese','zh-CHS','Chinese (Simplified)')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Chinese','zh-SG','Singapore')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Chinese','zh-TW','Taiwan')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Croatian','hr-HR','Croatia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Czech','cs-CZ','Czech Republic')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Danish','da-DK','Denmark')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Dhivehi','div-MV','Maldives')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Dutch','nl-BE','Belgium')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Dutch','nl-NL','The Netherlands')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-AU','Australia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-BZ','Belize')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-CA','Canada')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-CB','Caribbean')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-IE','Ireland')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-JM','Jamaica')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-NZ','New Zealand')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-PH','Philippines')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-ZA','South Africa')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-TT','Trinidad and Tobago')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-GB','United Kingdom')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-US','United States')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-ZW','Zimbabwe')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Estonian','et-EE','Estonia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Faroese','fo-FO','Faroe Islands')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Farsi','fa-IR','Iran')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Finnish','fi-FI','Finland')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('French','fr-BE','Belgium')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('French','fr-CA','Canada')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('French','fr-FR','France')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('French','fr-LU','Luxembourg')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('French','fr-MC','Monaco')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('French','fr-CH','Switzerland')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Galician','gl-ES','Galician')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Georgian','ka-GE','Georgia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('German','de-AT','Austria')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('German','de-DE','Germany')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('German','de-LI','Liechtenstein')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('German','de-LU','Luxembourg')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('German','de-CH','Switzerland')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Greek','el-GR','Greece')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Gujarati','gu-IN','India')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Hebrew','he-IL','Israel')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Hindi','hi-IN','India')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Hungarian','hu-HU','Hungary')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Icelandic','is-IS','Iceland')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Indonesian','id-ID','Indonesia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Italian','it-IT','Italy')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Italian','it-CH','Switzerland')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Japanese','ja-JP','Japan')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Kannada','kn-IN','India')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Kazakh','kk-KZ','Kazakhstan')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Konkani','kok-IN','India')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Korean','ko-KR','Korea')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Kyrgyz','ky-KZ','Kazakhstan')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Latvian','lv-LV','Latvia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Lithuanian','lt-LT','Lithuania')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Macedonian','mk-MK','FYROM')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Malay','ms-BN','Brunei')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Malay','ms-MY','Malaysia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Marathi','mr-IN','India')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Mongolian','mn-MN','Mongolia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Bokml','nb-NO','Norway')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Nynorsk','nn-NO','Norway')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Polish','pl-PL','Polish - Poland')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Portuguese','pt-BR','Brazil')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Portuguese','pt-PT','Portugal')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Punjabi','pa-IN','India')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Romanian','ro-RO','Romania')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Russian','ru-RU','Russia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Sanskrit','sa-IN','India')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Cyrillic','sr-SP-Cyrl','Serbia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Latin','sr-SP-Latn','Serbia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Slovak','sk-SK','Slovakia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Slovenian','sl-SI','Slovenia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-AR','Argentina')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-BO','Bolivia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-CL','Chile')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-CO','Colombia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-CR','Costa Rica')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-DO','Dominican Republic')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-EC','Ecuador')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-SV','El Salvador')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-GT','Guatemala')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-HN','Honduras')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-MX','Mexico')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-NI','Nicaragua')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-PA','Panama')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-PY','Paraguay')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-PE','Peru')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-PR','Puerto Rico')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-ES','Spain')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-UY','Uruguay')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-VE','Venezuela')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Swahili','sw-KE','Kenya')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Swedish','sv-FI','Finland')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Swedish','sv-SE','Sweden')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Syriac','syr-SY','Syria')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Tamil','ta-IN','India')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Tatar','tt-RU','Russia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Telugu','te-IN','India')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Thai','th-TH','Thailand')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Turkish','tr-TR','Turkey')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Ukrainian','uk-UA','Ukraine')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Urdu','ur-PK','Pakistan')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Cyrillic','uz-UZ-Cyrl','Uzbekistan')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Latin','uz-UZ-Latn','Uzbekistan')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Vietnamese','vi-VN','Vietnam')

	END
GO
/****** Object:  ForeignKey [FK_Object_ObjectType]    Script Date: 09/06/2011 17:22:06 ******/
ALTER TABLE [dbo].[Object]  WITH CHECK ADD  CONSTRAINT [FK_Object_ObjectType] FOREIGN KEY([ObjectTypeID])
REFERENCES [dbo].[ObjectType] ([ID])
GO
ALTER TABLE [dbo].[Object] CHECK CONSTRAINT [FK_Object_ObjectType]
GO
/****** Object:  ForeignKey [FK_MetadataSchema_User_Join_MetadataSchema]    Script Date: 09/06/2011 17:22:06 ******/
ALTER TABLE [dbo].[MetadataSchema_User_Join]  WITH CHECK ADD  CONSTRAINT [FK_MetadataSchema_User_Join_MetadataSchema] FOREIGN KEY([MetadataSchemaID])
REFERENCES [dbo].[MetadataSchema] ([ID])
GO
ALTER TABLE [dbo].[MetadataSchema_User_Join] CHECK CONSTRAINT [FK_MetadataSchema_User_Join_MetadataSchema]
GO
/****** Object:  ForeignKey [FK_MetadataSchema_Group_Join_MetadataSchema]    Script Date: 09/06/2011 17:22:07 ******/
ALTER TABLE [dbo].[MetadataSchema_Group_Join]  WITH CHECK ADD  CONSTRAINT [FK_MetadataSchema_Group_Join_MetadataSchema] FOREIGN KEY([MetadataSchemaID])
REFERENCES [dbo].[MetadataSchema] ([ID])
GO
ALTER TABLE [dbo].[MetadataSchema_Group_Join] CHECK CONSTRAINT [FK_MetadataSchema_Group_Join_MetadataSchema]
GO
/****** Object:  ForeignKey [FK_AccessProvider_Destination]    Script Date: 09/06/2011 17:22:07 ******/
ALTER TABLE [dbo].[AccessProvider]  WITH CHECK ADD  CONSTRAINT [FK_AccessProvider_Destination] FOREIGN KEY([DestinationID])
REFERENCES [dbo].[Destination] ([ID])
GO
ALTER TABLE [dbo].[AccessProvider] CHECK CONSTRAINT [FK_AccessProvider_Destination]
GO
/****** Object:  ForeignKey [FK_Folder_Folder]    Script Date: 09/06/2011 17:22:07 ******/
ALTER TABLE [dbo].[Folder]  WITH CHECK ADD  CONSTRAINT [FK_Folder_Folder] FOREIGN KEY([ParentID])
REFERENCES [dbo].[Folder] ([ID])
GO
ALTER TABLE [dbo].[Folder] CHECK CONSTRAINT [FK_Folder_Folder]
GO
/****** Object:  ForeignKey [FK_Folder_FolderType]    Script Date: 09/06/2011 17:22:07 ******/
ALTER TABLE [dbo].[Folder]  WITH CHECK ADD  CONSTRAINT [FK_Folder_FolderType] FOREIGN KEY([FolderTypeID])
REFERENCES [dbo].[FolderType] ([ID])
GO
ALTER TABLE [dbo].[Folder] CHECK CONSTRAINT [FK_Folder_FolderType]
GO
/****** Object:  ForeignKey [FK_FormatCategory_FormatType]    Script Date: 09/06/2011 17:22:07 ******/
ALTER TABLE [dbo].[FormatCategory]  WITH CHECK ADD  CONSTRAINT [FK_FormatCategory_FormatType] FOREIGN KEY([FormatTypeID])
REFERENCES [dbo].[FormatType] ([ID])
GO
ALTER TABLE [dbo].[FormatCategory] CHECK CONSTRAINT [FK_FormatCategory_FormatType]
GO
/****** Object:  ForeignKey [FK_Format_FormatCategory]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Format]  WITH CHECK ADD  CONSTRAINT [FK_Format_FormatCategory] FOREIGN KEY([FormatCategoryID])
REFERENCES [dbo].[FormatCategory] ([ID])
GO
ALTER TABLE [dbo].[Format] CHECK CONSTRAINT [FK_Format_FormatCategory]
GO
/****** Object:  ForeignKey [FK_Folder_User_Join_Folder]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Folder_User_Join]  WITH CHECK ADD  CONSTRAINT [FK_Folder_User_Join_Folder] FOREIGN KEY([FolderID])
REFERENCES [dbo].[Folder] ([ID])
GO
ALTER TABLE [dbo].[Folder_User_Join] CHECK CONSTRAINT [FK_Folder_User_Join_Folder]
GO
/****** Object:  ForeignKey [FK_Folder_Group_Join_Folder]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Folder_Group_Join]  WITH CHECK ADD  CONSTRAINT [FK_Folder_Group_Join_Folder] FOREIGN KEY([FolderID])
REFERENCES [dbo].[Folder] ([ID])
GO
ALTER TABLE [dbo].[Folder_Group_Join] CHECK CONSTRAINT [FK_Folder_Group_Join_Folder]
GO
/****** Object:  ForeignKey [FK_Object_Object_Join_Object]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Object_Object_Join]  WITH CHECK ADD  CONSTRAINT [FK_Object_Object_Join_Object] FOREIGN KEY([ObjectID1])
REFERENCES [dbo].[Object] ([ID])
GO
ALTER TABLE [dbo].[Object_Object_Join] CHECK CONSTRAINT [FK_Object_Object_Join_Object]
GO
/****** Object:  ForeignKey [FK_Object_Object_Join_Object1]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Object_Object_Join]  WITH CHECK ADD  CONSTRAINT [FK_Object_Object_Join_Object1] FOREIGN KEY([ObjectID2])
REFERENCES [dbo].[Object] ([ID])
GO
ALTER TABLE [dbo].[Object_Object_Join] CHECK CONSTRAINT [FK_Object_Object_Join_Object1]
GO
/****** Object:  ForeignKey [FK_Object_Object_Join_ObjectRelationType]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Object_Object_Join]  WITH CHECK ADD  CONSTRAINT [FK_Object_Object_Join_ObjectRelationType] FOREIGN KEY([ObjectRelationTypeID])
REFERENCES [dbo].[ObjectRelationType] ([ID])
GO
ALTER TABLE [dbo].[Object_Object_Join] CHECK CONSTRAINT [FK_Object_Object_Join_ObjectRelationType]
GO
/****** Object:  ForeignKey [FK_Object_Folder_Join_Folder]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Object_Folder_Join]  WITH CHECK ADD  CONSTRAINT [FK_Object_Folder_Join_Folder] FOREIGN KEY([FolderID])
REFERENCES [dbo].[Folder] ([ID])
GO
ALTER TABLE [dbo].[Object_Folder_Join] CHECK CONSTRAINT [FK_Object_Folder_Join_Folder]
GO
/****** Object:  ForeignKey [FK_Object_Folder_Join_Object]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Object_Folder_Join]  WITH CHECK ADD  CONSTRAINT [FK_Object_Folder_Join_Object] FOREIGN KEY([ObjectID])
REFERENCES [dbo].[Object] ([ID])
GO
ALTER TABLE [dbo].[Object_Folder_Join] CHECK CONSTRAINT [FK_Object_Folder_Join_Object]
GO
/****** Object:  ForeignKey [FK_Metadata_Language]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Metadata]  WITH CHECK ADD  CONSTRAINT [FK_Metadata_Language] FOREIGN KEY([LanguageID])
REFERENCES [dbo].[Language] ([ID])
GO
ALTER TABLE [dbo].[Metadata] CHECK CONSTRAINT [FK_Metadata_Language]
GO
/****** Object:  ForeignKey [FK_Metadata_MetadataSchema]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Metadata]  WITH CHECK ADD  CONSTRAINT [FK_Metadata_MetadataSchema] FOREIGN KEY([MetadataSchemaID])
REFERENCES [dbo].[MetadataSchema] ([ID])
GO
ALTER TABLE [dbo].[Metadata] CHECK CONSTRAINT [FK_Metadata_MetadataSchema]
GO
/****** Object:  ForeignKey [FK_Metadata_Object]    Script Date: 09/06/2011 17:22:08 ******/
ALTER TABLE [dbo].[Metadata]  WITH CHECK ADD  CONSTRAINT [FK_Metadata_Object] FOREIGN KEY([ObjectID])
REFERENCES [dbo].[Object] ([ID])
GO
ALTER TABLE [dbo].[Metadata] CHECK CONSTRAINT [FK_Metadata_Object]
GO
/****** Object:  ForeignKey [FK_Conversion_Destination]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[Conversion]  WITH CHECK ADD  CONSTRAINT [FK_Conversion_Destination] FOREIGN KEY([DestinationID])
REFERENCES [dbo].[Destination] ([ID])
GO
ALTER TABLE [dbo].[Conversion] CHECK CONSTRAINT [FK_Conversion_Destination]
GO
/****** Object:  ForeignKey [FK_Conversion_Format]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[Conversion]  WITH CHECK ADD  CONSTRAINT [FK_Conversion_Format] FOREIGN KEY([FormatID])
REFERENCES [dbo].[Format] ([ID])
GO
ALTER TABLE [dbo].[Conversion] CHECK CONSTRAINT [FK_Conversion_Format]
GO
/****** Object:  ForeignKey [FK_Conversion_FormatCategory]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[Conversion]  WITH CHECK ADD  CONSTRAINT [FK_Conversion_FormatCategory] FOREIGN KEY([FormatCategoryID])
REFERENCES [dbo].[FormatCategory] ([ID])
GO
ALTER TABLE [dbo].[Conversion] CHECK CONSTRAINT [FK_Conversion_FormatCategory]
GO
/****** Object:  ForeignKey [FK_File_Destination]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[File]  WITH CHECK ADD  CONSTRAINT [FK_File_Destination] FOREIGN KEY([DestinationID])
REFERENCES [dbo].[Destination] ([ID])
GO
ALTER TABLE [dbo].[File] CHECK CONSTRAINT [FK_File_Destination]
GO
/****** Object:  ForeignKey [FK_File_File]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[File]  WITH CHECK ADD  CONSTRAINT [FK_File_File] FOREIGN KEY([ParentID])
REFERENCES [dbo].[File] ([ID])
GO
ALTER TABLE [dbo].[File] CHECK CONSTRAINT [FK_File_File]
GO
/****** Object:  ForeignKey [FK_File_Format]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[File]  WITH CHECK ADD  CONSTRAINT [FK_File_Format] FOREIGN KEY([FormatID])
REFERENCES [dbo].[Format] ([ID])
GO
ALTER TABLE [dbo].[File] CHECK CONSTRAINT [FK_File_Format]
GO
/****** Object:  ForeignKey [FK_File_Object]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[File]  WITH CHECK ADD  CONSTRAINT [FK_File_Object] FOREIGN KEY([ObjectID])
REFERENCES [dbo].[Object] ([ID])
GO
ALTER TABLE [dbo].[File] CHECK CONSTRAINT [FK_File_Object]
GO
/****** Object:  ForeignKey [FK_AccessPoint_Conversion]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[AccessPoint]  WITH CHECK ADD  CONSTRAINT [FK_AccessPoint_Conversion] FOREIGN KEY([ID])
REFERENCES [dbo].[Conversion] ([ID])
GO
ALTER TABLE [dbo].[AccessPoint] CHECK CONSTRAINT [FK_AccessPoint_Conversion]
GO
/****** Object:  ForeignKey [FK_AccessPoint_User_Join_AccessPoint]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[AccessPoint_User_Join]  WITH CHECK ADD  CONSTRAINT [FK_AccessPoint_User_Join_AccessPoint] FOREIGN KEY([AccessPointID])
REFERENCES [dbo].[AccessPoint] ([ID])
GO
ALTER TABLE [dbo].[AccessPoint_User_Join] CHECK CONSTRAINT [FK_AccessPoint_User_Join_AccessPoint]
GO
/****** Object:  ForeignKey [FK_AccessPoint_Object_Join_AccessPoint]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[AccessPoint_Object_Join]  WITH CHECK ADD  CONSTRAINT [FK_AccessPoint_Object_Join_AccessPoint] FOREIGN KEY([AccessPointID])
REFERENCES [dbo].[AccessPoint] ([ID])
GO
ALTER TABLE [dbo].[AccessPoint_Object_Join] CHECK CONSTRAINT [FK_AccessPoint_Object_Join_AccessPoint]
GO
/****** Object:  ForeignKey [FK_AccessPoint_Object_Join_Object]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[AccessPoint_Object_Join]  WITH CHECK ADD  CONSTRAINT [FK_AccessPoint_Object_Join_Object] FOREIGN KEY([ObjectID])
REFERENCES [dbo].[Object] ([ID])
GO
ALTER TABLE [dbo].[AccessPoint_Object_Join] CHECK CONSTRAINT [FK_AccessPoint_Object_Join_Object]
GO
/****** Object:  ForeignKey [FK_AccessPoint_Group_Join_AccessPoint]    Script Date: 09/06/2011 17:22:09 ******/
ALTER TABLE [dbo].[AccessPoint_Group_Join]  WITH CHECK ADD  CONSTRAINT [FK_AccessPoint_Group_Join_AccessPoint] FOREIGN KEY([AcessPointID])
REFERENCES [dbo].[AccessPoint] ([ID])
GO
ALTER TABLE [dbo].[AccessPoint_Group_Join] CHECK CONSTRAINT [FK_AccessPoint_Group_Join_AccessPoint]
GO
