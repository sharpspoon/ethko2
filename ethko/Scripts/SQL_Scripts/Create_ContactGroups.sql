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


