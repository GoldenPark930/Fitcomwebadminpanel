CREATE TABLE [dbo].[tblChallengeToFriend] (
    [ChallengeToFriendId] INT            IDENTITY (1, 1) NOT NULL,
    [SubjectId]           INT            NOT NULL,
    [TargetId]            INT            NOT NULL,
    [ChallengeId]         INT            NOT NULL,
    [IsPending]           BIT            NOT NULL,
    [ChallengeDate]       DATETIME       DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [ChallengeByUserName] NVARCHAR (MAX) NULL,
    [Result]              NVARCHAR (MAX) NULL,
    [Fraction]            NVARCHAR (MAX) NULL,
    [ResultUnitSuffix]    NVARCHAR (MAX) NULL,
    [IsProgram]           BIT            NULL,
    [PersonalMessage]     NVARCHAR (MAX) NULL,
    [UserChallengeId]     INT            CONSTRAINT [DF_tblChallengeToFriend_UserChallengeId] DEFAULT ((0)) NOT NULL,
    [IsOnBoarding]        BIT            CONSTRAINT [DF_tblChallengeToFriend_IsOnBoarding] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.tblChallengeToFriend] PRIMARY KEY CLUSTERED ([ChallengeToFriendId] ASC)
);

