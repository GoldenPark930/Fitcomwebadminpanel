CREATE TABLE [dbo].[tblUserToken] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [UserId]          INT            NOT NULL,
    [Token]           NVARCHAR (50)  NULL,
    [TokenDevicesID]  NVARCHAR (256) NULL,
    [DeviceUID]       NVARCHAR (MAX) NULL,
    [ClientIPAddress] NVARCHAR (20)  NULL,
    [IsExpired]       BIT            NOT NULL,
    [ExpiredOn]       DATETIME       NOT NULL,
    [IsRememberMe]    BIT            CONSTRAINT [DF__tblUserTo__IsRem__5CD6CB2B] DEFAULT ((0)) NOT NULL,
    [DeviceType]      NVARCHAR (50)  NULL,
    CONSTRAINT [PK_dbo.tblUserToken] PRIMARY KEY CLUSTERED ([Id] ASC)
);

