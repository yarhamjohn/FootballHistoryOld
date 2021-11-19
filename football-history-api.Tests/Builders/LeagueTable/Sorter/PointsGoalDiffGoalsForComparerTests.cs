namespace football.history.api.Tests.Builders.LeagueTable.Sorter;

[TestFixture]
public class PointsGoalDiffGoalsForComparerTests
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
            new LeagueTableRow {Team = "Norwich City", Points     = 1, GoalDifference = 0, GoalsFor = 0, GoalAverage = 1},
            new LeagueTableRow {Team = "Newcastle United", Points = 0, GoalDifference = 1, GoalsFor = 1, GoalAverage = 0},
            1).SetName("when points are different");

        yield return new TestCaseData(
            new LeagueTableRow {Team = "Norwich City", Points     = 0, GoalDifference = 1, GoalsFor = 0, GoalAverage = 1},
            new LeagueTableRow {Team = "Newcastle United", Points = 0, GoalDifference = 0, GoalsFor = 1, GoalAverage = 0},
            1).SetName("when points are equal but goal diff is different");

        yield return new TestCaseData(
            new LeagueTableRow {Team = "Norwich City", Points     = 0, GoalDifference = 0, GoalsFor = 1, GoalAverage = 1},
            new LeagueTableRow {Team = "Newcastle United", Points = 0, GoalDifference = 0, GoalsFor = 0, GoalAverage = 0},
            1).SetName("when points and goal diff are equal but goals for is different");
            
        yield return new TestCaseData(
            new LeagueTableRow {Team = "Norwich City", Points     = 0, GoalDifference = 0, GoalsFor = 0, GoalAverage = 1},
            new LeagueTableRow {Team = "Newcastle United", Points = 0, GoalDifference = 0, GoalsFor = 0, GoalAverage = 0},
            -1).SetName("when points and goal diff and goals for are equal");
            
        yield return new TestCaseData(
            new LeagueTableRow {Team = "Norwich City", Points = 0, GoalDifference = 0, GoalsFor = 0, GoalAverage = 1},
            new LeagueTableRow {Team = "Norwich City", Points = 0, GoalDifference = 0, GoalsFor = 0, GoalAverage = 0},
            0).SetName("when only non-sorting fields are different");
    }

    [TestCaseSource(nameof(Compare_variations))]
    public void Compare_returns_correct_comparison_result(
        LeagueTableRow? x,
        LeagueTableRow? y,
        int expected)
    {
        var comparer = new PointsGoalDiffGoalsForComparer();

        var actual = comparer.Compare(x, y);

        actual.Should().Be(expected);
    }
}