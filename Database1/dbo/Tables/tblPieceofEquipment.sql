CREATE TABLE [dbo].[tblPieceofEquipment] (
    [PieceId]   INT           IDENTITY (1, 1) NOT NULL,
    [PieceName] NVARCHAR (20) NULL,
    CONSTRAINT [PK_dbo.tblPieceofEquipment] PRIMARY KEY CLUSTERED ([PieceId] ASC)
);

