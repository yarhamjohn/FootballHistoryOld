import { FunctionComponent } from "react";
import { Point } from "@nivo/line";
import { TooltipContent } from "./TooltipContent";
import { HistoricalSeason } from "../../../Domain/Types";

const Tooltip: FunctionComponent<{
  points: Point[];
  seasons: HistoricalSeason[];
}> = ({ points, seasons }) => {
  const getSeason = (point: Point) => {
    const startDate = Number(point.data.xFormatted);
    return seasons.filter((s) => s.seasonStartYear === startDate)[0];
  };

  return (
    <div
      style={{
        background: "white",
        border: "1px solid #ccc",
        borderRadius: "5px"
      }}
    >
      {points.map((point) =>
        point.data.y === null ? null : (
          <TooltipContent key={point.id} point={point} season={getSeason(point)} />
        )
      )}
    </div>
  );
};

export { Tooltip };
