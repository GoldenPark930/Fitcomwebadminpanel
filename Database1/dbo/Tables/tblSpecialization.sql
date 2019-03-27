CREATE TABLE [dbo].[tblSpecialization] (
    [SpecializationId]   INT           IDENTITY (1, 1) NOT NULL,
    [SpecializationName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_dbo.tblSpecialization] PRIMARY KEY CLUSTERED ([SpecializationId] ASC)
);

