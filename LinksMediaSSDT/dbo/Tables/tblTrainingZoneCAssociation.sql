CREATE TABLE [dbo].[tblTrainingZoneCAssociation] (
    [TrainingZoneId] INT IDENTITY (1, 1) NOT NULL,
    [ChallengeId]    INT NOT NULL,
    [PartId]         INT NOT NULL,
    CONSTRAINT [PK_dbo.tblTrainingZoneCAssociation] PRIMARY KEY CLUSTERED ([TrainingZoneId] ASC)
);

