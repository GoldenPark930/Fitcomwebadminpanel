CREATE TABLE [dbo].[tblActivity] (
    [ActivityId]     INT            IDENTITY (1, 1) NOT NULL,
    [NameOfActivity] NVARCHAR (MAX) NULL,
    [TrainerId]      INT            NOT NULL,
    [DateOfEvent]    DATETIME       NOT NULL,
    [AddressLine1]   NVARCHAR (MAX) NULL,
    [AddressLine2]   NVARCHAR (MAX) NULL,
    [City]           INT            NOT NULL,
    [State]          NVARCHAR (MAX) NULL,
    [Zip]            NVARCHAR (MAX) NULL,
    [LearnMore]      NVARCHAR (MAX) NULL,
    [Video]          NVARCHAR (MAX) NULL,
    [Pic]            NVARCHAR (MAX) NULL,
    [CreatedBy]      INT            NOT NULL,
    [CreatedDate]    DATETIME       NULL,
    [ModifiedBy]     INT            NOT NULL,
    [ModifiedDate]   DATETIME       NULL,
    [Description]    NVARCHAR (MAX) NULL,
    [PromotionText]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.tblActivity] PRIMARY KEY CLUSTERED ([ActivityId] ASC)
);

