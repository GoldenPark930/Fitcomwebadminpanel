CREATE TABLE [dbo].[tblCity] (
    [CityId]    INT            IDENTITY (1, 1) NOT NULL,
    [StateCode] NVARCHAR (MAX) NULL,
    [CityName]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.tblCity] PRIMARY KEY CLUSTERED ([CityId] ASC)
);

