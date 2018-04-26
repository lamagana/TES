CREATE SCHEMA `tesproject`;

USE tesproject;

CREATE TABLE [dbo].[RoleType] (
    [RoleID]   INT          IDENTITY (0, 1) NOT NULL,
    [RoleType] VARCHAR (30) NOT NULL,
    PRIMARY KEY CLUSTERED ([RoleID] ASC)
);

CREATE TABLE [dbo].[Users] (
    [UserId]    INT           IDENTITY (1, 1) NOT NULL,
    [FirstName] VARCHAR (50)  NULL,
    [LastName]  VARCHAR (50)  NULL,
    [Password]  VARCHAR (255) NOT NULL,
    [Email]     VARCHAR (50)  NOT NULL,
    [RoleID]    INT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC),
    FOREIGN KEY ([RoleID]) REFERENCES [dbo].[RoleType] ([RoleID])
);

CREATE TABLE [dbo].[RoleRequest] (
    [RoleRequestId]   INT IDENTITY (1, 1) NOT NULL,
    [UserId]          INT NOT NULL,
    [CurrentRoleId]   INT NOT NULL,
    [RequestedRoleId] INT NOT NULL,
    [Completed]       BIT DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([RoleRequestId] ASC)
);

CREATE TABLE [dbo].[Projects] (
    [ProjectId]   INT          IDENTITY (1, 1) NOT NULL,
    [Description] VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([ProjectId] ASC)
);

CREATE TABLE [dbo].[Groups] (
    [GroupId]   INT          IDENTITY (1, 1) NOT NULL,
    [ProjectId] INT          NOT NULL,
    [GroupName] VARCHAR (30) NOT NULL,
    PRIMARY KEY CLUSTERED ([GroupId] ASC),
    FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Projects] ([ProjectId])
);

CREATE TABLE [dbo].[Groups_Users] (
    [Group_UserId] INT IDENTITY (1, 1) NOT NULL,
    [GroupId]      INT NULL,
    [UserId]       INT NULL,
    PRIMARY KEY CLUSTERED ([Group_UserId] ASC),
    FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Groups] ([GroupId]),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
);

CREATE TABLE [dbo].[TimeSheet] (
    [TimeSheetId] INT      IDENTITY (1, 1) NOT NULL,
    [UserId]      INT      NOT NULL,
    [ProjectId]   INT      NOT NULL,
    [GroupId]     INT      NOT NULL,
    [StartTime]   DATETIME NULL,
    [StopTime]    DATETIME NULL,
    PRIMARY KEY CLUSTERED ([TimeSheetId] ASC),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]),
    FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Projects] ([ProjectId]),
    FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Groups] ([GroupId])
);

SET IDENTITY_INSERT [dbo].[RoleType] ON
INSERT INTO [dbo].[RoleType] ([RoleID], [RoleType]) VALUES (0, N'Unassigned')
INSERT INTO [dbo].[RoleType] ([RoleID], [RoleType]) VALUES (1, N'Admin')
INSERT INTO [dbo].[RoleType] ([RoleID], [RoleType]) VALUES (2, N'Teacher')
INSERT INTO [dbo].[RoleType] ([RoleID], [RoleType]) VALUES (3, N'Student')
SET IDENTITY_INSERT [dbo].[RoleType] OFF

SET IDENTITY_INSERT [dbo].[Users] ON
INSERT INTO [dbo].[Users] ([UserId], [FirstName], [LastName], [Password], [Email], [RoleID]) VALUES (1, N'Admin', N' ', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'admin@weber.edu', 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
