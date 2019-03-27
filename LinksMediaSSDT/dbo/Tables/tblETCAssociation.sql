CREATE TABLE [dbo].[tblETCAssociation] (
    [RecordId]       INT IDENTITY (1, 1) NOT NULL,
    [ChallengeId]    INT NOT NULL,
    [ExerciseTypeId] INT NOT NULL,
    CONSTRAINT [PK_dbo.tblETCAssociation] PRIMARY KEY CLUSTERED ([RecordId] ASC)
);

