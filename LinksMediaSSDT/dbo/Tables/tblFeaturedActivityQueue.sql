CREATE TABLE [dbo].[tblFeaturedActivityQueue] (
    [Id]           INT      IDENTITY (1, 1) NOT NULL,
    [ActivityID]   INT      NOT NULL,
    [StartDate]    DATETIME NULL,
    [EndDate]      DATETIME NULL,
    [CreatedBy]    INT      NOT NULL,
    [CreatedDate]  DATETIME NULL,
    [ModifiedBy]   INT      DEFAULT ((0)) NOT NULL,
    [ModifiedDate] DATETIME NULL,
    CONSTRAINT [PK_dbo.tblFeaturedActivityQueue] PRIMARY KEY CLUSTERED ([Id] ASC)
);

