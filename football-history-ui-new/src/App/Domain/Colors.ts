import { yellow, green, red, blue, amber } from "@mui/material/colors";

const getLeagueStatusColor = (status: string | null) => {
  switch (status) {
    case "Champions":
      return amber[500];
    case "Champions & Promoted - Test Matches":
      return amber[500];
    case "Champions & Test Matches":
      return amber[500];
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
};

export { getLeagueStatusColor };
