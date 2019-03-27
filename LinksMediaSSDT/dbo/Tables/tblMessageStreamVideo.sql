CREATE TABLE [dbo].[tblMessageStreamVideo] (
    [RecordId]        INT            IDENTITY (1, 1) NOT NULL,
    [MessageStraemId] INT            NOT NULL,
    [VideoUrl]        NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.tblMessageStreamVideo] PRIMARY KEY CLUSTERED ([RecordId] ASC)
);

