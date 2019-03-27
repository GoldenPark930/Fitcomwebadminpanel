CREATE TABLE [dbo].[tblUserChallenges] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [UserId]               INT            NOT NULL,
    [AcceptedDate]         DATETIME       NOT NULL,
    [CreatedBy]            INT            NOT NULL,
    [CreatedDate]          DATETIME       NULL,
    [Result]               NVARCHAR (MAX) NULL,
    [Fraction]             NVARCHAR (MAX) NULL,
    [isGlobal]             BIT            DEFAULT ((0)) NOT NULL,
    [isNewsFeed]           BIT            DEFAULT ((0)) NOT NULL,
    [ResultUnit]           NVARCHAR (MAX) NULL,
    [ChallengeId]          INT            DEFAULT ((0)) NOT NULL,
    [ModifiedBy]           INT            DEFAULT ((0)) NOT NULL,
    [ModifiedDate]         DATETIME       NULL,
    [IsProgramchallenge]   BIT            NULL,
    [CompletedTextMessage] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.tblUserChallenges] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
--Summary: To update the Result Unit corresponding to Challenge SubType Id
--Developer Name: Arvind Kumar
--Create Date: 28th May, 2015


CREATE TRIGGER [dbo].[TRG_UpdateUserChallenge]
ON [dbo].[tblUserChallenges]
AFTER UPDATE AS
BEGIN
DECLARE @userChallengeId int;
DECLARE @subTypeId int;

SELECT @subTypeId=C.ChallengeSubTypeId,@userChallengeId=I.Id FROM INSERTED I
INNER JOIN dbo.tblChallenge C ON I.ChallengeId=C.ChallengeId;
UPDATE dbo.tblUserChallenges SET ResultUnit=CASE WHEN @subTypeId=3 THEN 'Reps'
											     WHEN @subTypeId=4 THEN 'Rounds'
											     WHEN @subTypeId=5 THEN 'Reps'
										         WHEN @subTypeId=7 THEN 'lbs'
											     WHEN @subTypeId=8 THEN 'lbs'
											     WHEN @subTypeId=10 THEN 'Miles'
											     WHEN @subTypeId=12 THEN 'Meters'
											     ELSE
											     ''
											     END
											     WHERE Id=@userChallengeId;
END

GO
--Summary: To update the Result Unit corresponding to Challenge SubType Id
--Developer Name: Arvind Kumar
--Create Date: 28th May, 2015

CREATE TRIGGER [dbo].[TRG_InsertUserChallenge]
ON [dbo].[tblUserChallenges]
AFTER INSERT AS
BEGIN
DECLARE @userChallengeId INT;
DECLARE @subTypeId INT;
DECLARE @result VARCHAR(20);

SELECT @subTypeId=C.ChallengeSubTypeId,@userChallengeId=I.Id, @result=I.Result FROM INSERTED I
INNER JOIN dbo.tblChallenge C ON I.ChallengeId=C.ChallengeId;
IF @subTypeId IN (1,2,6,9,11,12)
 BEGIN
	--We have to split the result and update the appropriate result in result column and also update the Result Unit
	DECLARE @tempResult VARCHAR(20);
	DECLARE @tempResultUnit VARCHAR(10);
	
	UPDATE dbo.tblUserChallenges SET Result=@result, ResultUnit='' WHERE Id=@userChallengeId;
 END
ELSE
 BEGIN
	UPDATE dbo.tblUserChallenges SET ResultUnit=CASE WHEN @subTypeId=3 THEN 'Reps'
											     WHEN @subTypeId=4 THEN 'Rounds'
											     WHEN @subTypeId=5 THEN 'Reps'
										         WHEN @subTypeId=7 THEN 'lbs'
											     WHEN @subTypeId=8 THEN 'lbs'
											     WHEN @subTypeId=10 THEN 'Miles'
											     WHEN @subTypeId=12 THEN 'Meters'
											     ELSE
											     ''
											     END
											     WHERE Id=@userChallengeId;
    END
 END

--DECLARE @codeName VarChar(100)
--SET @codeName = '1023 - Hydrabad'
 
-- SELECT CHARINDEX('-', @codeName)
 
--SELECT SUBSTRING(@codeName, CHARINDEX('-', @codeName) + 2, 100)
