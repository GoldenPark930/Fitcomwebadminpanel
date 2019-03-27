CREATE TABLE [dbo].[tblTrainerType] (
    [TrainerTypeId]   INT           IDENTITY (1, 1) NOT NULL,
    [TrainerTypeName] NVARCHAR (30) NOT NULL,
    CONSTRAINT [PK_tblTrainerType] PRIMARY KEY CLUSTERED ([TrainerTypeId] ASC)
);

