﻿--Drop All Tables
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
drop table IF EXISTS Notifications

--UserTypes
CREATE TABLE [dbo].[UserTypes] (
    [UserTypeId]   INT           IDENTITY (1, 1) NOT NULL,
    [UserTypeName] NVARCHAR (50) NOT NULL,
    [InsDate]      DATETIME2      NOT NULL,
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
    [LockoutEndDateUtc]    DATETIME2       NULL,
    [LastLogin]            DATETIME2       NULL,
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
    [InsDate]    DATETIME2       NOT NULL,
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
    [InsDate]          DATETIME2  NOT NULL,
    [FstUser]          NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.ContactGroups] PRIMARY KEY CLUSTERED ([ContactGroupId] ASC)
);

--Contacts
CREATE TABLE [dbo].[Contacts] (
    [ContactId]          INT            IDENTITY (1, 1) NOT NULL,
    [UserId]             NVARCHAR (128) NOT NULL,
    [InsDate]            DATETIME2  NOT NULL,
    [FName]              VARCHAR (MAX)  NOT NULL,
    [LName]              VARCHAR (MAX)  NOT NULL,
    [MName]              VARCHAR (MAX)  NULL,
	[FullName]              VARCHAR (MAX)  NULL,
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
    [InsDate]          DATETIME2  NOT NULL,
    [RowVersion]       ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.PracticeAreas] PRIMARY KEY CLUSTERED ([PracticeAreaId] ASC)
);

--CaseStages
CREATE TABLE [dbo].[CaseStages] (
    [CaseStageId]   INT            IDENTITY (1, 1) NOT NULL,
    [CaseStageName] NVARCHAR (50)  NOT NULL,
    [FstUser]       NVARCHAR (128) NOT NULL,
    [InsDate]       DATETIME2  NOT NULL,
    [RowVersion]    ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.CaseStages] PRIMARY KEY CLUSTERED ([CaseStageId] ASC)
);

--Offices
CREATE TABLE [dbo].[Offices] (
    [OfficeId]   INT            IDENTITY (1, 1) NOT NULL,
    [OfficeName] NVARCHAR (50)  NOT NULL,
    [FstUser]    NVARCHAR (128) NOT NULL,
    [InsDate]    DATETIME2  NOT NULL,
    [RowVersion] ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.Offices] PRIMARY KEY CLUSTERED ([OfficeId] ASC)
);

--BillingMethods
CREATE TABLE [dbo].[BillingMethods] (
    [BillingMethodId]   INT            IDENTITY (1, 1) NOT NULL,
    [BillingMethodName] VARCHAR (MAX)  NOT NULL,
    [FstUser]           NVARCHAR (128) NOT NULL,
    [InsDate]           DATETIME2  NOT NULL,
    [RowVersion]        ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.BillingMethods] PRIMARY KEY CLUSTERED ([BillingMethodId] ASC)
);

--Cases
CREATE TABLE [dbo].[Cases] (
    [CaseId]          INT        IDENTITY (1, 1)    NOT NULL,
    [ContactId]       INT            NOT NULL,
    [CaseName]        VARCHAR (MAX)  NOT NULL,
    [CaseNumber]      VARCHAR (MAX)  NOT NULL,
    [PracticeAreaId]  INT            NOT NULL,
    [BillingMethodId] INT            NOT NULL,
    [OfficeId]        INT            NOT NULL,
    [CaseStageId]     INT            NOT NULL,
    [DateOpened]      DATETIME2  NOT NULL,
	[Statute]      DATETIME2  NOT NULL,
    [Description]     VARCHAR (MAX)  NULL,
    [FstUser]         NVARCHAR (128) NOT NULL,
    [InsDate]         DATETIME2  NOT NULL,
    [RowVersion]      ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.Cases] PRIMARY KEY CLUSTERED ([CaseId] ASC),
	CONSTRAINT [FK_Cases_ToContacts] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contacts] ([ContactId]),
	CONSTRAINT [FK_Cases_ToPracticeAreas] FOREIGN KEY ([PracticeAreaId]) REFERENCES [dbo].[PracticeAreas] ([PracticeAreaId]),
	CONSTRAINT [FK_Cases_ToBillingMethods] FOREIGN KEY ([BillingMethodId]) REFERENCES [dbo].[BillingMethods] ([BillingMethodId]),
	CONSTRAINT [FK_Cases_ToOffices] FOREIGN KEY ([OfficeId]) REFERENCES [dbo].[Offices] ([OfficeId]),
	CONSTRAINT [FK_Cases_ToCaseStages] FOREIGN KEY ([CaseStageId]) REFERENCES [dbo].[CaseStages] ([CaseStageId])
);

--LeadReferralSources
CREATE TABLE [dbo].[LeadReferralSources] (
    [ReferralSourceId]   INT            IDENTITY (1, 1) NOT NULL,
    [ReferralSourceName] VARCHAR (MAX)  NOT NULL,
    [FstUser]          NVARCHAR (128) NOT NULL,
    [InsDate]          DATETIME2  NOT NULL,
    [RowVersion]       ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.LeadReferralSources] PRIMARY KEY CLUSTERED ([ReferralSourceId] ASC)
);

--LeadStatuses
CREATE TABLE [dbo].[LeadStatuses] (
    [LeadStatusId]   INT            IDENTITY (1, 1) NOT NULL,
    [LeadStatusName] VARCHAR (MAX)  NOT NULL,
    [FstUser]          NVARCHAR (128) NOT NULL,
    [InsDate]          DATETIME2  NOT NULL,
    [RowVersion]       ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.LeadStatuses] PRIMARY KEY CLUSTERED ([LeadStatusId] ASC)
);

--Notifications
CREATE TABLE [dbo].[Notifications] (
    [NotificationId]          INT            IDENTITY (1, 1) NOT NULL,
	[UserId]         NVARCHAR (128) NOT NULL,
    [N1]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N2]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N3]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N4]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N5]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N6]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N7]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N8]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N9]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N10]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N11]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N12]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N13]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N14]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N15]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N16]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N17]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N18]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N19]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N20]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N21]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N22]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N23]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N24]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N25]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N26]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N27]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N28]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N29]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N30]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N31]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N32]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N33]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N34]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N35]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N36]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N37]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N38]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N39]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N40]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N41]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N42]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N43]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N44]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N45]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N46]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N47]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N48]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N49]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N50]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N51]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N52]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N53]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N54]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N55]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N56]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N57]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N58]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N59]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N60]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N61]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N62]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N63]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N64]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N65]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N66]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N67]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N68]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N69]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N70]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N71]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N72]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N73]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N74]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N75]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N76]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N77]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N78]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N79]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N80]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N81]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N82]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N83]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N84]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N85]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N86]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N87]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N88]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N89]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N90]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N91]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N92]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N93]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N94]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N95]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N96]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N97]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N98]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N99]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N100]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N101]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N102]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N103]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N104]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N105]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N106]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N107]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N108]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N109]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N110]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N111]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N112]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N113]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N114]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N115]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N116]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N117]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N118]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N119]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N120]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N121]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N122]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N123]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N124]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N125]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N126]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N127]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N128]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N129]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N130]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N131]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N132]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N133]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N134]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N135]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N136]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N137]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N138]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N139]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N140]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N141]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N142]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N143]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N144]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N145]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N146]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N147]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N148]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N149]       VARCHAR(2)    DEFAULT('on')         NULL,
	[N150]       VARCHAR(2)    DEFAULT('on')         NULL,
	[LstUser]         NVARCHAR (128) NOT NULL,
	[LstDate]         DATETIME2 (7)  NOT NULL,
    [FstUser]         NVARCHAR (128) NOT NULL,
    [InsDate]         DATETIME2 (7)  NOT NULL,
    [RowVersion]      ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.Notifications] PRIMARY KEY CLUSTERED ([NotificationId] ASC)
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

insert into Contacts
(FName, LName, MName, FullName, Title, Email, ContactGroupId, CellPhone, WorkPhone, HomePhone, Fax, SSN, JobTitle, Address, Address2, City, State, Zip, Country, License, Website, Notes, Birthday, InsDate, UserId, Archived, EnableClientPortal)
values
('Robin','Ward', 'Conn', 'Robin Conn Ward', 'Mr.', 'system@steelcitysites.net', 1, '334-332-7010', '334-332-7010', '334-332-7010', '334-332-7010', '123456789', 'Awesome Person', '123 Main Street', 'APT 1', 'Birmingham', 'AL', '36830', 'USA', 
'License-001', 'https://steelcitysites.net', 'Insert some notes', '2000-01-31',
 GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net'), 0, 0)

insert into Notifications
(UserId, LstUser, FstUser, InsDate, LstDate)
values
( (select id from AspNetUsers where UserName='system@steelcitysites.net'), (select id from AspNetUsers where UserName='system@steelcitysites.net'), (select id from AspNetUsers where UserName='system@steelcitysites.net'), GETDATE(), GETDATE())