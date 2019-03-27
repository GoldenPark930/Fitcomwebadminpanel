CREATE TABLE [dbo].[tblFollowings] (
    [RecordId]         INT IDENTITY (1, 1) NOT NULL,
    [UserId]           INT NOT NULL,
    [FollowUserId]     INT NOT NULL,
    [IsManuallyFollow] BIT CONSTRAINT [DF_tblFollowings_IsManuallyFollow] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.tblFollowings] PRIMARY KEY CLUSTERED ([RecordId] ASC)
);

