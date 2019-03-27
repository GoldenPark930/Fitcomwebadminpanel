CREATE TABLE [dbo].[tblTrendingCategory] (
    [TrendingCategoryId] INT            IDENTITY (1, 1) NOT NULL,
    [TrendingType]       VARCHAR (50)   NOT NULL,
    [TrendingName]       NVARCHAR (255) NOT NULL,
    [IsActive]           BIT            CONSTRAINT [DF_tblTrendingCategory_IsActive] DEFAULT ((1)) NOT NULL,
    [TrendingPicUrl]     NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_tblTrendingCategory] PRIMARY KEY CLUSTERED ([TrendingCategoryId] ASC)
);

