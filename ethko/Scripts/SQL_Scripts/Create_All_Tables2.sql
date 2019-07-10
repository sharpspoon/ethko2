--UserTypes
CREATE TABLE [dbo].[UserTypes] (
    [UserTypeId]   INT           IDENTITY (1, 1) NOT NULL,
    [UserTypeName] NVARCHAR (50) NOT NULL,
    [InsDate]      DATETIME      NOT NULL,
    [RowVersion]   ROWVERSION    NOT NULL,
    CONSTRAINT [PK_dbo.UserTypes] PRIMARY KEY CLUSTERED ([UserTypeId] ASC)
);
--AspNetUsers
CREATE TABLE [dbo].[AspNetUsers] (
    [Id]                   NVARCHAR (128) NOT NULL,
    [FName]                VARCHAR (MAX)  NOT NULL,
    [LName]                VARCHAR (MAX)  NOT NULL,
    [MName]                VARCHAR (MAX)  NULL,
    [UserTypeId]           INT            NOT NULL,
    [Title]                VARCHAR (50)   NULL,
    [Archived]             SMALLINT       DEFAULT ((0)) NOT NULL,
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
    [UserName]             NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetUsers_ToUserTypes] FOREIGN KEY ([UserTypeId]) REFERENCES [dbo].[UserTypes] ([UserTypeId])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [dbo].[AspNetUsers]([UserName] ASC);

--AspNetRoles
CREATE TABLE [dbo].[AspNetRoles] (
    [Id]   NVARCHAR (128) NOT NULL,
    [Name] NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex]
    ON [dbo].[AspNetRoles]([Name] ASC);

--AspNetUserRoles
CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] NVARCHAR (128) NOT NULL,
    [RoleId] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserRoles]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RoleId]
    ON [dbo].[AspNetUserRoles]([RoleId] ASC);

--AspNetUserClaims
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [UserId]     NVARCHAR (128) NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserClaims]([UserId] ASC);

--AspNetUserLogins
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] NVARCHAR (128) NOT NULL,
    [ProviderKey]   NVARCHAR (128) NOT NULL,
    [UserId]        NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [UserId] ASC),
    CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserLogins]([UserId] ASC);

--Companies
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
    [RowVersion] ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.Companies] PRIMARY KEY CLUSTERED ([CompanyId] ASC)
);

--ContactGroups
CREATE TABLE [dbo].[ContactGroups] (
    [ContactGroupId]   INT            IDENTITY (1, 1) NOT NULL,
    [ContactGroupName] NVARCHAR (50)  NOT NULL,
    [InsDate]          DATETIME2 (7)  NOT NULL,
    [FstUser]          NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.ContactGroups] PRIMARY KEY CLUSTERED ([ContactGroupId] ASC)
);

--Contacts
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
    [RowVersion]         ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.Contacts] PRIMARY KEY CLUSTERED ([ContactId] ASC)
);

--PracticeAreas
CREATE TABLE [dbo].[PracticeAreas] (
    [PracticeAreaId]   INT            IDENTITY (1, 1) NOT NULL,
    [PracticeAreaName] VARCHAR (MAX)  NOT NULL,
    [FstUser]          NVARCHAR (128) NOT NULL,
    [InsDate]          DATETIME2 (7)  NOT NULL,
    [RowVersion]       ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.PracticeAreas] PRIMARY KEY CLUSTERED ([PracticeAreaId] ASC)
);

--CaseStages
CREATE TABLE [dbo].[CaseStages] (
    [CaseStageId]   INT            IDENTITY (1, 1) NOT NULL,
    [CaseStageName] NVARCHAR (50)  NOT NULL,
    [FstUser]       NVARCHAR (128) NOT NULL,
    [InsDate]       DATETIME2 (7)  NOT NULL,
    [RowVersion]    ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.CaseStages] PRIMARY KEY CLUSTERED ([CaseStageId] ASC)
);

--Offices
CREATE TABLE [dbo].[Offices] (
    [OfficeId]   INT            IDENTITY (1, 1) NOT NULL,
    [OfficeName] NVARCHAR (50)  NOT NULL,
    [FstUser]    NVARCHAR (128) NOT NULL,
    [InsDate]    DATETIME2 (7)  NOT NULL,
    [RowVersion] ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.Offices] PRIMARY KEY CLUSTERED ([OfficeId] ASC)
);

--BillingMethods
CREATE TABLE [dbo].[BillingMethods] (
    [BillingMethodId]   INT            IDENTITY (1, 1) NOT NULL,
    [BillingMethodName] VARCHAR (MAX)  NOT NULL,
    [FstUser]           NVARCHAR (128) NOT NULL,
    [InsDate]           DATETIME2 (7)  NOT NULL,
    [RowVersion]        ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.BillingMethods] PRIMARY KEY CLUSTERED ([BillingMethodId] ASC)
);

--Cases
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
    [RowVersion]      ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.Cases] PRIMARY KEY CLUSTERED ([CaseId] ASC)
);

