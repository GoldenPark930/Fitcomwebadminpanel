CREATE TABLE [dbo].[tblCESAssociation] (
    [ExeciseSetId]  BIGINT         IDENTITY (1, 1) NOT NULL,
    [IsRestType]    BIT            CONSTRAINT [DF_tblCESAssociation_IsRestType] DEFAULT ((0)) NULL,
    [SetResult]     NVARCHAR (20)  NULL,
    [RestTime]      NVARCHAR (50)  NULL,
    [SetReps]       INT            NULL,
    [Description]   NVARCHAR (MAX) NULL,
    [RecordId]      INT            NULL,
    [CreatedBy]     INT            NOT NULL,
    [CreatedDate]   DATETIME       NOT NULL,
    [ModifiedBy]    INT            NOT NULL,
    [ModifiedDate]  DATETIME       NOT NULL,
    [AutoCountDown] BIT            CONSTRAINT [DF_tblCESAssociation_AutoCountDown] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_tblCESAssociation] PRIMARY KEY CLUSTERED ([ExeciseSetId] ASC)
);

