import { useApi } from "./useApi";
import { useFetch } from "./useFetch";
import { Match } from "./useFetchLeagueMatches";

const useFetchPlayOffMatches = (competitionId: number) => {
  const api = useApi();

  const url = `${api}/api/v2/matches?competitionId=${competitionId}&type=PlayOff`;

  return useFetch<Match[]>(url);
};

export { useFetchPlayOffMatches };
