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


