--Drop All Tables
drop table IF EXISTS  Cases
drop table IF EXISTS BillingMethods
drop table IF EXISTS Offices
drop table IF EXISTS CaseStages
drop table IF EXISTS PracticeAreas
drop table IF EXISTS Contacts
drop table IF EXISTS ContactGroups
drop table IF EXISTS Companies
drop table IF EXISTS AspNetUserLogins
drop table IF EXISTS AspNetUserClaims
drop table IF EXISTS AspNetUserRoles
drop table IF EXISTS AspNetRoles
drop table IF EXISTS AspNetUsers
drop table IF EXISTS UserTypes
drop table IF EXISTS LeadReferralSources
drop table IF EXISTS LeadStatuses

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
    [InsDate]          DATETIME  NOT NULL,
    [FstUser]          NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.ContactGroups] PRIMARY KEY CLUSTERED ([ContactGroupId] ASC)
);

--Contacts
CREATE TABLE [dbo].[Contacts] (
    [ContactId]          INT            IDENTITY (1, 1) NOT NULL,
    [UserId]             NVARCHAR (128) NOT NULL,
    [InsDate]            DATETIME  NOT NULL,
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
    [InsDate]          DATETIME  NOT NULL,
    [RowVersion]       ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.PracticeAreas] PRIMARY KEY CLUSTERED ([PracticeAreaId] ASC)
);

--CaseStages
CREATE TABLE [dbo].[CaseStages] (
    [CaseStageId]   INT            IDENTITY (1, 1) NOT NULL,
    [CaseStageName] NVARCHAR (50)  NOT NULL,
    [FstUser]       NVARCHAR (128) NOT NULL,
    [InsDate]       DATETIME  NOT NULL,
    [RowVersion]    ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.CaseStages] PRIMARY KEY CLUSTERED ([CaseStageId] ASC)
);

--Offices
CREATE TABLE [dbo].[Offices] (
    [OfficeId]   INT            IDENTITY (1, 1) NOT NULL,
    [OfficeName] NVARCHAR (50)  NOT NULL,
    [FstUser]    NVARCHAR (128) NOT NULL,
    [InsDate]    DATETIME  NOT NULL,
    [RowVersion] ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.Offices] PRIMARY KEY CLUSTERED ([OfficeId] ASC)
);

--BillingMethods
CREATE TABLE [dbo].[BillingMethods] (
    [BillingMethodId]   INT            IDENTITY (1, 1) NOT NULL,
    [BillingMethodName] VARCHAR (MAX)  NOT NULL,
    [FstUser]           NVARCHAR (128) NOT NULL,
    [InsDate]           DATETIME  NOT NULL,
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
    [DateOpened]      DATETIME  NOT NULL,
    [Description]     VARCHAR (MAX)  NULL,
    [FstUser]         NVARCHAR (128) NOT NULL,
    [InsDate]         DATETIME  NOT NULL,
    [RowVersion]      ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.Cases] PRIMARY KEY CLUSTERED ([CaseId] ASC)
);

--LeadReferralSources
CREATE TABLE [dbo].[LeadReferralSources] (
    [ReferralSourceId]   INT            IDENTITY (1, 1) NOT NULL,
    [ReferralSourceName] VARCHAR (MAX)  NOT NULL,
    [FstUser]          NVARCHAR (128) NOT NULL,
    [InsDate]          DATETIME  NOT NULL,
    [RowVersion]       ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.LeadReferralSources] PRIMARY KEY CLUSTERED ([ReferralSourceId] ASC)
);

--LeadStatuses
CREATE TABLE [dbo].[LeadStatuses] (
    [LeadStatusId]   INT            IDENTITY (1, 1) NOT NULL,
    [LeadStatusName] VARCHAR (MAX)  NOT NULL,
    [FstUser]          NVARCHAR (128) NOT NULL,
    [InsDate]          DATETIME  NOT NULL,
    [RowVersion]       ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.LeadStatuses] PRIMARY KEY CLUSTERED ([LeadStatusId] ASC)
);





insert into UserTypes
(UserTypeName, insdate)
values
('Attorney', GETDATE()),
('Paralegal', GETDATE()),
('Staff', GETDATE());

insert into aspnetusers
(id, fname, lname, usertypeid, email, emailconfirmed, passwordhash, securitystamp,phonenumberconfirmed, twofactorenabled, lockoutenabled, accessfailedcount, username)
values
('4b6983a2-7178-472b-b7ae-f96470ea8087', 'Robin','Ward',1,'system@steelcitysites.net', 0, 'AEc78Zla/rBy6zDF+GRskTyFtZ/FtsvAMp4BK5L/swVWUfXGFkHGx5SFq10kybaD6Q==','9229c419-dd65-49d2-bb7f-ad9d667221b8',0,0,1,0,'system@steelcitysites.net')

insert into ContactGroups
(ContactGroupName, insdate, FstUser)
values
('Default', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net'))

insert into casestages
(CaseStageName, insdate, FstUser)
values
('Discovery', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('In Trial', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('On Hold', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net'))

insert into BillingMethods
(BillingMethodName, insdate, FstUser)
values
('Hourly', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Contingency', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Flat Fee', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Mix of Flat Fee and Hourly', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Pro Bono', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net'))

insert into Offices
(OfficeName, insdate, FstUser)
values
('Birmingham', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net'))

insert into practiceareas
(practiceareaname, insdate, FstUser)
values
('Bankruptcy', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Business', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Civil', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Criminal Defense', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Divorce/Separation', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('DUI/DWI', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Employment', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Estate Planning', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Family', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Foreclosure', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Immigration', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Landlord/Tenant', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Personal Injury', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Real Estate', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Tax', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net'))

insert into leadreferralsources
(referralsourcename, insdate, FstUser)
values
('Advertisement', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Avvo', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Client Referral', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Facebook', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('LinkedIn', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Networking Event', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Professional Referral', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Search', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Twitter', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Website', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Yelp', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Other', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net'))

insert into leadstatuses
(leadstatusname, insdate, FstUser)
values
('New', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Contacted', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Consult Scheduled', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Pending', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net'))