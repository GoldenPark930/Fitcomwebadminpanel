CREATE TABLE [dbo].[tblChallengeCategoryAssociation] (
    [ChallengeCategoryRecordId] BIGINT IDENTITY (1, 1) NOT NULL,
    [ChallengeId]               INT    NOT NULL,
    [ChallengeCategoryId]       INT    NOT NULL,
    [IsProgram]                 BIT    CONSTRAINT [DF_tblChallengeCategoryAssociation_IsProgram] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_tblChallengeCategoryAssociation] PRIMARY KEY CLUSTERED ([ChallengeCategoryRecordId] ASC)
);

