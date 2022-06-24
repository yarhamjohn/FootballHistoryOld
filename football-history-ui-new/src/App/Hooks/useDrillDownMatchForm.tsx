import { Match } from "../Domain/Types";

const useDrillDownMatchForm = (matches: Match[], teamId: number) => {
  const form = matches
    .filter((m) => m.rules.type === "League")
    .sort((a, b) => new Date(a.matchDate).valueOf() - new Date(b.matchDate).valueOf()) // Although they look like Date types, they are in fact strings and need recreating as Dates (https://stackoverflow.com/questions/2627650/why-javascript-gettime-is-not-a-function)
    .map((m) =>
      m.homeTeam.id === teamId && m.homeTeam.goals > m.awayTeam.goals
        ? "W"
        : m.awayTeam.id === teamId && m.awayTeam.goals > m.homeTeam.goals
        ? "W"
        : m.homeTeam.goals === m.awayTeam.goals
        ? "D"
        : "L"
    );

  return { form };
};

export { useDrillDownMatchForm };
