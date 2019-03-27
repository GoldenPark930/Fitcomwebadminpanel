CREATE TABLE [dbo].[tblMessageStreamPic] (
    [RecordId]        INT            IDENTITY (1, 1) NOT NULL,
    [MessageStraemId] INT            NOT NULL,
    [PicUrl]          NVARCHAR (MAX) NULL,
    [ImageMode]       NVARCHAR (MAX) NULL,
    [Height]          NVARCHAR (50)  NULL,
    [Width]           NVARCHAR (50)  NULL,
    CONSTRAINT [PK_dbo.tblMessageStreamPic] PRIMARY KEY CLUSTERED ([RecordId] ASC)
);

