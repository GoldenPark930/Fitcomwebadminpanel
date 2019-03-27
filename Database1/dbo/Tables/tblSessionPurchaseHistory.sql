CREATE TABLE [dbo].[tblSessionPurchaseHistory] (
    [PurchaseHistoryId] BIGINT   IDENTITY (1, 1) NOT NULL,
    [UserCredId]        INT      NOT NULL,
    [TrainerId]         INT      NOT NULL,
    [NumberOfSession]   INT      NOT NULL,
    [Amount]            MONEY    NOT NULL,
    [PurchaseDateTime]  DATETIME NOT NULL,
    [CreatedBy]         INT      NULL,
    [CreatedDatetime]   DATETIME NULL,
    [ModifiedBy]        INT      NULL,
    [ModifiedDatetime]  DATETIME NULL,
    CONSTRAINT [PK_tblSessionPurchaseHistory] PRIMARY KEY CLUSTERED ([PurchaseHistoryId] ASC)
);

