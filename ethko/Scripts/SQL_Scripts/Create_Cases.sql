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


