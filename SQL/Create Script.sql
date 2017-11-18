/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2016 (13.0.4001)
    Source Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2017
    Target Database Engine Edition : Microsoft SQL Server Standard Edition
    Target Database Engine Type : Standalone SQL Server
*/
USE [master]
GO
/****** Object:  Database [PolishWarehouse]    Script Date: 11/17/2017 7:37:33 PM ******/
CREATE DATABASE [PolishWarehouse]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PolishWarehouse', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\PolishWarehouse.mdf' , SIZE = 28672KB , MAXSIZE = 204800KB , FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'PolishWarehouse_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\PolishWarehouse_log.ldf' , SIZE = 10240KB , MAXSIZE = 102400KB , FILEGROWTH = 1024KB )
GO
ALTER DATABASE [PolishWarehouse] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PolishWarehouse].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PolishWarehouse] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PolishWarehouse] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PolishWarehouse] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PolishWarehouse] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PolishWarehouse] SET ARITHABORT OFF 
GO
ALTER DATABASE [PolishWarehouse] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PolishWarehouse] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PolishWarehouse] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PolishWarehouse] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PolishWarehouse] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PolishWarehouse] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PolishWarehouse] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PolishWarehouse] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PolishWarehouse] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PolishWarehouse] SET  ENABLE_BROKER 
GO
ALTER DATABASE [PolishWarehouse] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PolishWarehouse] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PolishWarehouse] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PolishWarehouse] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PolishWarehouse] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PolishWarehouse] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PolishWarehouse] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PolishWarehouse] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [PolishWarehouse] SET  MULTI_USER 
GO
ALTER DATABASE [PolishWarehouse] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PolishWarehouse] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PolishWarehouse] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PolishWarehouse] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [PolishWarehouse] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'PolishWarehouse', N'ON'
GO
ALTER DATABASE [PolishWarehouse] SET QUERY_STORE = OFF
GO
USE [PolishWarehouse]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [PolishWarehouse]
GO
/****** Object:  User [PolishUser]    Script Date: 11/17/2017 7:37:33 PM ******/
CREATE USER [PolishUser] FOR LOGIN [PolishUser] WITH DEFAULT_SCHEMA=[PolishUser]
GO
/****** Object:  DatabaseRole [gd_execprocs]    Script Date: 11/17/2017 7:37:33 PM ******/
CREATE ROLE [gd_execprocs]
GO
ALTER ROLE [db_securityadmin] ADD MEMBER [PolishUser]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [PolishUser]
GO
ALTER ROLE [db_backupoperator] ADD MEMBER [PolishUser]
GO
ALTER ROLE [db_datareader] ADD MEMBER [PolishUser]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [PolishUser]
GO
/****** Object:  Schema [PolishUser]    Script Date: 11/17/2017 7:37:33 PM ******/
CREATE SCHEMA [PolishUser]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BrandCategory]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BrandCategory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Brands]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Brands](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CategoryID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Colors]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Colors](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[IsPrimary] [bit] NOT NULL,
	[IsSecondary] [bit] NOT NULL,
	[IsGlitter] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IncomingLineTypes]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IncomingLineTypes](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Timestamp] [timestamp] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IncomingOrderLines]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IncomingOrderLines](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Timestamp] [timestamp] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[IncomingOrderID] [bigint] NOT NULL,
	[IncomingLineTypeID] [bigint] NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Price] [money] NOT NULL,
	[Qty] [int] NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[Tracking] [nvarchar](max) NULL,
	[ShippingProviderID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IncomingOrderLines_Polishes]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IncomingOrderLines_Polishes](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Timestamp] [timestamp] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[IncomingOrderLinesID] [bigint] NOT NULL,
	[ColorID] [int] NULL,
	[BrandID] [int] NULL,
	[PolishName] [nvarchar](100) NOT NULL,
	[Coats] [int] NOT NULL,
	[HasBeenTried] [bit] NOT NULL,
	[WasGift] [bit] NOT NULL,
	[GiftFromName] [nvarchar](100) NULL,
	[Description] [varchar](max) NULL,
	[PolishID] [bigint] NULL,
	[Converted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IncomingOrders]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IncomingOrders](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Timestamp] [timestamp] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[PurchaseDate] [datetime] NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[Tracking] [nvarchar](max) NULL,
	[TrackingProviderID] [int] NULL,
	[Price] [money] NOT NULL,
	[OrderComplete] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Logs]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Logs](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[LogType] [varchar](5) NOT NULL,
	[Details] [nvarchar](max) NOT NULL,
	[FriendlyMessage] [nvarchar](max) NULL,
	[Error] [nvarchar](max) NULL,
	[StackTrace] [nvarchar](max) NULL,
	[InputData] [nvarchar](max) NULL,
	[OutputData] [nvarchar](max) NULL,
	[ParentLogID] [bigint] NULL,
	[CreatedOn] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Polishes]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Polishes](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ColorID] [int] NOT NULL,
	[BrandID] [int] NOT NULL,
	[Timestamp] [timestamp] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastEditedOn] [datetime] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[ColorNumber] [int] NOT NULL,
	[Label] [nvarchar](100) NOT NULL,
	[Coats] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[HasBeenTried] [bit] NOT NULL,
	[WasGift] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Polishes_AdditionalInfo]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Polishes_AdditionalInfo](
	[PolishID] [bigint] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[GiftFromName] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[PolishID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Polishes_AdditionalInfo_ARCHIVE]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Polishes_AdditionalInfo_ARCHIVE](
	[PolishID] [bigint] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[GiftFromName] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[PolishID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Polishes_ARCHIVE]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Polishes_ARCHIVE](
	[ID] [bigint] NOT NULL,
	[ColorID] [int] NOT NULL,
	[BrandID] [int] NOT NULL,
	[Timestamp] [timestamp] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastEditedOn] [datetime] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[ColorNumber] [int] NOT NULL,
	[Label] [nvarchar](100) NOT NULL,
	[Coats] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[HasBeenTried] [bit] NOT NULL,
	[WasGift] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Polishes_DestashInfo]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Polishes_DestashInfo](
	[PolishID] [bigint] NOT NULL,
	[Qty] [int] NOT NULL,
	[BuyerName] [nvarchar](50) NULL,
	[AskingPrice] [money] NULL,
	[SoldPrice] [money] NULL,
	[TrackingNumber] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[SaleStatus] [nvarchar](5) NULL,
PRIMARY KEY CLUSTERED 
(
	[PolishID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Polishes_DestashInfo_ARCHIVE]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Polishes_DestashInfo_ARCHIVE](
	[PolishID] [bigint] NOT NULL,
	[Qty] [int] NOT NULL,
	[BuyerName] [nvarchar](50) NULL,
	[AskingPrice] [money] NULL,
	[SoldPrice] [money] NULL,
	[TrackingNumber] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[SaleStatus] [nvarchar](5) NULL,
PRIMARY KEY CLUSTERED 
(
	[PolishID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Polishes_Glitter_Colors]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Polishes_Glitter_Colors](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[PolishID] [bigint] NOT NULL,
	[ColorID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Polishes_Glitter_Colors_ARCHIVE]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Polishes_Glitter_Colors_ARCHIVE](
	[ID] [bigint] NOT NULL,
	[PolishID] [bigint] NOT NULL,
	[ColorID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Polishes_Images]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Polishes_Images](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[PolishID] [bigint] NOT NULL,
	[Image] [varchar](max) NOT NULL,
	[MIMEType] [varchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[MakerImage] [bit] NULL,
	[PublicImage] [bit] NOT NULL,
	[DisplayImage] [bit] NULL,
	[CompressedImage] [varchar](max) NULL,
	[CompressedMIMEType] [varchar](50) NULL,
	[ImagePath] [varchar](max) NULL,
	[CompressedImagePath] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Polishes_Images_ARCHIVE]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Polishes_Images_ARCHIVE](
	[ID] [bigint] NOT NULL,
	[PolishID] [bigint] NOT NULL,
	[Image] [varchar](max) NOT NULL,
	[MIMEType] [varchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[MakerImage] [bit] NULL,
	[PublicImage] [bit] NOT NULL,
	[DisplayImage] [bit] NULL,
	[CompressedImage] [varchar](max) NULL,
	[CompressedMIMEType] [varchar](50) NULL,
	[ImagePath] [varchar](max) NULL,
	[CompressedImagePath] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Polishes_PolishTypes]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Polishes_PolishTypes](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[PolishID] [bigint] NOT NULL,
	[PolishTypeID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Polishes_PolishTypes_ARCHIVE]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Polishes_PolishTypes_ARCHIVE](
	[ID] [bigint] NOT NULL,
	[PolishID] [bigint] NOT NULL,
	[PolishTypeID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Polishes_Secondary_Colors]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Polishes_Secondary_Colors](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[PolishID] [bigint] NOT NULL,
	[ColorID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Polishes_Secondary_Colors_ARCHIVE]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Polishes_Secondary_Colors_ARCHIVE](
	[ID] [bigint] NOT NULL,
	[PolishID] [bigint] NOT NULL,
	[ColorID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PolishTypes]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PolishTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Settings]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[KeyName] [nvarchar](100) NOT NULL,
	[KeyValue] [nvarchar](max) NULL,
	[KeyDataType] [nvarchar](100) NULL,
	[PublicSetting] [bit] NOT NULL,
	[PrivateSetting] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShippingProviders]    Script Date: 11/17/2017 7:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShippingProviders](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Timestamp] [timestamp] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[TrackingBaseURL] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 11/17/2017 7:37:33 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserId]    Script Date: 11/17/2017 7:37:33 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserId]    Script Date: 11/17/2017 7:37:33 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_RoleId]    Script Date: 11/17/2017 7:37:33 PM ******/
CREATE NONCLUSTERED INDEX [IX_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserId]    Script Date: 11/17/2017 7:37:33 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserRoles]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 11/17/2017 7:37:33 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Brands] ADD  DEFAULT ((1)) FOR [CategoryID]
GO
ALTER TABLE [dbo].[Colors] ADD  DEFAULT ((1)) FOR [IsPrimary]
GO
ALTER TABLE [dbo].[Colors] ADD  DEFAULT ((1)) FOR [IsSecondary]
GO
ALTER TABLE [dbo].[Colors] ADD  DEFAULT ((0)) FOR [IsGlitter]
GO
ALTER TABLE [dbo].[IncomingLineTypes] ADD  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[IncomingOrderLines] ADD  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[IncomingOrderLines] ADD  DEFAULT ((1)) FOR [Qty]
GO
ALTER TABLE [dbo].[IncomingOrderLines_Polishes] ADD  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[IncomingOrderLines_Polishes] ADD  DEFAULT ((1)) FOR [Coats]
GO
ALTER TABLE [dbo].[IncomingOrderLines_Polishes] ADD  DEFAULT ((0)) FOR [HasBeenTried]
GO
ALTER TABLE [dbo].[IncomingOrderLines_Polishes] ADD  DEFAULT ((0)) FOR [WasGift]
GO
ALTER TABLE [dbo].[IncomingOrderLines_Polishes] ADD  DEFAULT ((0)) FOR [Converted]
GO
ALTER TABLE [dbo].[IncomingOrders] ADD  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[IncomingOrders] ADD  DEFAULT ((0)) FOR [OrderComplete]
GO
ALTER TABLE [dbo].[Logs] ADD  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Polishes] ADD  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Polishes] ADD  DEFAULT ((1)) FOR [Coats]
GO
ALTER TABLE [dbo].[Polishes] ADD  DEFAULT ((1)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Polishes] ADD  DEFAULT ((0)) FOR [HasBeenTried]
GO
ALTER TABLE [dbo].[Polishes] ADD  DEFAULT ((0)) FOR [WasGift]
GO
ALTER TABLE [dbo].[Polishes_ARCHIVE] ADD  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Polishes_ARCHIVE] ADD  DEFAULT ((1)) FOR [Coats]
GO
ALTER TABLE [dbo].[Polishes_ARCHIVE] ADD  DEFAULT ((1)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Polishes_ARCHIVE] ADD  DEFAULT ((0)) FOR [HasBeenTried]
GO
ALTER TABLE [dbo].[Polishes_ARCHIVE] ADD  DEFAULT ((0)) FOR [WasGift]
GO
ALTER TABLE [dbo].[Polishes_DestashInfo] ADD  DEFAULT ((1)) FOR [Qty]
GO
ALTER TABLE [dbo].[Polishes_Images] ADD  DEFAULT ((0)) FOR [MakerImage]
GO
ALTER TABLE [dbo].[Polishes_Images] ADD  DEFAULT ((1)) FOR [PublicImage]
GO
ALTER TABLE [dbo].[Polishes_Images] ADD  DEFAULT ((0)) FOR [DisplayImage]
GO
ALTER TABLE [dbo].[Polishes_Images_ARCHIVE] ADD  DEFAULT ((1)) FOR [PublicImage]
GO
ALTER TABLE [dbo].[Settings] ADD  DEFAULT ((0)) FOR [PublicSetting]
GO
ALTER TABLE [dbo].[Settings] ADD  DEFAULT ((1)) FOR [PrivateSetting]
GO
ALTER TABLE [dbo].[ShippingProviders] ADD  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Brands]  WITH CHECK ADD  CONSTRAINT [FK_Brand_Category_ID] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[BrandCategory] ([ID])
GO
ALTER TABLE [dbo].[Brands] CHECK CONSTRAINT [FK_Brand_Category_ID]
GO
ALTER TABLE [dbo].[IncomingOrderLines]  WITH CHECK ADD  CONSTRAINT [FK_Incoming_Line_Type_ID] FOREIGN KEY([IncomingLineTypeID])
REFERENCES [dbo].[IncomingLineTypes] ([ID])
GO
ALTER TABLE [dbo].[IncomingOrderLines] CHECK CONSTRAINT [FK_Incoming_Line_Type_ID]
GO
ALTER TABLE [dbo].[IncomingOrderLines]  WITH CHECK ADD  CONSTRAINT [FK_Incoming_Order_ID] FOREIGN KEY([IncomingOrderID])
REFERENCES [dbo].[IncomingOrders] ([ID])
GO
ALTER TABLE [dbo].[IncomingOrderLines] CHECK CONSTRAINT [FK_Incoming_Order_ID]
GO
ALTER TABLE [dbo].[IncomingOrderLines]  WITH CHECK ADD  CONSTRAINT [FK_Shipping_Provider_ID] FOREIGN KEY([ShippingProviderID])
REFERENCES [dbo].[ShippingProviders] ([ID])
GO
ALTER TABLE [dbo].[IncomingOrderLines] CHECK CONSTRAINT [FK_Shipping_Provider_ID]
GO
ALTER TABLE [dbo].[IncomingOrderLines_Polishes]  WITH CHECK ADD  CONSTRAINT [FK_Incoming_Order_Lines_Brand_ID] FOREIGN KEY([BrandID])
REFERENCES [dbo].[Brands] ([ID])
GO
ALTER TABLE [dbo].[IncomingOrderLines_Polishes] CHECK CONSTRAINT [FK_Incoming_Order_Lines_Brand_ID]
GO
ALTER TABLE [dbo].[IncomingOrderLines_Polishes]  WITH CHECK ADD  CONSTRAINT [FK_Incoming_Order_Lines_Color_ID] FOREIGN KEY([ColorID])
REFERENCES [dbo].[Colors] ([ID])
GO
ALTER TABLE [dbo].[IncomingOrderLines_Polishes] CHECK CONSTRAINT [FK_Incoming_Order_Lines_Color_ID]
GO
ALTER TABLE [dbo].[IncomingOrderLines_Polishes]  WITH CHECK ADD  CONSTRAINT [FK_Incoming_Order_Lines_ID] FOREIGN KEY([IncomingOrderLinesID])
REFERENCES [dbo].[IncomingOrderLines] ([ID])
GO
ALTER TABLE [dbo].[IncomingOrderLines_Polishes] CHECK CONSTRAINT [FK_Incoming_Order_Lines_ID]
GO
ALTER TABLE [dbo].[IncomingOrders]  WITH CHECK ADD  CONSTRAINT [FK_IncomingOrders_Shipping_Provider_ID] FOREIGN KEY([TrackingProviderID])
REFERENCES [dbo].[ShippingProviders] ([ID])
GO
ALTER TABLE [dbo].[IncomingOrders] CHECK CONSTRAINT [FK_IncomingOrders_Shipping_Provider_ID]
GO
ALTER TABLE [dbo].[Polishes]  WITH CHECK ADD  CONSTRAINT [FK_Polishes_Brand_ID] FOREIGN KEY([BrandID])
REFERENCES [dbo].[Brands] ([ID])
GO
ALTER TABLE [dbo].[Polishes] CHECK CONSTRAINT [FK_Polishes_Brand_ID]
GO
ALTER TABLE [dbo].[Polishes]  WITH CHECK ADD  CONSTRAINT [FK_Polishes_Color_ID] FOREIGN KEY([ColorID])
REFERENCES [dbo].[Colors] ([ID])
GO
ALTER TABLE [dbo].[Polishes] CHECK CONSTRAINT [FK_Polishes_Color_ID]
GO
ALTER TABLE [dbo].[Polishes_AdditionalInfo]  WITH CHECK ADD  CONSTRAINT [FK_Polishes_AdditionalInfo_Polish_ID] FOREIGN KEY([PolishID])
REFERENCES [dbo].[Polishes] ([ID])
GO
ALTER TABLE [dbo].[Polishes_AdditionalInfo] CHECK CONSTRAINT [FK_Polishes_AdditionalInfo_Polish_ID]
GO
ALTER TABLE [dbo].[Polishes_DestashInfo]  WITH CHECK ADD  CONSTRAINT [FK_Polishes_DestashInfo_Polish_ID] FOREIGN KEY([PolishID])
REFERENCES [dbo].[Polishes] ([ID])
GO
ALTER TABLE [dbo].[Polishes_DestashInfo] CHECK CONSTRAINT [FK_Polishes_DestashInfo_Polish_ID]
GO
ALTER TABLE [dbo].[Polishes_Glitter_Colors]  WITH CHECK ADD  CONSTRAINT [FK_Polishes_Glitter_Colors_Color_ID] FOREIGN KEY([ColorID])
REFERENCES [dbo].[Colors] ([ID])
GO
ALTER TABLE [dbo].[Polishes_Glitter_Colors] CHECK CONSTRAINT [FK_Polishes_Glitter_Colors_Color_ID]
GO
ALTER TABLE [dbo].[Polishes_Glitter_Colors]  WITH CHECK ADD  CONSTRAINT [FK_Polishes_Glitter_Colors_Polish_ID] FOREIGN KEY([PolishID])
REFERENCES [dbo].[Polishes] ([ID])
GO
ALTER TABLE [dbo].[Polishes_Glitter_Colors] CHECK CONSTRAINT [FK_Polishes_Glitter_Colors_Polish_ID]
GO
ALTER TABLE [dbo].[Polishes_Images]  WITH CHECK ADD  CONSTRAINT [FK_Polishes_Images_Polish_ID] FOREIGN KEY([PolishID])
REFERENCES [dbo].[Polishes] ([ID])
GO
ALTER TABLE [dbo].[Polishes_Images] CHECK CONSTRAINT [FK_Polishes_Images_Polish_ID]
GO
ALTER TABLE [dbo].[Polishes_PolishTypes]  WITH CHECK ADD  CONSTRAINT [FK_Polishes_PolishTypes_Polish_ID] FOREIGN KEY([PolishID])
REFERENCES [dbo].[Polishes] ([ID])
GO
ALTER TABLE [dbo].[Polishes_PolishTypes] CHECK CONSTRAINT [FK_Polishes_PolishTypes_Polish_ID]
GO
ALTER TABLE [dbo].[Polishes_PolishTypes]  WITH CHECK ADD  CONSTRAINT [FK_Polishes_PolishTypes_Polish_Type_ID] FOREIGN KEY([PolishTypeID])
REFERENCES [dbo].[PolishTypes] ([ID])
GO
ALTER TABLE [dbo].[Polishes_PolishTypes] CHECK CONSTRAINT [FK_Polishes_PolishTypes_Polish_Type_ID]
GO
ALTER TABLE [dbo].[Polishes_Secondary_Colors]  WITH CHECK ADD  CONSTRAINT [FK_Polishes_Secondary_Colors_Color_ID] FOREIGN KEY([ColorID])
REFERENCES [dbo].[Colors] ([ID])
GO
ALTER TABLE [dbo].[Polishes_Secondary_Colors] CHECK CONSTRAINT [FK_Polishes_Secondary_Colors_Color_ID]
GO
ALTER TABLE [dbo].[Polishes_Secondary_Colors]  WITH CHECK ADD  CONSTRAINT [FK_Polishes_Secondary_Colors_Polish_ID] FOREIGN KEY([PolishID])
REFERENCES [dbo].[Polishes] ([ID])
GO
ALTER TABLE [dbo].[Polishes_Secondary_Colors] CHECK CONSTRAINT [FK_Polishes_Secondary_Colors_Polish_ID]
GO
USE [master]
GO
ALTER DATABASE [PolishWarehouse] SET  READ_WRITE 
GO
