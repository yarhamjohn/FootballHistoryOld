import { FC, ReactElement, useContext } from "react";
import { Point } from "@nivo/line";
import { ColorModeContext } from "../../../Contexts/ColorModeContext";

type Props = { points: Point[] };

const DrillDownTooltip: FC<Props> = ({ points }): ReactElement => {
  const { mode } = useContext(ColorModeContext);

  return (
    <div
      style={{
        background: mode === "dark" ? "white" : "black",
        color: mode === "dark" ? "black" : "white",
        padding: "1rem",
        borderRadius: "5px",
        display: "flex",
        flexDirection: "column"
      }}
    >
      <span>Date: {points.map((p) => new Date(p.data.xFormatted).toDateString())[0]}</span>
      <span>Position: {points.map((p) => p.data.yFormatted)[0]}</span>
    </div>
  );
};

export { DrillDownTooltip };
