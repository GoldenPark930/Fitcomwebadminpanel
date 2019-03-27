CREATE TABLE [dbo].[tblChallengeType] (
    [ChallengeSubTypeId]    INT            IDENTITY (1, 1) NOT NULL,
    [ChallengeSubType]      NVARCHAR (200) NULL,
    [ChallengeType]         NVARCHAR (20)  NULL,
    [MaxLimit]              INT            NOT NULL,
    [Unit]                  NVARCHAR (20)  NULL,
    [ResultUnit]            NVARCHAR (20)  NULL,
    [IsExerciseMoreThanOne] NVARCHAR (5)   NULL,
    [IsActive]              BIT            NULL,
    CONSTRAINT [PK_dbo.tblChallengeType] PRIMARY KEY CLUSTERED ([ChallengeSubTypeId] ASC)
);

