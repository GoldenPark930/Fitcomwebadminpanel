CREATE TABLE [dbo].[tblUsedSessionNote] (
    [UsedSessionNoteId] BIGINT         IDENTITY (1, 1) NOT NULL,
    [Notes]             NVARCHAR (MAX) NULL,
    [UseSessionId]      BIGINT         NULL,
    [CreatedBy]         INT            NULL,
    [createdDatetime]   DATETIME       NULL,
    [ModifiedBy]        INT            NULL,
    [ModifiedDatetime]  DATETIME       NULL,
    CONSTRAINT [PK_tblUsedSessionNote] PRIMARY KEY CLUSTERED ([UsedSessionNoteId] ASC)
);

