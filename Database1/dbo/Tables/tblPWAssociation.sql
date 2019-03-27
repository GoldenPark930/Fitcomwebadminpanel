CREATE TABLE [dbo].[tblPWAssociation] (
    [PWRocordId]               BIGINT   IDENTITY (1, 1) NOT NULL,
    [ProgramChallengeId]       INT      NOT NULL,
    [ModifiedBy]               INT      NOT NULL,
    [ModifiedDate]             DATETIME NOT NULL,
    [CreatedBy]                INT      NOT NULL,
    [CreatedDate]              DATETIME NOT NULL,
    [AssignedTrainerId]        INT      CONSTRAINT [DF_tblPWAssociation_AssignedTrainerId] DEFAULT ((0)) NULL,
    [AssignedTrainingzone]     INT      CONSTRAINT [DF_tblPWAssociation_AssignedTrainingzone] DEFAULT ((0)) NULL,
    [AssignedDifficulyLevelId] INT      CONSTRAINT [DF_tblPWAssociation_AssignedDifficulyLevelId] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_dbo.tblPWAssociation] PRIMARY KEY CLUSTERED ([PWRocordId] ASC)
);

