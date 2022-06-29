import { FC, ReactElement, useContext } from "react";
import { Point } from "@nivo/line";
import { ColorModeContext } from "../../../../../../../../Contexts/ColorModeContext";
import { useDrillDownTooltip } from "./useDrillDownTooltip";

type Props = { points: Point[] };

const DrillDownTooltip: FC<Props> = ({ points }): ReactElement => {
  const { mode } = useContext(ColorModeContext);
  const { date, position } = useDrillDownTooltip(points);

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
      <span>{date}</span>
      <span>{position}</span>
    </div>
  );
};

export { DrillDownTooltip };
