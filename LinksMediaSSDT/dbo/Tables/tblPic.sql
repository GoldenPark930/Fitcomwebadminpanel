CREATE TABLE [dbo].[tblPic] (
    [RecordId]    INT            IDENTITY (1, 1) NOT NULL,
    [UserId]      INT            NOT NULL,
    [PicUrl]      NVARCHAR (MAX) NULL,
    [Description] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.tblPic] PRIMARY KEY CLUSTERED ([RecordId] ASC)
);

