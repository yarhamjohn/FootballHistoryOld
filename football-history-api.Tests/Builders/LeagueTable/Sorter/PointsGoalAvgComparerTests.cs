namespace football.history.api.Tests.Builders.LeagueTable.Sorter;

[TestFixture]
public class PointsGoalAvgComparerTests
{
    private static IEnumerable Compare_variations()
    {
        yield return new TestCaseData(null, null, 0).SetName("when both x and y are null");
        yield return new TestCaseData(null, new LeagueTableRow(), -1).SetName("when only x is null");
        yield return new TestCaseData(new LeagueTableRow(), null, 1).SetName("when only y is null");

        yield return new TestCaseData(
            new LeagueTableRow(),
            new LeagueTableRow(),
            0).SetName("when x and y are equivalent");

        yield return new TestCaseData(
            new LeagueTableRow {Team = "Norwich City", Points     = 1, GoalAverage = 0, GoalDifference = 0},
            new LeagueTableRow {Team = "Newcastle United", Points = 0, GoalAverage = 1, GoalDifference = 1},
            1).SetName("when points are different");

        yield return new TestCaseData(
            new LeagueTableRow {Team = "Norwich City", Points     = 0, GoalAverage = 1, GoalDifference = 0},
            new LeagueTableRow {Team = "Newcastle United", Points = 0, GoalAverage = 0, GoalDifference = 1},
            1).SetName("when points are equal but goal average is different and not null");

        yield return new TestCaseData(
            new LeagueTableRow {Team = "Norwich City", Points     = 0, GoalAverage = null, GoalDifference = 0},
            new LeagueTableRow {Team = "Newcastle United", Points = 0, GoalAverage = null, GoalDifference = 1},
            -1).SetName("when points are equal but goal average are both null");
            
        yield return new TestCaseData(
            new LeagueTableRow {Team = "Norwich City", Points     = 0, GoalAverage = null, GoalDifference = 0},
            new LeagueTableRow {Team = "Newcastle United", Points = 0, GoalAverage = 0, GoalDifference = 1},
            -1).SetName("when points are equal but goal average for first team is null");
            
        yield return new TestCaseData(
            new LeagueTableRow {Team = "Norwich City", Points     = 0, GoalAverage = 0, GoalDifference = 0},
            new LeagueTableRow {Team = "Newcastle United", Points = 0, GoalAverage = null, GoalDifference = 1},
            1).SetName("when points are equal but goal average for second team is null");

        yield return new TestCaseData(
            new LeagueTableRow {Team = "Norwich City", Points     = 0, GoalAverage = 0, GoalDifference = 0},
            new LeagueTableRow {Team = "Newcastle United", Points = 0, GoalAverage = 0, GoalDifference = 1},
            -1).SetName("when points and goal average are equal");
            
        yield return new TestCaseData(
            new LeagueTableRow {Team = "Norwich City", Points = 0, GoalAverage = 0, GoalDifference = 0},
            new LeagueTableRow {Team = "Norwich City", Points = 0, GoalAverage = 0, GoalDifference = 1},
            0).SetName("when only non-sorting fields are different");
    }

    [TestCaseSource(nameof(Compare_variations))]
    public void Compare_returns_correct_comparison_result(
        LeagueTableRow? x,
        LeagueTableRow? y,
        int expected)
    {
        var comparer = new PointsGoalAvgComparer();

        var actual = comparer.Compare(x, y);

        actual.Should().Be(expected);
    }
}