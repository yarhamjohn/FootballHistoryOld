USE [master]
GO
/****** Object:  Database [Football]    Script Date: 13/11/2018 16:39:06 ******/
CREATE DATABASE [Football]
GO
ALTER DATABASE [Football] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Football].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Football] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Football] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Football] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Football] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Football] SET ARITHABORT OFF 
GO
ALTER DATABASE [Football] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Football] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Football] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Football] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Football] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Football] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Football] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Football] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Football] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Football] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Football] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [Football] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Football] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [Football] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Football] SET  MULTI_USER 
GO
ALTER DATABASE [Football] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Football] SET ENCRYPTION ON
GO
ALTER DATABASE [Football] SET QUERY_STORE = ON
GO
ALTER DATABASE [Football] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 7), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 10, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO)
GO
USE [Football]
GO
/****** Object:  UserDefinedFunction [dbo].[CalculatePoints]    Script Date: 13/11/2018 16:39:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[CalculatePoints](@NumWins INT, @NumDraws INT, @CompetitionId BIGINT)
RETURNS INT
AS
BEGIN
	DECLARE @Points TINYINT;
	SELECT @Points = (@NumWins * PointsForWin) + @NumDraws
	FROM dbo.Competitions
	WHERE @CompetitionId = Competitions.Id
	RETURN @Points
END
GO
/****** Object:  Table [dbo].[Clubs]    Script Date: 13/11/2018 16:39:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clubs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Abbreviation] [char](3) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Divisions]    Script Date: 13/11/2018 16:39:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Divisions](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Tier] [tinyint] NOT NULL,
	[From] [smallint] NOT NULL,
	[To] [smallint] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LeagueStatuses]    Script Date: 13/11/2018 16:39:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LeagueStatuses](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DivisionId] [bigint] NOT NULL,
	[Season] [char](11) NOT NULL,
	[TotalPlaces] [tinyint] NOT NULL,
	[PromotionPlaces] [tinyint] NOT NULL,
	[PlayOffPlaces] [tinyint] NOT NULL,
	[RelegationPlaces] [tinyint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Matches]    Script Date: 13/11/2018 16:39:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Matches](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MatchDate] [date] NOT NULL,
	[HomeGoals] [tinyint] NOT NULL,
	[AwayGoals] [tinyint] NOT NULL,
	[DivisionId] [bigint] NOT NULL,
	[HomeClubId] [bigint] NOT NULL,
	[AwayClubId] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PointDeductions]    Script Date: 13/11/2018 16:39:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PointDeductions](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DivisionId] [bigint] NOT NULL,
	[Season] [char](11) NOT NULL,
	[PointsDeducted] [tinyint] NOT NULL,
	[Reason] [nvarchar](255) NULL,
	[ClubId] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Matches]  WITH CHECK ADD  CONSTRAINT [FK_Matches_Away_Clubs] FOREIGN KEY([AwayClubId])
REFERENCES [dbo].[Clubs] ([Id])
GO
ALTER TABLE [dbo].[Matches] CHECK CONSTRAINT [FK_Matches_Away_Clubs]
GO
ALTER TABLE [dbo].[Matches]  WITH CHECK ADD  CONSTRAINT [FK_Matches_Divisions] FOREIGN KEY([DivisionId])
REFERENCES [dbo].[Matches] ([Id])
GO
ALTER TABLE [dbo].[Matches] CHECK CONSTRAINT [FK_Matches_Divisions]
GO
ALTER TABLE [dbo].[Matches]  WITH CHECK ADD  CONSTRAINT [FK_Matches_Home_Clubs] FOREIGN KEY([HomeClubId])
REFERENCES [dbo].[Clubs] ([Id])
GO
ALTER TABLE [dbo].[Matches] CHECK CONSTRAINT [FK_Matches_Home_Clubs]
GO
ALTER TABLE [dbo].[PointDeductions]  WITH CHECK ADD  CONSTRAINT [FK_PointDeductions_Clubs] FOREIGN KEY([ClubId])
REFERENCES [dbo].[Clubs] ([Id])
GO
ALTER TABLE [dbo].[PointDeductions] CHECK CONSTRAINT [FK_PointDeductions_Clubs]
GO
ALTER TABLE [dbo].[PointDeductions]  WITH CHECK ADD  CONSTRAINT [FK_PointDeductions_Divisions] FOREIGN KEY([DivisionId])
REFERENCES [dbo].[Divisions] ([Id])
GO
ALTER TABLE [dbo].[PointDeductions] CHECK CONSTRAINT [FK_PointDeductions_Divisions]
GO
USE [master]
GO
ALTER DATABASE [Football] SET  READ_WRITE 
GO
