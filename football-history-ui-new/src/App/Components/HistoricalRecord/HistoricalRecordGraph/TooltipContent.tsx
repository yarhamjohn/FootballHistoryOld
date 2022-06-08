import { FunctionComponent } from "react";
import { Point } from "@nivo/line";
import { HistoricalSeason } from "../../../Domain/Types";

enum Color {
  Green = "#75B266",
  Blue = "#7FBFBF",
  Red = "#B26694",
  Yellow = "#BFA67F",
  Grey = "#CCCCCC"
}

function getLeagueStatusColor(status: string | null) {
  switch (status) {
    case "Champions":
      return Color.Green;
    case "Promoted":
      return Color.Blue;
    case "Relegated":
      return Color.Red;
    case "PlayOffs":
      return Color.Yellow;
    case "PlayOff Winner":
      return Color.Blue;
    case "Relegation PlayOffs":
      return Color.Yellow;
    case "Relegated - PlayOffs":
      return Color.Red;
    case "Failed Re-election":
      return Color.Red;
    case "Re-elected":
      return Color.Yellow;
    default:
      return null;
  }
}

const TooltipContent: FunctionComponent<{ point: Point; season: HistoricalSeason }> = ({
  point,
  season
}) => {
  if (!point.id.startsWith("positions") || season.historicalPosition === null) {
    return null;
  }

  const color = getLeagueStatusColor(season.historicalPosition?.status);

  return (
    <div
      key={point.id}
      style={{
        color: point.serieColor,
        padding: "12px 12px",
        display: "flex",
        flexDirection: "column",
        boxShadow: `0px 0px 10px ${color} inset`
      }}
    >
      {color === null ? null : (
        <h3 style={{ color: color.toString() }}>{season.historicalPosition.status}</h3>
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
