import { useContext } from "react";
import { Row } from "../../../../../Domain/Types";
import { getLeagueStatusColor } from "../../../../../Domain/Colors";
import { blue, red } from "@mui/material/colors";
import { ColorModeContext } from "../../../../../Contexts/ColorModeContext";

const useLeagueTableRow = (
  row: Row
): {
  rowColor: string | null;
  fontColor: string;
  goalAverage: number;
  pointsPerGame: number;
  points: string;
} => {
  const { mode } = useContext(ColorModeContext);

  const rowColor = getLeagueStatusColor(row.status);

  const fontColor =
    rowColor === red[500] || rowColor === blue[500] || (rowColor === null && mode === "dark")
      ? "white"
      : "black";

  const goalAverage = +(Math.round(parseFloat(row.goalAverage + "e+2")) + "e-2");

  const pointsPerGame = +(Math.round(parseFloat(row.pointsPerGame + "e+2")) + "e-2");

  const points = `${row.points}${row.pointsDeducted > 0 ? " *" : ""}`;

  return { rowColor, fontColor, goalAverage, pointsPerGame, points };
};

export { useLeagueTableRow };
