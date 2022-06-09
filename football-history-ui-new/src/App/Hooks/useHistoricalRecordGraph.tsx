import { amber, blue, green, red } from "@mui/material/colors";
import { Datum, Serie } from "@nivo/line";
import { useContext } from "react";
import { ColorModeContext } from "../Contexts/ColorModeContext";
import { HistoricalSeason } from "../Domain/Types";

const useHistoricalRecordGraph = (seasons: HistoricalSeason[], selectedRange: number[]) => {
  const { mode } = useContext(ColorModeContext);

  const getTheme = () => {
    if (mode === "light") return;
    return {
      textColor: "#999",
      grid: {
        line: {
          stroke: "#333"
        }
      },
      crosshair: { line: { stroke: "#999" } }
    };
  };
  const getSeasonStartYears = (start: number, end: number) =>
    Array.from({ length: end - start }, (v, k) => k + start);

  const getPositionSeries = (seasons: HistoricalSeason[], range: number[]): Datum[] =>
    getSeasonStartYears(range[0] - 1, range[1] + 2).map((d) => {
      const test: Datum = {
        x: d,
        y: seasons.some((p) => p.seasonStartYear === d)
          ? seasons.filter((p) => p.seasonStartYear === d)[0].historicalPosition?.overallPosition ??
            null
          : null
      };

      test.status = seasons.some((p) => p.seasonStartYear === d)
        ? seasons.filter((p) => p.seasonStartYear === d)[0].historicalPosition?.status ?? null
        : null;

      return test;
    });

  const getBoundarySeries = (boundaryIndex: number, seasons: HistoricalSeason[]) => {
    return seasons
      .sort((i, j) => i.seasonStartYear - j.seasonStartYear)
      .map((s) => {
        return {
          x: s.seasonStartYear,
          y: s.boundaries.length > boundaryIndex ? s.boundaries[boundaryIndex] + 0.5 : null
        };
      });
  };

  const series: Serie[] = [
    {
      id: "positions",
      data: getPositionSeries(seasons, selectedRange)
    },
    {
      id: "tier1-tier2",
      data: getBoundarySeries(0, seasons)
    },
    {
      id: "tier2-tier3",
      data: getBoundarySeries(1, seasons)
    },
    {
      id: "tier3-tier4",
      data: getBoundarySeries(2, seasons)
    },
    {
      id: "tier4-tier5",
      data: getBoundarySeries(3, seasons)
    }
  ];

  const colors = [mode === "dark" ? "white" : "black", green[500], amber[500], red[500], blue[500]];

  // TODO: This should be calculated dynamically
  const yValues = [1, 16, 31, 46, 61, 76, 92];

  return { series, colors, yValues, getTheme };
};

export { useHistoricalRecordGraph };
