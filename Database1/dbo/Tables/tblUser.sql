﻿CREATE TABLE [dbo].[tblUser] (
    [UserId]                INT            IDENTITY (1, 1) NOT NULL,
    [TeamId]                INT            NOT NULL,
    [FirstName]             NVARCHAR (50)  NULL,
    [LastName]              NVARCHAR (50)  NULL,
    [DateOfBirth]           DATETIME       NULL,
    [Gender]                NVARCHAR (6)   NULL,
    [ZipCode]               NVARCHAR (10)  NULL,
    [EmailId]               NVARCHAR (MAX) NULL,
    [UserName]              NVARCHAR (50)  NULL,
    [Password]              NVARCHAR (MAX) NULL,
    [UserImageUrl]          NVARCHAR (MAX) NULL,
    [IsMailReceive]         BIT            NOT NULL,
    [CreatedBy]             INT            NOT NULL,
    [CreatedDate]           DATETIME       NOT NULL,
    [City]                  NVARCHAR (MAX) NULL,
    [State]                 NVARCHAR (MAX) NULL,
    [ModifiedBy]            INT            CONSTRAINT [DF__tblUser__Modifie__5629CD9C] DEFAULT ((0)) NOT NULL,
    [ModifiedDate]          DATETIME       CONSTRAINT [DF__tblUser__Modifie__571DF1D5] DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [UserBrief]             NVARCHAR (MAX) NULL,
    [Height]                NVARCHAR (MAX) NULL,
    [Weight]                NVARCHAR (MAX) NULL,
    [MTActive]              BIT            CONSTRAINT [DF_tblUser_MTActive] DEFAULT ((0)) NULL,
    [PersonalTrainerCredId] INT            CONSTRAINT [DF_tblUser_PersonalTrainerCredId] DEFAULT ((0)) NULL,
    [FitnessLevel]          VARCHAR (60)   NULL,
    CONSTRAINT [PK_dbo.tblUser] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

