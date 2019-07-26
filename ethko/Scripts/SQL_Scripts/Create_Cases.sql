CREATE TABLE [dbo].[Cases] (
    [CaseId]          INT            NOT NULL,
    [ContactId]       INT            NOT NULL,
    [CaseName]        VARCHAR (MAX)  NOT NULL,
    [CaseNumber]      VARCHAR (MAX)  NOT NULL,
    [PracticeAreaId]  INT            NOT NULL,
    [BillingMethodId] INT            NOT NULL,
    [OfficeId]        INT            NOT NULL,
    [CaseStageId]     INT            NOT NULL,
    [DateOpened]      DATETIME       NOT NULL,
    [Description]     VARCHAR (MAX)  NULL,
    [FstUser]         NVARCHAR (128) NOT NULL,
    [InsDate]         DATETIME       NOT NULL,
    [RowVersion]      ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.Cases] PRIMARY KEY CLUSTERED ([CaseId] ASC),
		CONSTRAINT [FK_Cases_ToPracticeAreas] FOREIGN KEY ([PracticeAreaId]) REFERENCES [dbo].[PracticeAreas] ([PracticeAreaId]),
	CONSTRAINT [FK_Cases_ToBillingMethods] FOREIGN KEY ([BillingMethodId]) REFERENCES [dbo].[BillingMethods] ([BillingMethodId]),
	CONSTRAINT [FK_Cases_ToOffices] FOREIGN KEY ([OfficeId]) REFERENCES [dbo].[Offices] ([OfficeId]),
	CONSTRAINT [FK_Cases_ToCaseStages] FOREIGN KEY ([CaseStageId]) REFERENCES [dbo].[CaseStages] ([CaseStageId])
);

