CREATE TABLE [dbo].[tblExercise] (
    [ExerciseId]   INT            IDENTITY (1, 1) NOT NULL,
    [ExerciseName] NVARCHAR (MAX) NULL,
    [Index]        NVARCHAR (MAX) NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [VideoLink]    NVARCHAR (MAX) NULL,
    [IsActive]     BIT            CONSTRAINT [DF__tblExerci__IsAct__02084FDA] DEFAULT ((0)) NULL,
    [V1080pUrl]    NVARCHAR (MAX) NULL,
    [V240pUrl]     NVARCHAR (MAX) NULL,
    [V360pUrl]     NVARCHAR (MAX) NULL,
    [V480pUrl]     NVARCHAR (MAX) NULL,
    [V720pUrl]     NVARCHAR (MAX) NULL,
    [ThumnailUrl]  NVARCHAR (MAX) NULL,
    [SecuryId]     NVARCHAR (MAX) NULL,
    [VideoId]      NVARCHAR (MAX) NULL,
    [SourceUrl]    NVARCHAR (MAX) NULL,
    [IsUpdated]    BIT            CONSTRAINT [DF_tblExercise_IsUpdated] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_dbo.tblExercise] PRIMARY KEY CLUSTERED ([ExerciseId] ASC)
);

