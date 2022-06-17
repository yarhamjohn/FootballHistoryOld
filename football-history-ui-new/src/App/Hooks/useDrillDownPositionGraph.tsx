import { blue, green, red } from "@mui/material/colors";
import { useContext } from "react";
import { ColorModeContext } from "../Contexts/ColorModeContext";
import { CompetitionRules, LeaguePosition } from "../Domain/Types";

const useDrillDownPositionGraph = (
  positions: LeaguePosition[],
  rules: CompetitionRules,
  numRows: number
) => {
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

  const getMinDate = () =>
    new Date(
      Math.min.apply(
        null,
        positions.map((p) => new Date(p.date).valueOf())
      )
    );

  const getMaxDate = () =>
    new Date(
      Math.max.apply(
        null,
        positions.map((p) => new Date(p.date).valueOf())
      )
    );

  const getPositionData = () =>
    positions
      .map((p) => {
        return { x: new Date(p.date), y: p.position };
      })
      .sort();

  const getData = () => {
    const data = [
      {
        id: "positions",
        data: getPositionData()
      }
    ];

    const colors = mode === "dark" ? ["white"] : ["black"];

    if (rules.promotionPlaces > 0) {
      data.push({
        id: "promotion",
        data: [
          { x: getMinDate(), y: rules.promotionPlaces },
          { x: getMaxDate(), y: rules.promotionPlaces }
        ]
      });

      colors.push(green[500]);
    }

    if (rules.playOffPlaces > 0) {
      data.push({
        id: "playOffs",
        data: [
          { x: getMinDate(), y: rules.promotionPlaces + rules.playOffPlaces },
          { x: getMaxDate(), y: rules.promotionPlaces + rules.playOffPlaces }
        ]
      });

      colors.push(blue[500]);
    }

    if (rules.relegationPlaces > 0) {
      data.push({
        id: "relegation",
        data: [
          { x: getMinDate(), y: rules.totalPlaces - rules.relegationPlaces + 1 },
          { x: getMaxDate(), y: rules.totalPlaces - rules.relegationPlaces + 1 }
        ]
      });

      colors.push(red[500]);
    }

    return { data, colors };
  };

  const gridYValues = numRows === 20 ? [1, 5, 10, 15, 20] : [1, 6, 12, 18, 24];
  const gridXValues = positions.map((p) => new Date(p.date)).filter((d) => d.getUTCDate() === 1);

  return { ...getData(), gridYValues, gridXValues, theme: getTheme() };
};

export { useDrillDownPositionGraph };
