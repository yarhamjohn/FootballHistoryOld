import { useApi } from "./useApi";
import { useFetch } from "./useFetch";

export type LeaguePosition = {
  date: Date;
  position: number;
};

const useFetchLeaguePositions = (competitionId: number, teamId: number) => {
  const api = useApi();

  const url = `${api}/api/v2/league-positions/competition/${competitionId}/team/${teamId}`;

  return useFetch<LeaguePosition[]>(url);
};

export { useFetchLeaguePositions };
