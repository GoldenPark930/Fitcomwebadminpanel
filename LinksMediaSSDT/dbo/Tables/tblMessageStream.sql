CREATE TABLE [dbo].[tblMessageStream] (
    [MessageStraemId]  INT            IDENTITY (1, 1) NOT NULL,
    [MessageType]      NVARCHAR (MAX) NULL,
    [Content]          NVARCHAR (MAX) NULL,
    [SubjectId]        INT            NOT NULL,
    [TargetId]         INT            NOT NULL,
    [TargetType]       NVARCHAR (MAX) NULL,
    [PostedDate]       DATETIME       NOT NULL,
    [IsTextImageVideo] BIT            NOT NULL,
    [IsImageVideo]     BIT            DEFAULT ((0)) NOT NULL,
    [IsNewsFeedHide]   BIT            CONSTRAINT [DF_tblMessageStream_IsNewsFeedHide] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.tblMessageStream] PRIMARY KEY CLUSTERED ([MessageStraemId] ASC)
);

