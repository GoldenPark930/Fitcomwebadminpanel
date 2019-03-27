CREATE TABLE [dbo].[tblNoTrainerChallengeTeam] (
    [NoTrainerWorkoutId] INT IDENTITY (1, 1) NOT NULL,
    [TeamId]             INT NOT NULL,
    [ChallengeId]        INT NOT NULL,
    [IsProgram]          BIT CONSTRAINT [DF_tblNoTrainerChallengeTeam_IsProgram] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_dbo.tblNoTrainerChallengeTeam] PRIMARY KEY CLUSTERED ([NoTrainerWorkoutId] ASC)
);

