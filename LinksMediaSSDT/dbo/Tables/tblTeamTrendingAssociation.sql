CREATE TABLE [dbo].[tblTeamTrendingAssociation] (
    [TreamTrendingId]      INT          IDENTITY (1, 1) NOT NULL,
    [TeamId]               INT          NOT NULL,
    [TrendingCategoryType] VARCHAR (50) NOT NULL,
    [TrendingCategoryId]   INT          NOT NULL,
    CONSTRAINT [PK_tblTeamTrendingAssociation] PRIMARY KEY CLUSTERED ([TreamTrendingId] ASC)
);

