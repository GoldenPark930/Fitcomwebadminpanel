CREATE TABLE [dbo].[tblUserNotifications] (
    [NotificationID]      BIGINT         IDENTITY (1, 1) NOT NULL,
    [SenderCredlID]       INT            NOT NULL,
    [ReceiverCredID]      INT            NOT NULL,
    [NotificationType]    NVARCHAR (MAX) NULL,
    [SenderUserName]      NVARCHAR (MAX) NULL,
    [Status]              BIT            NOT NULL,
    [IsRead]              BIT            NOT NULL,
    [CreatedDate]         DATETIME       NOT NULL,
    [ModifiedDate]        DATETIME       NULL,
    [TokenDevicesID]      NVARCHAR (MAX) NULL,
    [TargetID]            INT            CONSTRAINT [DF_tblUserNotifications_TargetID] DEFAULT ((0)) NULL,
    [ChallengeToFriendId] INT            NULL,
    [IsOnBoarding]        BIT            CONSTRAINT [DF_tblUserNotifications_IsOnBoarding] DEFAULT ((0)) NOT NULL,
    [IsFriendChallenge]   BIT            CONSTRAINT [DF_tblUserNotifications_IsFriendChallenge] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.tblUserNotifications] PRIMARY KEY CLUSTERED ([NotificationID] ASC)
);

