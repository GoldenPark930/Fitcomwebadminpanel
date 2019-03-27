CREATE TABLE [dbo].[tblComment] (
    [CommentId]       INT            IDENTITY (1, 1) NOT NULL,
    [MessageStraemId] INT            NOT NULL,
    [Message]         NVARCHAR (MAX) NULL,
    [CommentedBy]     INT            NOT NULL,
    [CommentedDate]   DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.tblComment] PRIMARY KEY CLUSTERED ([CommentId] ASC),
    CONSTRAINT [FK_dbo.tblComment_dbo.tblMessageStream_MessageStraemId] FOREIGN KEY ([MessageStraemId]) REFERENCES [dbo].[tblMessageStream] ([MessageStraemId]) ON DELETE CASCADE
);

