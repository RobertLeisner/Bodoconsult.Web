USE [master]
GO

/****** Object:  Database [BodoWebMailer]    Script Date: 08.09.2026 17:36:29 ******/
CREATE DATABASE [BodoWebMailer]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BodoWebMailer', FILENAME = N'D:\SQLData\BodoWebMailer.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'BodoWebMailer_log', FILENAME = N'D:\SQLData\BodoWebMailer_log.ldf' , SIZE = 3840KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BodoWebMailer].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [BodoWebMailer] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [BodoWebMailer] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [BodoWebMailer] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [BodoWebMailer] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [BodoWebMailer] SET ARITHABORT OFF 
GO

ALTER DATABASE [BodoWebMailer] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [BodoWebMailer] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [BodoWebMailer] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [BodoWebMailer] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [BodoWebMailer] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [BodoWebMailer] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [BodoWebMailer] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [BodoWebMailer] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [BodoWebMailer] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [BodoWebMailer] SET  DISABLE_BROKER 
GO

ALTER DATABASE [BodoWebMailer] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [BodoWebMailer] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [BodoWebMailer] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [BodoWebMailer] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [BodoWebMailer] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [BodoWebMailer] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [BodoWebMailer] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [BodoWebMailer] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [BodoWebMailer] SET  MULTI_USER 
GO

ALTER DATABASE [BodoWebMailer] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [BodoWebMailer] SET DB_CHAINING OFF 
GO

ALTER DATABASE [BodoWebMailer] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [BodoWebMailer] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [BodoWebMailer] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [BodoWebMailer] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [BodoWebMailer] SET QUERY_STORE = OFF
GO

ALTER DATABASE [BodoWebMailer] SET  READ_WRITE 
GO


GO

/****** Object:  DatabaseRole [MailSender]    Script Date: 08.09.2026 17:39:40 ******/
CREATE ROLE [MailSender]
GO

GO

/****** Object:  DatabaseRole [MailUser]    Script Date: 08.09.2026 17:40:05 ******/
CREATE ROLE [MailUser]
GO

USE [BodoWebMailer]
GO

/****** Object:  Table [dbo].[tMail]    Script Date: 08.09.2026 17:40:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tMail](
	[M_ID] [uniqueidentifier] NOT NULL,
	[M_From] [varchar](255) NULL,
	[M_To] [varchar](max) NOT NULL,
	[M_Subject] [varchar](255) NOT NULL,
	[M_Body] [varchar](max) NOT NULL,
	[M_Error] [bit] NOT NULL,
	[M_LogoPath] [varchar](900) NULL,
	[M_SignatureTemplate] [varchar](255) NULL,
	[M_Attachments] [varchar](max) NULL,
	[M_Archive] [bit] NOT NULL,
	[M_Queries] [varchar](max) NULL,
	[M_Zip] [bit] NOT NULL,
	[M_ZipPassword] [varchar](50) NULL,
 CONSTRAINT [PK_tMail] PRIMARY KEY CLUSTERED 
(
	[M_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[tMail] ADD  CONSTRAINT [DF_tMail_M_ID]  DEFAULT (newid()) FOR [M_ID]
GO

ALTER TABLE [dbo].[tMail] ADD  CONSTRAINT [DF_tMail_M_Error]  DEFAULT ((0)) FOR [M_Error]
GO

ALTER TABLE [dbo].[tMail] ADD  CONSTRAINT [DF__tMail__M_Archive__0BC6C43E]  DEFAULT ((0)) FOR [M_Archive]
GO

ALTER TABLE [dbo].[tMail] ADD  DEFAULT ((0)) FOR [M_Zip]
GO

USE [BodoWebMailer]
GO

/****** Object:  Table [dbo].[tMailArchive]    Script Date: 08.09.2026 17:40:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tMailArchive](
	[M_ID] [uniqueidentifier] NOT NULL,
	[M_From] [varchar](255) NULL,
	[M_To] [varchar](max) NOT NULL,
	[M_Subject] [varchar](255) NOT NULL,
	[M_Body] [varchar](max) NOT NULL,
	[M_LogoPath] [varchar](900) NULL,
	[M_SignatureTemplate] [varchar](255) NULL,
	[M_Date] [datetime] NOT NULL,
	[M_Attachments] [varchar](max) NULL,
	[M_Queries] [varchar](max) NULL,
	[M_Zip] [bit] NOT NULL,
	[M_ZipPassword] [varchar](50) NULL,
 CONSTRAINT [PK_tMailArchive] PRIMARY KEY CLUSTERED 
(
	[M_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[tMailArchive] ADD  CONSTRAINT [DF_tMailArchive_M_ID]  DEFAULT (newid()) FOR [M_ID]
GO

ALTER TABLE [dbo].[tMailArchive] ADD  CONSTRAINT [DF__tMailArch__M_Dat__145C0A3F]  DEFAULT (getdate()) FOR [M_Date]
GO

ALTER TABLE [dbo].[tMailArchive] ADD  DEFAULT ((0)) FOR [M_Zip]
GO

GO

SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[spHELP_Migration_DropOldUsers]
as

declare @SQL nvarchar(max), @M varchar(255)

set @SQL=''


DECLARE UC CURSOR LOCAL READ_ONLY FOR
SELECT [name]
from sysusers 
where (issqluser=1 or isntuser=1 or isntgroup=1)
	and [uid]>4

OPEN UC

FETCH NEXT FROM UC
INTO @M

WHILE (@@FETCH_STATUS = 0)
BEGIN

	set @SQL= 'DROP USER ['+@M+'] '

	exec dbo.sp_executesql @SQL

    FETCH NEXT FROM UC
    INTO @M
END

CLOSE UC

DEALLOCATE UC

--exec [dbo].[spHELP_Migration_DropOldUsers]

GO

USE [BodoWebMailer]
GO

/****** Object:  StoredProcedure [dbo].[spMail_Delete]    Script Date: 08.09.2026 17:42:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create proc [dbo].[spMail_Delete]
@ID uniqueidentifier
/*
Mail nach Versand löschen

© 2011 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted

delete from tMail where M_ID=@ID


GO

USE [BodoWebMailer]
GO

/****** Object:  StoredProcedure [dbo].[spMail_FetchAll]    Script Date: 08.09.2026 17:42:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE proc [dbo].[spMail_FetchAll]
/*
Alle Mails holen

© 2011 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted

--begin try

SELECT [M_ID]
		, M_FROM
		, [M_To]
		, [M_Subject]
		, [M_Body]
		, [M_LogoPath]
		, [M_SignatureTemplate] 
		, M_Attachments
		, M_Archive
		, M_Queries
		, M_Zip
		, M_ZipPassword
FROM [dbo].[tMail]
where [M_Error]=0


--end try
--begin catch
--	declare @Mail varchar(255), @MailMsg varchar(max)

--	select @Mail=CAST(value as varchar(255)) from dbo.Settings where SKey = 'MailAdmin'

--	set @MailMsg= DB_NAME()+': '+ERROR_PROCEDURE()+': Row '+cast(ERROR_LINE() as varchar(max))+': '+ERROR_MESSAGE()
	
--	print @MailMsg
	
--	exec dbo.spMail_New @Mail, 'Database-Error',@MailMsg, null, null, null
	
--end catch

GO

USE [BodoWebMailer]
GO

/****** Object:  StoredProcedure [dbo].[spMail_MoveToArchive]    Script Date: 08.09.2026 17:43:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE proc [dbo].[spMail_MoveToArchive]
@M_ID uniqueidentifier
/*

Move mails to archive

© 2011 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted

--begin try


	INSERT INTO [dbo].[tMailArchive]
			   ([M_ID]
			   ,[M_From]
			   ,[M_To]
			   ,[M_Subject]
			   ,[M_Body]
			   ,[M_LogoPath]
			   ,[M_SignatureTemplate]
			   ,[M_Attachments]
			   , M_Queries
				, M_Zip
			, M_ZipPassword)
	SELECT [M_ID]
		  ,[M_From]
		  ,[M_To]
		  ,[M_Subject]
		  ,[M_Body]
		  ,[M_LogoPath]
		  ,[M_SignatureTemplate]
		  ,[M_Attachments]
		  , M_Queries
		, M_Zip
		, M_ZipPassword
	FROM [dbo].[tMail]
	where M_ID=@M_ID

	delete from tMail where M_ID=@M_ID

--end try
--begin catch
--	declare @Mail varchar(255), @MailMsg varchar(max)

--	select @Mail=CAST(value as varchar(255)) from dbo.Settings where SKey = 'MailAdmin'

--	set @MailMsg= DB_NAME()+': '+ERROR_PROCEDURE()+': Row '+cast(ERROR_LINE() as varchar(max))+': '+ERROR_MESSAGE()
	
--	print @MailMsg
	
--	exec dbo.spMail_New @Mail, 'Database-Error',@MailMsg, null, null, null
	
--end catch


GO

USE [BodoWebMailer]
GO

/****** Object:  StoredProcedure [dbo].[spMail_SetError]    Script Date: 08.09.2026 17:43:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create proc [dbo].[spMail_SetError]
@ID uniqueidentifier
/*
Nach fehlgeschlagenem Versand Fehlerflag für Mail setzen

© 2011 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted

update tMail
set M_Error=1
where M_ID=@ID


GO

USE [BodoWebMailer]
GO

/****** Object:  StoredProcedure [dbo].[spMail_Test]    Script Date: 08.09.2026 17:43:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create proc [dbo].[spMail_Test]

/*

Send a test amil

© 2019 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted

	INSERT INTO [dbo].[tMail]
           ([M_From]
           ,[M_To]
           ,[M_Subject]
           ,[M_Body]
           ,[M_Error]
           ,[M_LogoPath]
           ,[M_SignatureTemplate]
           )
     VALUES
           ('noreply@test.de'
           ,'info@bodoconsult.de'
           ,'Testmail'
           ,'This is a test mail from BodoWebMailer'
           ,0
		   ,'c:\bodoconsult\Logos\BodoConsult.gif'
           ,'BodoConsultSignature')
GO

GRANT EXEC ON dbo.spMail_Delete TO MailSender

GO

GRANT EXEC ON dbo.spMail_FetchAll TO MailSender

GO

GRANT EXEC ON dbo.spMail_MoveToArchive To MailSender

GO

GRANT INSERT, UPDATE, DELETE, SELECT ON dbo.t_Mail TO MailSender

GO

GRANT INSERT ON dbo.t_Mail TO MailUser

GO

GRANT INSERT, UPDATE, DELETE, SELECT ON dbo.t_MailArchive TO MailSender

GO


GO

CREATE TABLE [dbo].[Settings](
	[S_ID] [uniqueidentifier] NOT NULL,
	[sKey] [varchar](255) NOT NULL,
	[Value] [varchar](max) NULL,
	[Description] [text] NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY NONCLUSTERED 
(
	[S_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_S_ID]  DEFAULT (newid()) FOR [S_ID]
GO


GO

INSERT INTO [dbo].[Settings]
           ([S_ID]
           ,[sKey]
           ,[Value]
           ,[Description])
     VALUES
           (newid()
           ,'MailAccount'
           ,'{
	"$type": "Bodoconsult.Web.Mail.Model.O365MailAccount, Bodoconsult.Web.Mail",
	"Instance": "Your instance encrypted",
	"Tenant": "Your tenant encrypted",
	"ClientId": "Your clientID encrypted",
	"ClientSecret": "Your client secret encrypted",
	"UserName": "Your username encrypted",
	"Scope": "Your scope encrypted"
}'
           ,'JSON string with mail acccount object')
GO

GRANT SELECT ON dbo.Settings TO MailSender

GO