CREATE TABLE [dbo].[tblUSerActivities] (
    [Id]           INT      IDENTITY (1, 1) NOT NULL,
    [ActivityId]   INT      NOT NULL,
    [UserId]       INT      NOT NULL,
    [AcceptedDate] DATETIME NOT NULL,
    [CreatedBy]    INT      NOT NULL,
    [CreatedDate]  DATETIME NULL,
    [ModifiedBy]   INT      DEFAULT ((0)) NOT NULL,
    [ModifiedDate] DATETIME NULL,
    CONSTRAINT [PK_dbo.tblUSerActivities] PRIMARY KEY CLUSTERED ([Id] ASC)
);

