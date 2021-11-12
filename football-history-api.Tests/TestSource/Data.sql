SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CompetitionRules](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
    [TotalPlaces] [tinyint] NOT NULL,
    [PromotionPlaces] [tinyint] NOT NULL,
    [RelegationPlaces] [tinyint] NOT NULL,
    [PlayOffPlaces] [tinyint] NOT NULL,
    [RelegationPlayOffPlaces] [tinyint] NOT NULL,
    [ReElectionPlaces] [tinyint] NOT NULL,
    [FailedReElectionPosition] [tinyint] NULL,
    [PointsForWin] [tinyint] NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Competitions](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](255) NOT NULL,
    [SeasonId] [bigint] NOT NULL,
    [Tier] [tinyint] NULL,
    [Region] [char](1) NULL,
    [RulesId] [bigint] NOT NULL,
    [Comment] [nvarchar](255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Positions](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
    [CompetitionId] [bigint] NOT NULL,
    [TeamId] [bigint] NOT NULL,
    [Position] [tinyint] NOT NULL,
    [Status] [nvarchar](255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Seasons](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
    [StartYear] [smallint] NOT NULL,
    [EndYear] [smallint] NOT NULL,
    [Comment] [nvarchar](255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY],
    UNIQUE NONCLUSTERED ([EndYear] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY],
    UNIQUE NONCLUSTERED ([StartYear] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Teams](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](255) NOT NULL,
    [Abbreviation] [char](3) NULL,
    [Notes] [nvarchar](max) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Deductions](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
    [CompetitionId] [bigint] NOT NULL,
    [PointsDeducted] [smallint] NOT NULL,
    [Reason] [nvarchar](255) NULL,
    [TeamId] [bigint] NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Matches](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
    [MatchDate] [date] NOT NULL,
    [CompetitionId] [bigint] NOT NULL,
    [RulesId] [bigint] NOT NULL,
    [HomeTeamId] [bigint] NOT NULL,
    [AwayTeamId] [bigint] NOT NULL,
    [HomeGoals] [tinyint] NOT NULL,
    [AwayGoals] [tinyint] NOT NULL,
    [HomeGoalsET] [tinyint] NOT NULL,
    [AwayGoalsET] [tinyint] NOT NULL,
    [HomePenaltiesTaken] [tinyint] NOT NULL,
    [HomePenaltiesScored] [tinyint] NOT NULL,
    [AwayPenaltiesTaken] [tinyint] NOT NULL,
    [AwayPenaltiesScored] [tinyint] NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[MatchRules](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
    [Type] [nvarchar](255) NOT NULL,
    [Stage] [nvarchar](255) NULL,
    [ExtraTime] [bit] NOT NULL,
    [Penalties] [bit] NOT NULL,
    [NumLegs] [tinyint] NULL,
    [AwayGoals] [bit] NOT NULL,
    [Replays] [bit] NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[Teams] ON
GO

INSERT [dbo].[Teams] ([Id], [Name], [Abbreviation], [Notes])
VALUES (1, N'Accrington Stanley', N'ACC', NULL)
GO
INSERT [dbo].[Teams] ([Id], [Name], [Abbreviation], [Notes])
VALUES (2, N'AFC Bournemouth', N'BOU', NULL)
GO
INSERT [dbo].[Teams] ([Id], [Name], [Abbreviation], [Notes])
VALUES (3, N'AFC Wimbledon', N'WBD', NULL)
GO
INSERT [dbo].[Teams] ([Id], [Name], [Abbreviation], [Notes])
VALUES (4, N'Aldershot Town', N'ALD', NULL)
GO
INSERT [dbo].[Teams] ([Id], [Name], [Abbreviation], [Notes])
VALUES (5, N'Arsenal', N'ARS', NULL)
GO
INSERT [dbo].[Teams] ([Id], [Name], [Abbreviation], [Notes])
VALUES (6, N'Aston Villa', N'AST', NULL)
GO
INSERT [dbo].[Teams] ([Id], [Name], [Abbreviation], [Notes])
VALUES (7, N'Barnet', N'BNT', NULL)
GO
INSERT [dbo].[Teams] ([Id], [Name], [Abbreviation], [Notes])
VALUES (8, N'Barnsley', N'BAR', NULL)
GO
INSERT [dbo].[Teams] ([Id], [Name], [Abbreviation], [Notes])
VALUES (9, N'Birmingham City', N'BIR', NULL)
GO
INSERT [dbo].[Teams] ([Id], [Name], [Abbreviation], [Notes])
VALUES (10, N'Blackburn Rovers', N'BLB', NULL)
GO
INSERT [dbo].[Teams] ([Id], [Name], [Abbreviation], [Notes])
VALUES (11, N'Blackpool', N'BLP', NULL)
GO
INSERT [dbo].[Teams] ([Id], [Name], [Abbreviation], [Notes])
VALUES (12, N'Bolton Wanderers', N'BOL', NULL)
GO
INSERT [dbo].[Teams] ([Id], [Name], [Abbreviation], [Notes])
VALUES (13, N'Boston United', N'BOS', NULL)
GO
INSERT [dbo].[Teams] ([Id], [Name], [Abbreviation], [Notes])
VALUES (14, N'Bradford City', N'BRA', NULL)
GO
INSERT [dbo].[Teams] ([Id], [Name], [Abbreviation], [Notes])
VALUES (15, N'Brentford', N'BRE', NULL)
GO
INSERT [dbo].[Teams] ([Id], [Name], [Abbreviation], [Notes])
VALUES (16, N'Brighton and Hove Albion', N'BHA', NULL)
GO
INSERT [dbo].[Teams] ([Id], [Name], [Abbreviation], [Notes])
VALUES (17, N'Bristol City', N'BRI', NULL)
GO
INSERT [dbo].[Teams] ([Id], [Name], [Abbreviation], [Notes])
VALUES (18, N'Bristol Rovers', N'BRR', NULL)
GO
INSERT [dbo].[Teams] ([Id], [Name], [Abbreviation], [Notes])
VALUES (19, N'Burnley', N'BNL', NULL)
GO
INSERT [dbo].[Teams] ([Id], [Name], [Abbreviation], [Notes])
VALUES (20, N'Burton Albion', N'BRT', NULL)
GO
INSERT [dbo].[Teams] ([Id], [Name], [Abbreviation], [Notes])
VALUES (21, N'Bury', N'BUR', NULL)
GO
    
SET IDENTITY_INSERT [dbo].[Teams] OFF
GO
    
SET IDENTITY_INSERT [dbo].[CompetitionRules] ON
GO
    
INSERT [dbo].[CompetitionRules] ([Id], [TotalPlaces], [PromotionPlaces], [RelegationPlaces], [PlayOffPlaces], [RelegationPlayOffPlaces], [ReElectionPlaces], [FailedReElectionPosition], [PointsForWin])
VALUES (1, 10, 0, 0, 0, 0, 0, NULL, 2)
GO
INSERT [dbo].[CompetitionRules] ([Id], [TotalPlaces], [PromotionPlaces], [RelegationPlaces], [PlayOffPlaces], [RelegationPlayOffPlaces], [ReElectionPlaces], [FailedReElectionPosition], [PointsForWin])
VALUES (2, 10, 0, 1, 0, 0, 0, NULL, 2)
GO
INSERT [dbo].[CompetitionRules] ([Id], [TotalPlaces], [PromotionPlaces], [RelegationPlaces], [PlayOffPlaces], [RelegationPlayOffPlaces], [ReElectionPlaces], [FailedReElectionPosition], [PointsForWin])
VALUES (3, 10, 1, 0, 0, 0, 0, null, 2)
GO
    
SET IDENTITY_INSERT [dbo].[CompetitionRules] OFF
GO
    
SET IDENTITY_INSERT [dbo].[Competitions] ON
GO
    
INSERT [dbo].[Competitions] ([Id], [Name], [SeasonId], [Tier], [Region], [RulesId], [Comment])
VALUES (1, N'First Division', 1, 1, NULL, 1, NULL)
GO
INSERT [dbo].[Competitions] ([Id], [Name], [SeasonId], [Tier], [Region], [RulesId], [Comment])
VALUES (2, N'First Division', 2, 1, NULL, 2, NULL)
GO
INSERT [dbo].[Competitions] ([Id], [Name], [SeasonId], [Tier], [Region], [RulesId], [Comment])
VALUES (3, N'Second Division', 2, 2, NULL, 3, NULL)
GO
    
SET IDENTITY_INSERT [dbo].[Competitions] OFF
GO
    
SET IDENTITY_INSERT [dbo].[Positions] ON
GO
    
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (1, 1, 1, 1, N'Champions')
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (2, 1, 2, 2, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (3, 1, 3, 3, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (4, 1, 4, 4, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (5, 1, 5, 5, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (6, 1, 6, 6, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (7, 1, 7, 7, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (8, 1, 8, 8, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (9, 1, 9, 8, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (10, 1, 10, 10, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (11, 2, 1, 1, N'Champions')
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (12, 2, 2, 2, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (13, 2, 3, 3, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (14, 2, 4, 4, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (15, 2, 5, 5, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (16, 2, 6, 6, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (17, 2, 7, 7, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (18, 2, 8, 8, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (19, 2, 9, 9, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (20, 2, 10, 10, N'Relegated')
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (21, 3, 11, 1, N'Champions')
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (22, 3, 12, 2, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (23, 3, 13, 3, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status]) 
VALUES (24, 3, 14, 4, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (25, 3, 15, 5, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (26, 3, 16, 6, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (27, 3, 17, 7, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (28, 3, 18, 8, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (29, 3, 19, 9, NULL)
GO
INSERT [dbo].[Positions] ([Id], [CompetitionId], [TeamId], [Position], [Status])
VALUES (30, 3, 20, 10, NULL)
GO
    
SET IDENTITY_INSERT [dbo].[Positions] OFF
GO
    
SET IDENTITY_INSERT [dbo].[Seasons] ON
GO
    
INSERT [dbo].[Seasons] ([Id], [StartYear], [EndYear], [Comment])
VALUES (1, 2000, 2001, NULL)
GO
INSERT [dbo].[Seasons] ([Id], [StartYear], [EndYear], [Comment])
VALUES (2, 2001, 2002, NULL)
GO
INSERT [dbo].[Seasons] ([Id], [StartYear], [EndYear], [Comment])
VALUES (3, 2002, 2003, NULL)
GO
    
SET IDENTITY_INSERT [dbo].[Seasons] OFF
GO

SET IDENTITY_INSERT [dbo].[Deductions] ON
GO

INSERT [dbo].[Deductions] ([Id], [CompetitionId], [PointsDeducted], [Reason], [TeamId])
VALUES (1, 1, 9, 'Financial irregularities', 1)
GO
INSERT [dbo].[Deductions] ([Id], [CompetitionId], [PointsDeducted], [Reason], [TeamId])
VALUES (2, 1, 12, 'Going into administration', 2)
GO
INSERT [dbo].[Deductions] ([Id], [CompetitionId], [PointsDeducted], [Reason], [TeamId])
VALUES (3, 3, 3, 'Failing to control players', 20)
GO

SET IDENTITY_INSERT [dbo].[Deductions] OFF
GO

SET IDENTITY_INSERT [dbo].[Matches] ON
GO

INSERT [dbo].[Matches] ([Id], [MatchDate], [CompetitionId], [RulesId], [HomeTeamId], [AwayTeamId], [HomeGoals], [AwayGoals], [HomeGoalsET], [AwayGoalsET], [HomePenaltiesTaken], [HomePenaltiesScored], [AwayPenaltiesTaken], [AwayPenaltiesScored])
VALUES (1, '2000-01-01', 1, 1, 1, 2, 1, 0, 0, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[Matches] ([Id], [MatchDate], [CompetitionId], [RulesId], [HomeTeamId], [AwayTeamId], [HomeGoals], [AwayGoals], [HomeGoalsET], [AwayGoalsET], [HomePenaltiesTaken], [HomePenaltiesScored], [AwayPenaltiesTaken], [AwayPenaltiesScored])
VALUES (2, '2000-01-02', 1, 2, 1, 2, 1, 0, 0, 0, 0, 0, 0, 0)
GO

SET IDENTITY_INSERT [dbo].[Matches] OFF
GO

SET IDENTITY_INSERT [dbo].[MatchRules] ON
GO

INSERT INTO [dbo].[MatchRules] ([Id], [Type], [Stage], [ExtraTime], [Penalties], [NumLegs], [AwayGoals], [Replays])
VALUES (1, 'League', NULL, 0, 0, NULL, 0, 0)
GO
INSERT INTO [dbo].[MatchRules] ([Id], [Type], [Stage], [ExtraTime], [Penalties], [NumLegs], [AwayGoals], [Replays])
VALUES (2, 'PlayOff', 'Final', 1, 1, NULL, 0, 0)
GO

SET IDENTITY_INSERT [dbo].[MatchRules] OFF
GO

ALTER TABLE [dbo].[Competitions] WITH CHECK
    ADD FOREIGN KEY([RulesId]) REFERENCES [dbo].[CompetitionRules] ([Id])
GO

ALTER TABLE [dbo].[Competitions] WITH CHECK
    ADD FOREIGN KEY([SeasonId]) REFERENCES [dbo].[Seasons] ([Id])
GO

ALTER TABLE [dbo].[Positions] WITH CHECK
    ADD FOREIGN KEY([CompetitionId]) REFERENCES [dbo].[Competitions] ([Id])
GO

ALTER TABLE [dbo].[Positions] WITH CHECK
    ADD FOREIGN KEY([TeamId]) REFERENCES [dbo].[Teams] ([Id])
GO

ALTER TABLE [dbo].[Deductions] WITH CHECK
    ADD FOREIGN KEY([CompetitionId]) REFERENCES [dbo].[Competitions] ([Id])
GO

ALTER TABLE [dbo].[Deductions] WITH CHECK
    ADD FOREIGN KEY([TeamId]) REFERENCES [dbo].[Teams] ([Id])
GO

ALTER TABLE [dbo].[Matches] WITH CHECK
    ADD FOREIGN KEY([AwayTeamId]) REFERENCES [dbo].[Teams] ([Id])
GO

ALTER TABLE [dbo].[Matches] WITH CHECK
    ADD FOREIGN KEY([CompetitionId]) REFERENCES [dbo].[Competitions] ([Id])
GO

ALTER TABLE [dbo].[Matches] WITH CHECK
    ADD FOREIGN KEY([HomeTeamId]) REFERENCES [dbo].[Teams] ([Id])
GO

ALTER TABLE [dbo].[Matches] WITH CHECK
    ADD FOREIGN KEY([RulesId]) REFERENCES [dbo].[MatchRules] ([Id])
GO
