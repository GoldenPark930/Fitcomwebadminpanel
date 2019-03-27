CREATE TABLE [dbo].[tblResultComment] (
    [ResultCommentId] INT            IDENTITY (1, 1) NOT NULL,
    [Id]              INT            NOT NULL,
    [Message]         NVARCHAR (MAX) NULL,
    [CommentedBy]     INT            NOT NULL,
    [CommentedDate]   DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.tblResultComment] PRIMARY KEY CLUSTERED ([ResultCommentId] ASC),
    CONSTRAINT [FK_tblResultComment_tblUserChallenges] FOREIGN KEY ([Id]) REFERENCES [dbo].[tblUserChallenges] ([Id]) ON DELETE CASCADE
);

