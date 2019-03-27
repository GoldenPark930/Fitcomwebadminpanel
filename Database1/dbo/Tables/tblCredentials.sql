CREATE TABLE [dbo].[tblCredentials] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Password]  NVARCHAR (MAX) NULL,
    [UserId]    INT            NOT NULL,
    [UserType]  NVARCHAR (10)  NULL,
    [LastLogin] DATETIME       NULL,
    [EmailId]   NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_dbo.tblCredentials] PRIMARY KEY CLUSTERED ([Id] ASC)
);

