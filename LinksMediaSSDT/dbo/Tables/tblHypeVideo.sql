CREATE TABLE [dbo].[tblHypeVideo] (
    [RecordId]     INT            IDENTITY (1, 1) NOT NULL,
    [HypeVideo]    NVARCHAR (MAX) NULL,
    [CreatedBy]    INT            NULL,
    [CreatedDate]  DATETIME       NULL,
    [UserId]       INT            DEFAULT ((0)) NOT NULL,
    [ChallengeId]  INT            NULL,
    [ModifiedBy]   INT            NULL,
    [ModifiedDate] DATETIME       NULL,
    CONSTRAINT [PK_dbo.tblHypeVideo] PRIMARY KEY CLUSTERED ([RecordId] ASC)
);

