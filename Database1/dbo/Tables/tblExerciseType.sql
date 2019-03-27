CREATE TABLE [dbo].[tblExerciseType] (
    [ExerciseTypeId] INT            IDENTITY (1, 1) NOT NULL,
    [ExerciseName]   NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.tblExerciseType] PRIMARY KEY CLUSTERED ([ExerciseTypeId] ASC)
);

