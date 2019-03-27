CREATE TABLE [dbo].[tblResultBoom] (
    [ResultBoomId] INT            IDENTITY (1, 1) NOT NULL,
    [ResultId]     INT            NOT NULL,
    [Message]      NVARCHAR (MAX) NULL,
    [BoomedBy]     INT            NOT NULL,
    [BoomedDate]   DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.tblResultBoom] PRIMARY KEY CLUSTERED ([ResultBoomId] ASC)
);

