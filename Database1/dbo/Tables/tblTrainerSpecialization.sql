CREATE TABLE [dbo].[tblTrainerSpecialization] (
    [Id]               INT IDENTITY (1, 1) NOT NULL,
    [SpecializationId] INT NOT NULL,
    [TrainerId]        INT NOT NULL,
    [IsInTopThree]     INT DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.tblTrainerSpecialization] PRIMARY KEY CLUSTERED ([Id] ASC)
);

