CREATE TABLE [dbo].[tblUserNotificationSetting] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [UserCredId]       INT            NOT NULL,
    [DeviceID]         NVARCHAR (256) NULL,
    [DeviceType]       NVARCHAR (30)  NULL,
    [NotificationType] NVARCHAR (30)  NOT NULL,
    [IsNotify]         BIT            NOT NULL,
    [CreatedDate]      DATETIME       NOT NULL,
    [ModifiedDate]     DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.tblUserNotification] PRIMARY KEY CLUSTERED ([Id] ASC)
);

