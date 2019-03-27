CREATE TABLE [dbo].[tblBodyPart] (
    [PartId]           INT            IDENTITY (1, 1) NOT NULL,
    [PartName]         NVARCHAR (MAX) NULL,
    [IsShownHoneyComb] BIT            CONSTRAINT [DF_tblBodyPart_IsShownHoneyComb] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_dbo.tblBodyPart] PRIMARY KEY CLUSTERED ([PartId] ASC)
);

