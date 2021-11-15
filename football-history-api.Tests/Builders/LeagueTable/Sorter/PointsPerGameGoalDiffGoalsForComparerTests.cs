using System.Collections;
using FluentAssertions;
using football.history.api.Builders;
using NUnit.Framework;

namespace football.history.api.Tests.Builders.LeagueTable.Sorter;

[TestFixture]
public class PointsPerGameGoalDiffGoalsForComparerTests
{
    private static IEnumerable Compare_variations()
    {
        yield return new TestCaseData(null, null, 0).SetName("when both x and y are null");
        yield return new TestCaseData(null, new LeagueTableRowDto(), -1).SetName("when only x is null");
        yield return new TestCaseData(new LeagueTableRowDto(), null, 1).SetName("when only y is null");

        yield return new TestCaseData(
            new LeagueTableRowDto(),
            new LeagueTableRowDto(),
            0).SetName("when x and y are equivalent");

        yield return new TestCaseData(
            new LeagueTableRowDto {Team = "Norwich City", PointsPerGame     = 1, GoalDifference = 0, GoalsFor = 0, GoalAverage = 0},
            new LeagueTableRowDto {Team = "Newcastle United", PointsPerGame = 0, GoalDifference = 1, GoalsFor = 1, GoalAverage = 1},
            1).SetName("when points per game are different and not null");

        yield return new TestCaseData(
            new LeagueTableRowDto {Team = "Norwich City", PointsPerGame     = null, GoalDifference = 0, GoalsFor = 0, GoalAverage = 0},
            new LeagueTableRowDto {Team = "Newcastle United", PointsPerGame = 0, GoalDifference = 1, GoalsFor = 1, GoalAverage = 1},
            -1).SetName("when points per game for the first team is null");
            
        yield return new TestCaseData(
            new LeagueTableRowDto {Team = "Norwich City", PointsPerGame     = 0, GoalDifference = 0, GoalsFor = 0, GoalAverage = 0},
            new LeagueTableRowDto {Team = "Newcastle United", PointsPerGame = null, GoalDifference = 1, GoalsFor = 1, GoalAverage = 1},
            1).SetName("when points per game for the second team is null");
            
        yield return new TestCaseData(
            new LeagueTableRowDto {Team = "Norwich City", PointsPerGame     = null, GoalDifference = 0, GoalsFor = 0, GoalAverage = 0},
            new LeagueTableRowDto {Team = "Newcastle United", PointsPerGame = null, GoalDifference = 1, GoalsFor = 1, GoalAverage = 1},
            -1).SetName("when points per game are both null");

        yield return new TestCaseData(
            new LeagueTableRowDto {Team = "Norwich City", PointsPerGame     = 0, GoalDifference = 1, GoalsFor = 0, GoalAverage = 0},
            new LeagueTableRowDto {Team = "Newcastle United", PointsPerGame = 0, GoalDifference = 0, GoalsFor = 1, GoalAverage = 1},
            1).SetName("when points per game are equal but goal diff is different");

        yield return new TestCaseData(
            new LeagueTableRowDto {Team = "Norwich City", PointsPerGame     = 0, GoalDifference = 0, GoalsFor = 1, GoalAverage = 0},
            new LeagueTableRowDto {Team = "Newcastle United", PointsPerGame = 0, GoalDifference = 0, GoalsFor = 0, GoalAverage = 1},
            1).SetName("when points per game and goal diff are equal but goals for is different");
            
        yield return new TestCaseData(
            new LeagueTableRowDto {Team = "Norwich City", PointsPerGame     = 0, GoalDifference = 0, GoalsFor = 0, GoalAverage = 0},
            new LeagueTableRowDto {Team = "Newcastle United", PointsPerGame = 0, GoalDifference = 0, GoalsFor = 0, GoalAverage = 1},
            -1).SetName("when points per game and goal diff and goals for are equal");
            
        yield return new TestCaseData(
            new LeagueTableRowDto {Team = "Norwich City", PointsPerGame = 0, GoalDifference = 0, GoalsFor = 0, GoalAverage = 0},
            new LeagueTableRowDto {Team = "Norwich City", PointsPerGame = 0, GoalDifference = 0, GoalsFor = 0, GoalAverage = 1},
            0).SetName("when only non-sorting fields are different");
    }

    [TestCaseSource(nameof(Compare_variations))]
    public void Compare_returns_correct_comparison_result(
        LeagueTableRowDto? x,
        LeagueTableRowDto? y,
        int expected)
    {
        var comparer = new PointsPerGameGoalDiffGoalsForComparer();

        var actual = comparer.Compare(x, y);

        actual.Should().Be(expected);
    }
}