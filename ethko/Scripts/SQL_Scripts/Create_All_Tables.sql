USE [ethko_db]
GO

/****** Object: Table [dbo].[UserTypes] Script Date: 7/9/2019 5:10:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserTypes] (
    [UserTypeId]   INT           IDENTITY (1, 1) NOT NULL,
    [UserTypeName] NVARCHAR (50) NOT NULL,
    [InsDate]      DATETIME      NOT NULL,
    [RowVersion]   ROWVERSION    NOT NULL
);

ALTER TABLE [dbo].[UserTypes]
    ADD CONSTRAINT [PK_dbo.UserTypes] PRIMARY KEY CLUSTERED ([UserTypeId] ASC);


GO


USE [ethko_db]
GO

/****** Object: Table [dbo].[AspNetUsers] Script Date: 7/9/2019 5:11:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetUsers] (
    [Id]                   NVARCHAR (128) NOT NULL,
    [FName]                VARCHAR (MAX)  NOT NULL,
    [LName]                VARCHAR (MAX)  NOT NULL,
    [MName]                VARCHAR (MAX)  NULL,
    [UserTypeId]           INT           NOT NULL,
    [Title]                VARCHAR (50)   NULL,
    [Archived]             SMALLINT    DEFAULT (0)   NOT NULL,
    [Hometown]             NVARCHAR (MAX) NULL,
    [Email]                NVARCHAR (256) NULL,
    [CellPhone]            VARCHAR (50)   NULL,
    [WorkPhone]            VARCHAR (50)   NULL,
    [HomePhone]            VARCHAR (50)   NULL,
    [Fax]                  VARCHAR (50)   NULL,
    [SSN]                  VARCHAR (9)    NULL,
    [JobTitle]             VARCHAR (50)   NULL,
    [Address]              VARCHAR (50)   NULL,
    [Address2]             VARCHAR (50)   NULL,
    [City]                 VARCHAR (50)   NULL,
    [State]                VARCHAR (50)   NULL,
    [Zip]                  VARCHAR (50)   NULL,
    [Country]              VARCHAR (MAX)  NULL,
    [License]              VARCHAR (MAX)  NULL,
    [Website]              VARCHAR (MAX)  NULL,
    [CalendarColor]        VARCHAR (MAX)  NULL,
    [Notes]                VARCHAR (MAX)  NULL,
    [Birthday]             DATE           NULL,
    [DefaultHourlyRate]    INT            NULL,
    [RowVersion]           ROWVERSION     NOT NULL,
    [EmailConfirmed]       BIT            NOT NULL,
    [PasswordHash]         NVARCHAR (MAX) NULL,
    [SecurityStamp]        NVARCHAR (MAX) NULL,
    [PhoneNumber]          NVARCHAR (MAX) NULL,
    [PhoneNumberConfirmed] BIT            NOT NULL,
    [TwoFactorEnabled]     BIT            NOT NULL,
    [LockoutEndDateUtc]    DATETIME       NULL,
    [LastLogin]            DATETIME       NULL,
    [LockoutEnabled]       BIT            NOT NULL,
    [AccessFailedCount]    INT            NOT NULL,
    [UserName]             NVARCHAR (256) NOT NULL
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [dbo].[AspNetUsers]([UserName] ASC);


GO
ALTER TABLE [dbo].[AspNetUsers]
    ADD CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC);


GO
ALTER TABLE [dbo].[AspNetUsers]
    ADD CONSTRAINT [FK_AspNetUsers_ToUserTypes] FOREIGN KEY ([UserTypeId]) REFERENCES [dbo].[UserTypes] ([UserTypeId]);


	USE [ethko_db]
GO

/****** Object: Table [dbo].[AspNetRoles] Script Date: 7/9/2019 5:22:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetRoles] (
    [Id]   NVARCHAR (128) NOT NULL,
    [Name] NVARCHAR (256) NOT NULL
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex]
    ON [dbo].[AspNetRoles]([Name] ASC);


GO
ALTER TABLE [dbo].[AspNetRoles]
    ADD CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC);


	USE [ethko_db]
GO

/****** Object: Table [dbo].[AspNetUserRoles] Script Date: 7/9/2019 5:27:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] NVARCHAR (128) NOT NULL,
    [RoleId] NVARCHAR (128) NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserRoles]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RoleId]
    ON [dbo].[AspNetUserRoles]([RoleId] ASC);


GO
ALTER TABLE [dbo].[AspNetUserRoles]
    ADD CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC);


GO
ALTER TABLE [dbo].[AspNetUserRoles]
    ADD CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE;


GO
ALTER TABLE [dbo].[AspNetUserRoles]
    ADD CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;


	USE [ethko_db]
GO

/****** Object: Table [dbo].[AspNetUserClaims] Script Date: 7/9/2019 5:25:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [UserId]     NVARCHAR (128) NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserClaims]([UserId] ASC);


GO
ALTER TABLE [dbo].[AspNetUserClaims]
    ADD CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC);


GO
ALTER TABLE [dbo].[AspNetUserClaims]
    ADD CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;


	USE [ethko_db]
GO

/****** Object: Table [dbo].[AspNetUserLogins] Script Date: 7/9/2019 5:23:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] NVARCHAR (128) NOT NULL,
    [ProviderKey]   NVARCHAR (128) NOT NULL,
    [UserId]        NVARCHAR (128) NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserLogins]([UserId] ASC);


GO
ALTER TABLE [dbo].[AspNetUserLogins]
    ADD CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [UserId] ASC);


GO
ALTER TABLE [dbo].[AspNetUserLogins]
    ADD CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;


	USE [ethko_db]
GO

/****** Object: Table [dbo].[Companies] Script Date: 7/9/2019 5:12:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Companies] (
    [CompanyId]  INT            IDENTITY (1, 1) NOT NULL,
    [FstUser]    NVARCHAR (128) NOT NULL,
    [InsDate]    DATETIME       NOT NULL,
    [Name]       VARCHAR (MAX)  NOT NULL,
    [Archived]   SMALLINT       NOT NULL,
    [Email]      VARCHAR (MAX)  NOT NULL,
    [Website]    VARCHAR (50)   NULL,
    [MainPhone]  VARCHAR (50)   NULL,
    [FaxNumber]  VARCHAR (50)   NULL,
    [Address]    VARCHAR (50)   NULL,
    [Address2]   VARCHAR (50)   NULL,
    [City]       VARCHAR (50)   NULL,
    [State]      VARCHAR (50)   NULL,
    [Zip]        VARCHAR (50)   NULL,
    [Country]    VARCHAR (MAX)  NULL,
    [RowVersion] ROWVERSION     NOT NULL
);


USE [ethko_db]
GO

/****** Object: Table [dbo].[ContactGroups] Script Date: 7/9/2019 5:13:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ContactGroups] (
    [ContactGroupId]   INT            IDENTITY (1, 1) NOT NULL,
    [ContactGroupName] NVARCHAR (50)  NOT NULL,
    [InsDate]          DATETIME2 (7)  NOT NULL,
    [FstUser]          NVARCHAR (128) NOT NULL
);


USE [ethko_db]
GO

/****** Object: Table [dbo].[Contacts] Script Date: 7/9/2019 5:14:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Contacts] (
    [ContactId]          INT            IDENTITY (1, 1) NOT NULL,
    [UserId]             NVARCHAR (128) NOT NULL,
    [InsDate]            DATETIME2 (7)  NOT NULL,
    [FName]              VARCHAR (MAX)  NOT NULL,
    [LName]              VARCHAR (MAX)  NOT NULL,
    [MName]              VARCHAR (MAX)  NULL,
    [Title]              VARCHAR (50)   NULL,
    [Archived]           SMALLINT       NOT NULL,
    [Email]              VARCHAR (MAX)  NOT NULL,
    [ContactGroupId]     INT            NULL,
    [CompanyId]          INT            NULL,
    [EnableClientPortal] SMALLINT       NOT NULL,
    [CellPhone]          VARCHAR (50)   NULL,
    [WorkPhone]          VARCHAR (50)   NULL,
    [HomePhone]          VARCHAR (50)   NULL,
    [Fax]                VARCHAR (50)   NULL,
    [SSN]                VARCHAR (9)    NULL,
    [JobTitle]           VARCHAR (50)   NULL,
    [Address]            VARCHAR (50)   NULL,
    [Address2]           VARCHAR (50)   NULL,
    [City]               VARCHAR (50)   NULL,
    [State]              VARCHAR (50)   NULL,
    [Zip]                VARCHAR (50)   NULL,
    [Country]            VARCHAR (MAX)  NULL,
    [License]            VARCHAR (MAX)  NULL,
    [Website]            VARCHAR (MAX)  NULL,
    [Notes]              VARCHAR (MAX)  NULL,
    [Birthday]           DATE           NULL,
    [RowVersion]         ROWVERSION     NOT NULL
);


USE [ethko_db]
GO

/****** Object: Table [dbo].[PracticeAreas] Script Date: 7/9/2019 5:15:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PracticeAreas] (
    [PracticeAreaId]   INT            IDENTITY (1, 1) NOT NULL,
    [PracticeAreaName] VARCHAR (MAX)  NOT NULL,
    [FstUser]          NVARCHAR (128) NOT NULL,
    [InsDate]          DATETIME2 (7)  NOT NULL,
    [RowVersion]       ROWVERSION     NOT NULL
);


USE [ethko_db]
GO

/****** Object: Table [dbo].[CaseStages] Script Date: 7/9/2019 5:11:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CaseStages] (
    [CaseStageId]   INT            IDENTITY (1, 1) NOT NULL,
    [CaseStageName] NVARCHAR (50)  NOT NULL,
    [FstUser]       NVARCHAR (128) NOT NULL,
    [InsDate]       DATETIME2 (7)  NOT NULL,
    [RowVersion]    ROWVERSION     NOT NULL
);


USE [ethko_db]
GO

/****** Object: Table [dbo].[Offices] Script Date: 7/9/2019 5:03:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Offices] (
    [OfficeId]   INT            IDENTITY (1, 1) NOT NULL,
    [OfficeName] NVARCHAR (50)  NOT NULL,
    [FstUser]    NVARCHAR (128) NOT NULL,
    [InsDate]    DATETIME2 (7)  NOT NULL,
    [RowVersion] ROWVERSION     NOT NULL
);


USE [ethko_db]
GO

/****** Object: Table [dbo].[BillingMethods] Script Date: 7/9/2019 5:07:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BillingMethods] (
    [BillingMethodId]   INT            IDENTITY (1, 1) NOT NULL,
    [BillingMethodName] VARCHAR (MAX)  NOT NULL,
    [FstUser]           NVARCHAR (128) NOT NULL,
    [InsDate]           DATETIME2 (7)  NOT NULL,
    [RowVersion]        ROWVERSION     NOT NULL
);


USE [ethko_db]
GO

/****** Object: Table [dbo].[Cases] Script Date: 7/9/2019 5:00:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Cases] (
    [CaseId]          INT            NOT NULL,
    [ContactId]       INT            NOT NULL,
    [CaseName]        VARCHAR (MAX)  NOT NULL,
    [CaseNumber]      VARCHAR (MAX)  NOT NULL,
    [PracticeAreaId]  INT            NOT NULL,
    [BillingMethodId] INT            NOT NULL,
    [OfficeId]        INT            NOT NULL,
    [CaseStageId]     INT            NOT NULL,
    [DateOpened]      DATETIME2 (7)  NOT NULL,
    [Description]     VARCHAR (MAX)  NULL,
    [FstUser]         NVARCHAR (128) NOT NULL,
    [InsDate]         DATETIME2 (7)  NOT NULL,
    [RowVersion]      ROWVERSION     NOT NULL
);


