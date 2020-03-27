using System.Collections.Generic;
using System.Data.Common;

namespace FootballHistoryTest.Api.Repositories.PointDeductions
{
    public interface IPointsDeductionRepository
    {
        List<PointsDeductionModel> GetPointsDeductionModels(int seasonStartYear, int tier);
        List<PointsDeductionModel> GetPointsDeductionModels(List<int> seasonStartYears, List<int> tiers);
    }
}
