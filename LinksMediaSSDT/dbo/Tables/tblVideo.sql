CREATE TABLE [dbo].[tblVideo] (
    [RecordId]    INT            IDENTITY (1, 1) NOT NULL,
    [UserId]      INT            NOT NULL,
    [VideoUrl]    NVARCHAR (MAX) NULL,
    [Description] NVARCHAR (MAX) NULL,
    [ChallengeId] INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.tblVideo] PRIMARY KEY CLUSTERED ([RecordId] ASC)
);

