CREATE TABLE [dbo].[tblUserAssignmentByTrainer] (
    [UserAssignmentId]       INT            IDENTITY (1, 1) NOT NULL,
    [SubjectId]              INT            NOT NULL,
    [TargetId]               INT            NOT NULL,
    [ChallengeId]            INT            NOT NULL,
    [IsCompleted]            BIT            NOT NULL,
    [ChallengeDate]          DATETIME       NOT NULL,
    [ChallengeByUserName]    NVARCHAR (MAX) NULL,
    [IsProgram]              BIT            NULL,
    [ChallengeCompletedDate] DATETIME       NULL,
    [PersonalMessage]        NVARCHAR (MAX) NULL,
    [IsOnBoarding]           BIT            CONSTRAINT [DF_tblUserAssignmentByTrainer_IsOnBoarding] DEFAULT ((0)) NOT NULL,
    [Fraction]               NVARCHAR (MAX) NULL,
    [ResultUnitSuffix]       NVARCHAR (MAX) NULL,
    [Result]                 NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.tblUserAssignmentByTrainer] PRIMARY KEY CLUSTERED ([UserAssignmentId] ASC)
);

