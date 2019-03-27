CREATE TABLE [dbo].[tblTrainingType] (
    [TypeId]   INT           IDENTITY (1, 1) NOT NULL,
    [TypeName] NVARCHAR (20) NULL,
    CONSTRAINT [PK_dbo.tblTrainingType] PRIMARY KEY CLUSTERED ([TypeId] ASC)
);

