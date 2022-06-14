import { FC } from "react";
import { Point } from "@nivo/line";
import { HistoricalSeason } from "../../../Domain/Types";
import { getLeagueStatusColor } from "../../../Domain/Colors";

type Props = { point: Point; season: HistoricalSeason };

const TooltipContent: FC<Props> = ({ point, season }) => {
  if (!point.id.startsWith("positions") || season.historicalPosition === null) {
    return null;
  }

  const color = getLeagueStatusColor(season.historicalPosition?.status);

  return (
    <div
      key={point.id}
      style={{
        color: "black",
        padding: "1rem",
        display: "flex",
        flexDirection: "column",
        boxShadow: `0 0 1rem ${color} inset`
      }}
    >
      {color === null ? null : (
        <h3 style={{ color: color, margin: 0 }}>{season.historicalPosition.status}</h3>
      )}
      <strong>{season.historicalPosition.competitionName}</strong>
      <span>
        <strong>Position</strong>: {season.historicalPosition.position}
      </span>
      <span>
        <strong>Season</strong>: {season.seasonStartYear}-{season.seasonStartYear + 1}
      </span>
    </div>
  );
};

export { TooltipContent };
