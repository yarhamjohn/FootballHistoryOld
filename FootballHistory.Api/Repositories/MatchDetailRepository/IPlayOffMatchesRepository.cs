using System.Collections.Generic;

namespace FootballHistory.Api.Repositories.MatchDetailRepository
{
    public interface IPlayOffMatchesRepository
    {
        List<MatchDetailModel> GetPlayOffMatches(int tier, string season);
        List<MatchDetailModel> GetPlayOffMatches(List<(int, string)> seasonTier);
    }
}
