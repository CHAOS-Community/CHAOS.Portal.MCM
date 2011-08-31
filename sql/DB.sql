USE [master]
GO
/****** Object:  Database [MCM]    Script Date: 08/31/2011 18:00:49 ******/
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
/****** Object:  ForeignKey [FK_AccessProvider_Destination]    Script Date: 08/31/2011 18:00:52 ******/
ALTER TABLE [dbo].[AccessProvider] DROP CONSTRAINT [FK_AccessProvider_Destination]
GO
/****** Object:  ForeignKey [FK_Folder_Folder]    Script Date: 08/31/2011 18:00:53 ******/
ALTER TABLE [dbo].[Folder] DROP CONSTRAINT [FK_Folder_Folder]
GO
/****** Object:  ForeignKey [FK_Folder_FolderType]    Script Date: 08/31/2011 18:00:53 ******/
ALTER TABLE [dbo].[Folder] DROP CONSTRAINT [FK_Folder_FolderType]
GO
/****** Object:  ForeignKey [FK_FormatCategory_FormatType]    Script Date: 08/31/2011 18:00:53 ******/
ALTER TABLE [dbo].[FormatCategory] DROP CONSTRAINT [FK_FormatCategory_FormatType]
GO
/****** Object:  ForeignKey [FK_Object_ObjectType]    Script Date: 08/31/2011 18:00:55 ******/
ALTER TABLE [dbo].[Object] DROP CONSTRAINT [FK_Object_ObjectType]
GO
/****** Object:  ForeignKey [FK_MetadataSchema_User_Join_MetadataSchema]    Script Date: 08/31/2011 18:00:55 ******/
ALTER TABLE [dbo].[MetadataSchema_User_Join] DROP CONSTRAINT [FK_MetadataSchema_User_Join_MetadataSchema]
GO
/****** Object:  ForeignKey [FK_MetadataSchema_Group_Join_MetadataSchema]    Script Date: 08/31/2011 18:00:55 ******/
ALTER TABLE [dbo].[MetadataSchema_Group_Join] DROP CONSTRAINT [FK_MetadataSchema_Group_Join_MetadataSchema]
GO
/****** Object:  ForeignKey [FK_Metadata_Language]    Script Date: 08/31/2011 18:00:55 ******/
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [FK_Metadata_Language]
GO
/****** Object:  ForeignKey [FK_Metadata_MetadataSchema]    Script Date: 08/31/2011 18:00:55 ******/
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [FK_Metadata_MetadataSchema]
GO
/****** Object:  ForeignKey [FK_Metadata_Object]    Script Date: 08/31/2011 18:00:55 ******/
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [FK_Metadata_Object]
GO
/****** Object:  ForeignKey [FK_Object_Object_Join_Object]    Script Date: 08/31/2011 18:00:55 ******/
ALTER TABLE [dbo].[Object_Object_Join] DROP CONSTRAINT [FK_Object_Object_Join_Object]
GO
/****** Object:  ForeignKey [FK_Object_Object_Join_Object1]    Script Date: 08/31/2011 18:00:55 ******/
ALTER TABLE [dbo].[Object_Object_Join] DROP CONSTRAINT [FK_Object_Object_Join_Object1]
GO
/****** Object:  ForeignKey [FK_Object_Object_Join_ObjectRelationType]    Script Date: 08/31/2011 18:00:55 ******/
ALTER TABLE [dbo].[Object_Object_Join] DROP CONSTRAINT [FK_Object_Object_Join_ObjectRelationType]
GO
/****** Object:  ForeignKey [FK_Object_Folder_Join_Folder]    Script Date: 08/31/2011 18:00:55 ******/
ALTER TABLE [dbo].[Object_Folder_Join] DROP CONSTRAINT [FK_Object_Folder_Join_Folder]
GO
/****** Object:  ForeignKey [FK_Object_Folder_Join_Object]    Script Date: 08/31/2011 18:00:55 ******/
ALTER TABLE [dbo].[Object_Folder_Join] DROP CONSTRAINT [FK_Object_Folder_Join_Object]
GO
/****** Object:  ForeignKey [FK_Format_FormatCategory]    Script Date: 08/31/2011 18:00:55 ******/
ALTER TABLE [dbo].[Format] DROP CONSTRAINT [FK_Format_FormatCategory]
GO
/****** Object:  ForeignKey [FK_Folder_User_Join_Folder]    Script Date: 08/31/2011 18:00:56 ******/
ALTER TABLE [dbo].[Folder_User_Join] DROP CONSTRAINT [FK_Folder_User_Join_Folder]
GO
/****** Object:  ForeignKey [FK_Folder_Group_Join_Folder]    Script Date: 08/31/2011 18:00:56 ******/
ALTER TABLE [dbo].[Folder_Group_Join] DROP CONSTRAINT [FK_Folder_Group_Join_Folder]
GO
/****** Object:  ForeignKey [FK_Conversion_Destination]    Script Date: 08/31/2011 18:00:56 ******/
ALTER TABLE [dbo].[Conversion] DROP CONSTRAINT [FK_Conversion_Destination]
GO
/****** Object:  ForeignKey [FK_Conversion_Format]    Script Date: 08/31/2011 18:00:56 ******/
ALTER TABLE [dbo].[Conversion] DROP CONSTRAINT [FK_Conversion_Format]
GO
/****** Object:  ForeignKey [FK_Conversion_FormatCategory]    Script Date: 08/31/2011 18:00:56 ******/
ALTER TABLE [dbo].[Conversion] DROP CONSTRAINT [FK_Conversion_FormatCategory]
GO
/****** Object:  ForeignKey [FK_File_Destination]    Script Date: 08/31/2011 18:00:56 ******/
ALTER TABLE [dbo].[File] DROP CONSTRAINT [FK_File_Destination]
GO
/****** Object:  ForeignKey [FK_File_File]    Script Date: 08/31/2011 18:00:56 ******/
ALTER TABLE [dbo].[File] DROP CONSTRAINT [FK_File_File]
GO
/****** Object:  ForeignKey [FK_File_Format]    Script Date: 08/31/2011 18:00:56 ******/
ALTER TABLE [dbo].[File] DROP CONSTRAINT [FK_File_Format]
GO
/****** Object:  ForeignKey [FK_File_Object]    Script Date: 08/31/2011 18:00:56 ******/
ALTER TABLE [dbo].[File] DROP CONSTRAINT [FK_File_Object]
GO
/****** Object:  ForeignKey [FK_AccessPoint_Conversion]    Script Date: 08/31/2011 18:00:56 ******/
ALTER TABLE [dbo].[AccessPoint] DROP CONSTRAINT [FK_AccessPoint_Conversion]
GO
/****** Object:  ForeignKey [FK_AccessPoint_User_Join_AccessPoint]    Script Date: 08/31/2011 18:00:56 ******/
ALTER TABLE [dbo].[AccessPoint_User_Join] DROP CONSTRAINT [FK_AccessPoint_User_Join_AccessPoint]
GO
/****** Object:  ForeignKey [FK_AccessPoint_Object_Join_AccessPoint]    Script Date: 08/31/2011 18:00:56 ******/
ALTER TABLE [dbo].[AccessPoint_Object_Join] DROP CONSTRAINT [FK_AccessPoint_Object_Join_AccessPoint]
GO
/****** Object:  ForeignKey [FK_AccessPoint_Object_Join_Object]    Script Date: 08/31/2011 18:00:56 ******/
ALTER TABLE [dbo].[AccessPoint_Object_Join] DROP CONSTRAINT [FK_AccessPoint_Object_Join_Object]
GO
/****** Object:  ForeignKey [FK_AccessPoint_Group_Join_AccessPoint]    Script Date: 08/31/2011 18:00:56 ******/
ALTER TABLE [dbo].[AccessPoint_Group_Join] DROP CONSTRAINT [FK_AccessPoint_Group_Join_AccessPoint]
GO
/****** Object:  StoredProcedure [dbo].[PopulateDefaultData]    Script Date: 08/31/2011 18:00:56 ******/
DROP PROCEDURE [dbo].[PopulateDefaultData]
GO
/****** Object:  Table [dbo].[AccessPoint_Group_Join]    Script Date: 08/31/2011 18:00:56 ******/
ALTER TABLE [dbo].[AccessPoint_Group_Join] DROP CONSTRAINT [FK_AccessPoint_Group_Join_AccessPoint]
GO
ALTER TABLE [dbo].[AccessPoint_Group_Join] DROP CONSTRAINT [DF_AccessPoint_Group_Join_DateCreated]
GO
DROP TABLE [dbo].[AccessPoint_Group_Join]
GO
/****** Object:  Table [dbo].[AccessPoint_Object_Join]    Script Date: 08/31/2011 18:00:56 ******/
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
