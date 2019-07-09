USE [ethko_db]
GO

/****** Object: Table [dbo].[BillingMethods] Script Date: 7/9/2019 5:07:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BillingMethods] (
    [BillingMethodId]   INT            IDENTITY (1, 1) NOT NULL,
    [BillingMethodName] VARCHAR (MAX)  NOT NULL,
    [FstUser]           NVARCHAR (128) NOT NULL,
    [InsDate]           DATETIME2 (7)  NOT NULL,
    [RowVersion]        ROWVERSION     NOT NULL
);


