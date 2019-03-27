CREATE TABLE [dbo].[tblUserAcceptedProgramWeek] (
    [UserAcceptedProgramWeekId] BIGINT   IDENTITY (1, 1) NOT NULL,
    [UserAcceptedProgramId]     INT      NOT NULL,
    [WeekSequenceNumber]        INT      NOT NULL,
    [CreatedBy]                 INT      NOT NULL,
    [CretaedDate]               DATETIME NULL,
    CONSTRAINT [PK_dbo.tblUserAcceptedProgramWeek] PRIMARY KEY CLUSTERED ([UserAcceptedProgramWeekId] ASC)
);

