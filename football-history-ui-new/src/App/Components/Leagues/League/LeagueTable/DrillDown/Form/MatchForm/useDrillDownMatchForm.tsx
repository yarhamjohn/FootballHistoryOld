import { green, red, blue } from "@mui/material/colors";
import { Form, Match, Outcome } from "../../../../../../../Domain/Types";

const useDrillDownMatchForm = (matches: Match[], teamId: number): { form: Form } => {
  const getMatchResult = (match: Match) => {
    const isHomeTeam = match.homeTeam.id === teamId;

    const outcome: Outcome =
      isHomeTeam && match.homeTeam.goals > match.awayTeam.goals
        ? "W"
        : !isHomeTeam && match.awayTeam.goals > match.homeTeam.goals
        ? "W"
        : match.homeTeam.goals === match.awayTeam.goals
        ? "D"
        : "L";

    const opponent = isHomeTeam ? match.awayTeam.name : match.homeTeam.name;

    const color = outcome === "W" ? green[500] : outcome === "L" ? red[500] : blue[500];

    return {
      outcome,
      color,
      title: `${new Date(match.matchDate).toDateString()} - ${opponent}`
    };
  };

  const form = matches
    // Although they look like Date types, they are in fact strings and need recreating as Dates.
    // See https://stackoverflow.com/questions/2627650/why-javascript-gettime-is-not-a-function.
    .sort((a, b) => new Date(a.matchDate).valueOf() - new Date(b.matchDate).valueOf())
    .map((m) => getMatchResult(m));

  return { form };
};

export { useDrillDownMatchForm };
