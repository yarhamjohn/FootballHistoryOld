import { SeasonDateRange } from "./index";
import { HistoricalSeason } from "../../shared/useFetchHistoricalRecord";

const useHistoricalPositionsGraph = (seasons: HistoricalSeason[], range: SeasonDateRange) => {
  const getSeasonStartYears = (start: number, end: number) =>
    Array.from({ length: end - start }, (v, k) => k + start);

  const getPositionSeries = (seasons: HistoricalSeason[], range: SeasonDateRange) =>
    getSeasonStartYears(range.startYear - 1, range.endYear + 2).map((d) => {
      return {
        x: d,
        y: seasons.some((p) => p.seasonStartYear === d)
          ? seasons.filter((p) => p.seasonStartYear === d)[0].historicalPosition?.overallPosition ??
            null
          : null,
      };
    });

  const getBoundarySeries = (boundaryIndex: number, seasons: HistoricalSeason[]) => {
    return seasons
      .sort((i, j) => i.seasonStartYear - j.seasonStartYear)
      .map((s) => {
        return {
          x: s.seasonStartYear,
          y: s.boundaries.length > boundaryIndex ? s.boundaries[boundaryIndex] + 0.5 : null,
        };
      });
  };

  const series = [
    {
      id: "positions",
      data: getPositionSeries(seasons, range),
    },
    {
      id: "tier1-tier2",
      data: getBoundarySeries(0, seasons),
    },
    {
      id: "tier2-tier3",
      data: getBoundarySeries(1, seasons),
    },
    {
      id: "tier3-tier4",
      data: getBoundarySeries(2, seasons),
    },
  ];

  const colors = ["black", "#75B266", "#BFA67F", "#B26694"];

  // TODO: This should be calculated dynamically
  const yValues = [1, 16, 31, 46, 61, 76, 92];

  return { series, colors, yValues };
};

export { useHistoricalPositionsGraph };
