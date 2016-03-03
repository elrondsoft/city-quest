USE [Your_CityQuest_DataBaseName]
GO

DECLARE @InitDate VARCHAR(50);
DECLARE @InitCityQuestDataBase VARCHAR(50);
SET @InitDate = '2016-03-01 12:00:00';
SET @InitCityQuestDataBase = 'CityQuestInitializeTransaction';

BEGIN TRANSACTION @InitCityQuestDataBase;

	INSERT INTO [dbo].[Users] ([Name], [Surname], [UserName], [Password], [EmailAddress], [PhoneNumber], [IsEmailConfirmed], [IsDeleted], [CreationTime])     
		VALUES ('System', 'Administrator', 'Admin', 
				'AKOBPkJ5gKDI/GKRh5RfSkkO/vNIqKXEXCGzgDwSis6rro3O+WRHh9LAa02fcCRiEA==', --P@ssw0rd
				'SystemAdministrator@CityQuest.com', '+380123456789', 1, 0, @InitDate)
	
	INSERT INTO [dbo].[Roles] ([Name], [DisplayName], [IsStatic], [IsDeleted], [IsDefault], [CreationTime])
	     VALUES
	           ('Admin', 'Administrator', 1, 0, 0, @InitDate), 
			   ('GameMaster', 'Game master', 1, 0, 0, @InitDate), 
			   ('Player', 'Player', 1, 0, 1, @InitDate)
	
	INSERT INTO [dbo].[UserRoles] ([UserId], [RoleId], [IsDeleted], [CreationTime])
	     VALUES (1, 1, 0, @InitDate)

	INSERT INTO [dbo].[PermissionSettings]
           ([Name], [IsGranted], [IsDeleted], [CreationTime], [RoleId], [Discriminator])
		VALUES ('CanAll', 1, 0, @InitDate, 1, 'RolePermissionSetting')

	INSERT INTO [dbo].[Divisions] ([Name], [Description], [IsDefault], [IsActive], [IsDeleted], [CreationTime])
		VALUES ('Amateur division', 'Division for amateurs.', 1, 1, 0, @InitDate)

	INSERT INTO [dbo].[Teams] ([DivisionId], [Name], [Description], [Slogan], [IsDefault], [IsActive], [IsDeleted], [CreationTime])
		VALUES (1, 'Amateur team', 'Team for amateurs.', 'We are amateurs!', 1, 1, 0, @InitDate)

	INSERT INTO [dbo].[PlayerCareers] ([UserId], [TeamId], [CareerDateStart], [IsCaptain], [IsActive], [IsDeleted], [CreationTime])
		VALUES (1, 1, @InitDate, 1, 1, 0, @InitDate)

	INSERT INTO [dbo].[ConditionTypes] ([Name], [Description], [IsDefault], [IsActive], [IsDeleted], [CreationTime])
		VALUES 
			('Condition_JustInputCode', 'Condition that requers code to be passed.', 1, 1, 0, @InitDate),
			('Condition_Time', 'Condition that requers code to be passed.', 0, 1, 0, @InitDate)

	INSERT INTO [dbo].[GameStatus]
           ([Name], [Description], [NextAllowedStatusNames], [IsDefault], [IsDeleted], [CreationTime])
		VALUES 
			('GameStatus_Planned', 'This status means that the game is planned.', 'GameStatus_InProgress', 1, 0, @InitDate), 
			('GameStatus_InProgress', 'This status means that the game is in progress.', 'GameStatus_Paused,GameStatus_Completed', 0, 0, @InitDate), 
			('GameStatus_Paused', 'This status means that the game is paused.', 'GameStatus_InProgress', 0, 0, @InitDate), 
			('GameStatus_Completed', 'This status means that the game is completed.', '', 0, 0, @InitDate)

	INSERT INTO [dbo].[GameTaskTypes]
           ([Name], [Description], [IsActive], [IsDefault], [IsDeleted], [CreationTime])
		VALUES 
			('Simple game's task', 'Default game's task type. Using to create standart task.', 1, 1, 0, @InitDate)

COMMIT TRANSACTION @InitCityQuestDataBase;
GO