CREATE TABLE [dbo].[tblPWWorkoutsAssociation] (
    [PWWorkoutId]        BIGINT   IDENTITY (1, 1) NOT NULL,
    [PWRocordId]         BIGINT   NOT NULL,
    [WorkoutChallengeId] INT      NOT NULL,
    [CreatedBy]          INT      NOT NULL,
    [CreatedDate]        DATETIME NOT NULL,
    [ModifiedBy]         INT      NOT NULL,
    [ModifiedDate]       DATETIME NOT NULL,
    CONSTRAINT [PK_tblPWWorkoutsAssociation] PRIMARY KEY CLUSTERED ([PWWorkoutId] ASC)
);

