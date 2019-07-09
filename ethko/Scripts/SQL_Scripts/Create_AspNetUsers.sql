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
    [UserTypeId]           INT            NOT NULL,
    [Title]                VARCHAR (50)   NULL,
    [Archived]             SMALLINT       NOT NULL,
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


