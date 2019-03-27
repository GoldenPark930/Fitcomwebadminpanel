CREATE TABLE [dbo].[tblSponsorChallengeQueue] (
    [QueueId]            INT            IDENTITY (1, 1) NOT NULL,
    [ChallengeId]        INT            NULL,
    [TrainerId]          INT            NOT NULL,
    [NameOfChallenge]    NVARCHAR (MAX) NULL,
    [SponsorName]        NVARCHAR (MAX) NULL,
    [StartDate]          DATETIME       NULL,
    [EndDate]            DATETIME       NULL,
    [CurrentlyDisplayed] BIT            NULL,
    [ResultId]           INT            NOT NULL,
    [HypeVideoId]        INT            NOT NULL,
    [CreatedBy]          INT            NULL,
    [CreatedDate]        DATETIME       NULL,
    [ModifiedBy]         INT            NULL,
    [ModifiedDate]       DATETIME       NULL,
    CONSTRAINT [PK_dbo.tblSponsorChallengeQueue] PRIMARY KEY CLUSTERED ([QueueId] ASC)
);

