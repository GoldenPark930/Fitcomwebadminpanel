CREATE TABLE [dbo].[tblDifficulty] (
    [DifficultyId]    INT            IDENTITY (1, 1) NOT NULL,
    [Difficulty]      NVARCHAR (MAX) NULL,
    [IsShowInProgram] BIT            NULL,
    CONSTRAINT [PK_dbo.tblDifficulty] PRIMARY KEY CLUSTERED ([DifficultyId] ASC)
);

