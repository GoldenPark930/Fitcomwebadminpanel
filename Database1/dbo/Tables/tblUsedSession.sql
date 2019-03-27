CREATE TABLE [dbo].[tblUsedSession] (
    [UseSessionId]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [UseSessionDateTime] VARCHAR (255) NOT NULL,
    [TrainingType]       NVARCHAR (50) NULL,
    [IsAttended]         BIT           CONSTRAINT [DF_tblUsedSession_IsAttended] DEFAULT ((0)) NOT NULL,
    [IsTracNotes]        BIT           CONSTRAINT [DF_tblUsedSession_IsTracNotes] DEFAULT ((0)) NOT NULL,
    [UserCredId]         INT           NOT NULL,
    [TrainerId]          INT           NOT NULL,
    [CreatedBy]          INT           NULL,
    [CreatedDatetime]    DATETIME      NULL,
    [ModifiedBy]         INT           NULL,
    [ModifiedDatetime]   DATETIME      NULL,
    CONSTRAINT [PK_tblUsedSession] PRIMARY KEY CLUSTERED ([UseSessionId] ASC)
);

