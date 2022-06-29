import { Match } from "../../../../../Domain/Types";

const useResultTable = (matches: Match[]) => {
  const leagueMatches = matches.filter((m) => m.rules.type === "League");

  const abbreviations = Array.from(
    new Set(leagueMatches.map((m) => m.homeTeam.abbreviation))
  ).sort();

  return { leagueMatches, abbreviations };
};

export { useResultTable };
