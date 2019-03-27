CREATE TABLE [dbo].[tblCEAssociation] (
    [RocordId]               INT            IDENTITY (1, 1) NOT NULL,
    [ChallengeId]            INT            NOT NULL,
    [Description]            NVARCHAR (MAX) NULL,
    [ExerciseId]             INT            NOT NULL,
    [ModifiedBy]             INT            NOT NULL,
    [ModifiedDate]           DATETIME       NOT NULL,
    [CreatedBy]              INT            CONSTRAINT [DF__tblCEAsso__Creat__440B1D61] DEFAULT ((0)) NOT NULL,
    [CreatedDate]            DATETIME       CONSTRAINT [DF__tblCEAsso__Creat__44FF419A] DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [Reps]                   INT            CONSTRAINT [DF__tblCEAssoc__Reps__45F365D3] DEFAULT ((0)) NOT NULL,
    [WeightForMan]           INT            CONSTRAINT [DF__tblCEAsso__Weigh__46E78A0C] DEFAULT ((0)) NOT NULL,
    [WeightForWoman]         INT            CONSTRAINT [DF__tblCEAsso__Weigh__47DBAE45] DEFAULT ((0)) NOT NULL,
    [IsAlternateExeciseName] BIT            CONSTRAINT [DF__tblCEAsso__IsAlt__02FC7413] DEFAULT ((0)) NULL,
    [AlternateExeciseName]   NVARCHAR (255) NULL,
    [SelectedEquipment]      NVARCHAR (255) NULL,
    [SelectedTraingZone]     NVARCHAR (255) NULL,
    [SelectedExeciseType]    NVARCHAR (255) NULL,
    [IsShownFirstExecise]    BIT            CONSTRAINT [DF_tblCEAssociation_IsShownFirstExecise] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.tblCEAssociation] PRIMARY KEY CLUSTERED ([RocordId] ASC)
);

