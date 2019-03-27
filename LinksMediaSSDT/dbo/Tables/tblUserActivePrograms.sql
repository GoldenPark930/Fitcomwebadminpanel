CREATE TABLE [dbo].[tblUserActivePrograms] (
    [UserAcceptedProgramId] INT      IDENTITY (1, 1) NOT NULL,
    [UserCredId]            INT      NOT NULL,
    [AcceptedDate]          DATETIME NOT NULL,
    [CreatedBy]             INT      NOT NULL,
    [CreatedDate]           DATETIME NULL,
    [ProgramId]             INT      NOT NULL,
    [ModifiedBy]            INT      NOT NULL,
    [ModifiedDate]          DATETIME NULL,
    [CompletedDate]         DATETIME NULL,
    [IsCompleted]           BIT      CONSTRAINT [DF_tblUserActiveProgramWorksouts_IsCompleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_dbo.tblUserProgramWorksouts] PRIMARY KEY CLUSTERED ([UserAcceptedProgramId] ASC)
);

