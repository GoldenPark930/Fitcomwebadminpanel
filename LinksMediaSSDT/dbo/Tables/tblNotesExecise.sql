CREATE TABLE [dbo].[tblNotesExecise] (
    [NotesExeciseId]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [Notes]             NVARCHAR (MAX) NULL,
    [ExeciseName]       NVARCHAR (255) NOT NULL,
    [UsedSessionNoteId] BIGINT         NULL,
    [CreatedBy]         INT            NULL,
    [CreatedDatetime]   DATETIME       NULL,
    [ModifiedBy]        INT            NULL,
    [ModifiedDatetime]  DATETIME       NULL,
    CONSTRAINT [PK_tblNotesSession] PRIMARY KEY CLUSTERED ([NotesExeciseId] ASC)
);

