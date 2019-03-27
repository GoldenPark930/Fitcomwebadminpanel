CREATE TABLE [dbo].[tblUserAcceptedProgramWorkouts] (
    [PAcceptedWorkoutId]        BIGINT   IDENTITY (1, 1) NOT NULL,
    [UserAcceptedProgramWeekId] BIGINT   NULL,
    [ProgramChallengeId]        INT      NOT NULL,
    [UserCredID]                INT      NOT NULL,
    [PWeekID]                   BIGINT   NOT NULL,
    [PWWorkoutID]               BIGINT   NOT NULL,
    [WorkoutChallengeID]        INT      NOT NULL,
    [IsCompleted]               BIT      NOT NULL,
    [ChallengeDate]             DATETIME NOT NULL,
    [WeekWorkoutSequenceNumber] INT      CONSTRAINT [DF_tblUserAcceptedProgramWorkouts_WeekWorkoutSequenceNumber] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.tblProgramAcceptedWorkouts] PRIMARY KEY CLUSTERED ([PAcceptedWorkoutId] ASC)
);

