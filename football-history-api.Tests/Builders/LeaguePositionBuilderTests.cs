namespace football.history.api.Tests.Builders;

[TestFixture]
public class LeaguePositionBuilderTests
{
    private const int CompetitionId = 1;
    private const int TeamId = 2;
    
    [Test]
    public void GetPositions_returns_empty_list_if_no_matching_competition()
    {
        var mockCompetitionRepository = new Mock<ICompetitionRepository>();
        mockCompetitionRepository
            .Setup(x => x.GetCompetition(CompetitionId))
            .Returns((CompetitionModel?) null);
        
        var mockMatchRepository = new Mock<IMatchRepository>();
        var mockPointDeductionRepository = new Mock<IPointDeductionRepository>();
        var mockLeagueTableBuilder = new Mock<ILeagueTableBuilder>();
        
        var builder = new LeaguePositionBuilder(
            mockLeagueTableBuilder.Object, 
            mockMatchRepository.Object,
            mockPointDeductionRepository.Object,
            mockCompetitionRepository.Object);
        
        var positions = builder.GetPositions(CompetitionId, TeamId);

        positions.Should().BeEmpty();
    }
    
    
    [Test]
    public void GetPositions_returns_empty_list_if_no_matches_in_competition()
    {
        var mockCompetitionRepository = new Mock<ICompetitionRepository>();
        mockCompetitionRepository
            .Setup(x => x.GetCompetition(CompetitionId))
            .Returns(GetTestCompetitionModel());
        
        var mockMatchRepository = new Mock<IMatchRepository>();
        mockMatchRepository
            .Setup(x => x.GetLeagueMatches(CompetitionId))
            .Returns(GetTestMatchModels());
        
        var mockPointDeductionRepository = new Mock<IPointDeductionRepository>();
        var mockLeagueTableBuilder = new Mock<ILeagueTableBuilder>();
        
        var builder = new LeaguePositionBuilder(
            mockLeagueTableBuilder.Object, 
            mockMatchRepository.Object,
            mockPointDeductionRepository.Object,
            mockCompetitionRepository.Object);
        
        var positions = builder.GetPositions(CompetitionId, TeamId);

        positions.Should().BeEmpty();
    }

    [Test]
    public void GetPositions_returns_correct_positions_for_all_expected_dates()
    {
        var testCompetitionModel = GetTestCompetitionModel();
        var testMatchModels = GetTestMatchModels(
            new DateTime(2000, 1, 10), 
            new DateTime(2000, 1, 12),
            new DateTime(2000, 1, 13));

        var testPointDeductionModels = GetTestPointDeductionModels();
        
        var mockCompetitionRepository = new Mock<ICompetitionRepository>();
        mockCompetitionRepository
            .Setup(x => x.GetCompetition(CompetitionId))
            .Returns(testCompetitionModel);
        
        var mockMatchRepository = new Mock<IMatchRepository>();

        mockMatchRepository
            .Setup(x => x.GetLeagueMatches(CompetitionId))
            .Returns(testMatchModels);
        
        var mockPointDeductionRepository = new Mock<IPointDeductionRepository>();
        mockPointDeductionRepository
            .Setup(x => x.GetPointDeductions(CompetitionId))
            .Returns(testPointDeductionModels);

        var mockLeagueTableBuilder = new Mock<ILeagueTableBuilder>();
        mockLeagueTableBuilder
            .Setup(x => x.BuildPartialLeagueTable(
                testCompetitionModel,
                testMatchModels,
                It.IsAny<DateTime>(),
                testPointDeductionModels))
            .Returns<CompetitionModel, MatchModel[], DateTime, PointDeductionModel[]>((_, _, date, _) =>
                new LeagueTable(
                    new LeagueTableRow[] { new() { TeamId = TeamId, Position = date.Day } },
                    GetTestCompetition()));

        var builder = new LeaguePositionBuilder(
            mockLeagueTableBuilder.Object,
            mockMatchRepository.Object,
            mockPointDeductionRepository.Object,
            mockCompetitionRepository.Object);

        var positions = builder.GetPositions(CompetitionId, TeamId);

        positions.Should().HaveCount(6);
        positions.Should().BeEquivalentTo(new LeaguePosition[]
        {
            new(new DateTime(2000, 1, 9), 9),
            new(new DateTime(2000, 1, 10), 10),
            new(new DateTime(2000, 1, 11), 11),
            new(new DateTime(2000, 1, 12), 12),
            new(new DateTime(2000, 1, 13), 13),
            new(new DateTime(2000, 1, 14), 14)
        });
    }

    private static Competition GetTestCompetition()
    {
        return new Competition(
            Id: CompetitionId,
            Name: "First Division",
            Season: new (Id: 1, StartYear: 2000, EndYear: 2001),
            Level: "1",
            Comment: null,
            Rules: new (
                PointsForWin: 1,
                TotalPlaces: 1,
                PromotionPlaces: 1,
                RelegationPlaces: 1,
                PlayOffPlaces: 1,
                RelegationPlayOffPlaces: 1,
                ReElectionPlaces: 1,
                FailedReElectionPosition: null));
    }

    private static CompetitionModel GetTestCompetitionModel()
        => new(
            Id: CompetitionId,
            Name: "First Division",
            SeasonId: 1,
            StartYear: 2000,
            EndYear: 2001,
            Tier: 1,
            Region: null,
            Comment: null,
            PointsForWin: 1,
            TotalPlaces: 1,
            PromotionPlaces: 1,
            RelegationPlaces: 1,
            PlayOffPlaces: 1,
            RelegationPlayOffPlaces: 1,
            ReElectionPlaces: 1,
            FailedReElectionPosition: null);

    private static MatchModel[] GetTestMatchModels(params DateTime[] matchDates)
        => matchDates.Select(
                date => new MatchModel(
                    Id: 1,
                    MatchDate: date,
                    CompetitionId: CompetitionId,
                    CompetitionName: "Premier League",
                    CompetitionStartYear: 2000,
                    CompetitionEndYear: 2001,
                    CompetitionTier: 1,
                    CompetitionRegion: null,
                    RulesType: "League",
                    RulesStage: null,
                    RulesExtraTime: false,
                    RulesPenalties: false,
                    RulesNumLegs: null,
                    RulesAwayGoals: false,
                    RulesReplays: false,
                    HomeTeamId: 1,
                    HomeTeamName: "Norwich City",
                    HomeTeamAbbreviation: "NOR",
                    AwayTeamId: 2,
                    AwayTeamName: "Newcastle United",
                    AwayTeamAbbreviation: "NEW",
                    HomeGoals: 1,
                    AwayGoals: 0,
                    HomeGoalsExtraTime: 0,
                    AwayGoalsExtraTime: 0,
                    HomePenaltiesTaken: 0,
                    HomePenaltiesScored: 0,
                    AwayPenaltiesTaken: 0,
                    AwayPenaltiesScored: 0))
            .ToArray();

    private static PointDeductionModel[] GetTestPointDeductionModels()
        => new PointDeductionModel[]
        {
            new(Id: 1,
                CompetitionId: CompetitionId,
                PointsDeducted: 1,
                TeamId: TeamId,
                TeamName: "Newcastle United",
                Reason: "Financial irregularities")
        };
}