import { grey } from "@mui/material/colors";
import { Datum, Serie } from "@nivo/line";
import { useContext } from "react";
import { ColorModeContext } from "../Contexts/ColorModeContext";
import { SeasonsContext } from "../Contexts/SeasonsContext";
import { HistoricalSeason } from "../Domain/Types";

const useHistoricalRecordGraph = (
  historicalSeasons: HistoricalSeason[],
  selectedRange: number[]
) => {
  const { seasons } = useContext(SeasonsContext);
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

  const getSelectedHistoricalSeasons = () =>
    historicalSeasons.filter(
      (x) =>
        x.seasonStartYear >= Math.min(...selectedRange) &&
        x.seasonStartYear <= Math.max(...selectedRange)
    );

  const getPositionSeries = (): Datum[] =>
    getSelectedHistoricalSeasons().map((s) => {
      return {
        x: s.seasonStartYear,
        y: s.historicalPosition?.overallPosition ?? null,
        status: s.historicalPosition?.status ?? null
      };
    });

  const getBoundarySeries = (boundaryIndex: number) => {
    return getSelectedHistoricalSeasons()
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
      data: getPositionSeries(),
      color: mode === "dark" ? "white" : "black"
    },
    {
      id: "tier1-tier2",
      data: getBoundarySeries(0),
      color: grey[500]
    },
    {
      id: "tier2-tier3",
      data: getBoundarySeries(1),
      color: grey[500]
    },
    {
      id: "tier3-tier4",
      data: getBoundarySeries(2),
      color: grey[500]
    },
    {
      id: "tier4-tier5",
      data: getBoundarySeries(3),
      color: grey[500]
    }
  ];

  // This represents an attempt to apply evenly-spaced y-axis grid lines
  const yValues = [1, 16, 31, 46, 61, 76, 92];

  const xValues = seasons
    .filter(
      (x) => x.startYear >= Math.min(...selectedRange) && x.startYear <= Math.max(...selectedRange)
    )
    .map((x) => x.startYear);

  return { series, yValues, xValues, getTheme };
};

export { useHistoricalRecordGraph };
