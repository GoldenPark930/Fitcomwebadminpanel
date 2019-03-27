CREATE TABLE [dbo].[tblState] (
    [StateCode] NVARCHAR (128) NOT NULL,
    [StateName] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.tblState] PRIMARY KEY CLUSTERED ([StateCode] ASC)
);

