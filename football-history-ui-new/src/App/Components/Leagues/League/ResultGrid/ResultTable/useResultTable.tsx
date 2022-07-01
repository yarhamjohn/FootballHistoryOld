import { Match } from "../../../../../Domain/Types";

const useResultTable = (matches: Match[]) => {
  const abbreviations = Array.from(new Set(matches.map((m) => m.homeTeam.abbreviation))).sort();

  return { abbreviations };
};

export { useResultTable };
