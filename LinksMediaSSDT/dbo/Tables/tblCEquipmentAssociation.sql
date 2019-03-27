CREATE TABLE [dbo].[tblCEquipmentAssociation] (
    [CEquipmentId] INT IDENTITY (1, 1) NOT NULL,
    [ChallengeId]  INT NOT NULL,
    [EquipmentId]  INT NOT NULL,
    CONSTRAINT [PK_dbo.tblCEquipmentAssociation] PRIMARY KEY CLUSTERED ([CEquipmentId] ASC)
);

