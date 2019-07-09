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


