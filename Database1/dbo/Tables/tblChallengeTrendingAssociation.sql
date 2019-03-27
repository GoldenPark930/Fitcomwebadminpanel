CREATE TABLE [dbo].[tblChallengeTrendingAssociation] (
    [ChallengeTrendingId] BIGINT IDENTITY (1, 1) NOT NULL,
    [ChallengeId]         INT    NOT NULL,
    [TrendingCategoryId]  INT    NOT NULL,
    [IsProgram]           BIT    CONSTRAINT [DF_tblChallengeTrendingAssociation_IsProgram] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_tblChallengeTrendingAssociation] PRIMARY KEY CLUSTERED ([ChallengeTrendingId] ASC)
);

