CREATE TABLE [dbo].[tblBoom] (
    [BoomId]          INT            IDENTITY (1, 1) NOT NULL,
    [MessageStraemId] INT            NOT NULL,
    [Message]         NVARCHAR (MAX) NULL,
    [BoomedBy]        INT            NOT NULL,
    [BoomedDate]      DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.tblBoom] PRIMARY KEY CLUSTERED ([BoomId] ASC)
);

