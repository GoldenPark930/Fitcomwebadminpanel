CREATE TABLE [dbo].[tblSessionNotesExeciseSet] (
    [ExeciseSetId]     BIGINT   IDENTITY (1, 1) NOT NULL,
    [Reps]             INT      NULL,
    [Weight]           INT      NULL,
    [NotesExeciseId]   BIGINT   NULL,
    [CreatedBy]        INT      NULL,
    [CreatedDatetime]  DATETIME NULL,
    [ModifiedBy]       INT      NULL,
    [ModifiedDatetime] DATETIME NULL,
    CONSTRAINT [PK_tblSessionNotesExeciseSet] PRIMARY KEY CLUSTERED ([ExeciseSetId] ASC)
);

