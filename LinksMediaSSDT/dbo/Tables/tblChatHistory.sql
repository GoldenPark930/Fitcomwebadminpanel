CREATE TABLE [dbo].[tblChatHistory] (
    [ChatHistoryId]      BIGINT         IDENTITY (1, 1) NOT NULL,
    [SenderCredId]       INT            NOT NULL,
    [ReceiverCredId]     INT            NOT NULL,
    [Message]            NVARCHAR (MAX) NOT NULL,
    [TrasactionDateTime] DATETIME       NOT NULL,
    [IsRead]             BIT            CONSTRAINT [DF_tblChatHistory_IsRead] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_tblChatHistory] PRIMARY KEY CLUSTERED ([ChatHistoryId] ASC)
);

