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


