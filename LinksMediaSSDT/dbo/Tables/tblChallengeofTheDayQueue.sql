CREATE TABLE [dbo].[tblChallengeofTheDayQueue] (
    [QueueId]            INT            IDENTITY (1, 1) NOT NULL,
    [UserId]             INT            NOT NULL,
    [NameofChallenge]    NVARCHAR (MAX) NULL,
    [StartDate]          DATETIME       NULL,
    [EndDate]            DATETIME       NULL,
    [CurrentlyDisplayed] BIT            NULL,
    [CreatedBy]          INT            NULL,
    [CreatedDate]        DATETIME       NULL,
    [ResultId]           INT            DEFAULT ((0)) NOT NULL,
    [HypeVideoId]        INT            DEFAULT ((0)) NOT NULL,
    [ChallengeId]        INT            DEFAULT ((0)) NOT NULL,
    [ModifiedBy]         INT            NULL,
    [ModifiedDate]       DATETIME       NULL,
    CONSTRAINT [PK_dbo.tblChallengeofTheDayQueue] PRIMARY KEY CLUSTERED ([QueueId] ASC)
);

