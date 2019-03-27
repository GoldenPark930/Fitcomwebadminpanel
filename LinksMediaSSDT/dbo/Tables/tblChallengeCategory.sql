CREATE TABLE [dbo].[tblChallengeCategory] (
    [ChallengeCategoryId]   INT            IDENTITY (1, 1) NOT NULL,
    [ChallengeCategoryName] NVARCHAR (255) NOT NULL,
    [ChallengeSubTypeId]    INT            NOT NULL,
    [Isactive]              BIT            NULL,
    CONSTRAINT [PK_tblChallengeCategory] PRIMARY KEY CLUSTERED ([ChallengeCategoryId] ASC)
);

