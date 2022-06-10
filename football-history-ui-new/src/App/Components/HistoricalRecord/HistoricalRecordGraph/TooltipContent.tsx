import { FunctionComponent } from "react";
import { Point } from "@nivo/line";
import { HistoricalSeason } from "../../../Domain/Types";
import { blue, green, red, yellow } from "@mui/material/colors";

function getLeagueStatusColor(status: string | null) {
  switch (status) {
    case "Champions":
      return yellow[500];
    case "Champions & Promoted - Test Matches":
      return yellow[500];
    case "Champions & Test Matches":
      return yellow[500];
    case "Promoted":
      return green[500];
    case "Promoted - Test Matches":
      return green[500];
    case "Relegated":
      return red[500];
    case "PlayOffs":
      return blue[500];
    case "PlayOff Winner":
      return green[500];
    case "Relegation PlayOffs":
      return blue[500];
    case "Test Matches":
      return blue[500];
    case "Relegated - PlayOffs":
      return red[500];
    case "Relegated - Test Matches":
      return red[500];
    case "Failed Re-election":
      return red[500];
    case "Re-elected":
      return blue[500];
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
        color: "black",
        padding: "1rem",
        display: "flex",
        flexDirection: "column",
        boxShadow: `0 0 1rem ${color} inset`
      }}
    >
      {color === null ? null : (
        <h3 style={{ color: color.toString(), margin: 0 }}>{season.historicalPosition.status}</h3>
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

export { TooltipContent, getLeagueStatusColor };
