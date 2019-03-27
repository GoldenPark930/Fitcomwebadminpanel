CREATE TABLE [dbo].[tblEquipment] (
    [EquipmentId] INT            IDENTITY (1, 1) NOT NULL,
    [Equipment]   NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.tblEquipment] PRIMARY KEY CLUSTERED ([EquipmentId] ASC)
);

