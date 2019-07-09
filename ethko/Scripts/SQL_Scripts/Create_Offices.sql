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


