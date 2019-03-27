CREATE TABLE [dbo].[tblTrainerTeamMembers] (
    [RecordId]     INT      IDENTITY (1, 1) NOT NULL,
    [TeamId]       INT      NOT NULL,
    [UserId]       INT      NOT NULL,
    [CreatedBy]    INT      NOT NULL,
    [CreatedDate]  DATETIME NULL,
    [ModifiedBy]   INT      DEFAULT ((0)) NOT NULL,
    [ModifiedDate] DATETIME NULL,
    CONSTRAINT [PK_dbo.tblTrainerTeamMembers] PRIMARY KEY CLUSTERED ([RecordId] ASC)
);

