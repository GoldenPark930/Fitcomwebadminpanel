CREATE TABLE [dbo].[tblUserSessionDetail] (
    [UserSessionId]          BIGINT   IDENTITY (1, 1) NOT NULL,
    [RemaingNumberOfSession] INT      NULL,
    [UsedNumberOfSession]    INT      NULL,
    [UserCredId]             INT      NULL,
    [TrainerId]              INT      NULL,
    [CreatedBy]              INT      NULL,
    [CreatedDatetime]        DATETIME NULL,
    [ModifiedBy]             INT      NULL,
    [ModifiedDateTime]       DATETIME NULL,
    CONSTRAINT [PK_tblUserSessionDetail] PRIMARY KEY CLUSTERED ([UserSessionId] ASC)
);

