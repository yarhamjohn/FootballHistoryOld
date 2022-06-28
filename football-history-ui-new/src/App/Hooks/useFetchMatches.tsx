import { useQuery } from "react-query";
import { fetchData, getMatchesUrl } from "../Domain/Api";
import { Match } from "../Domain/Types";

const useFetchMatches = (competitionId: number, teamId?: number) => {
  return useQuery<Match[], Error>(["matches", { competitionId, teamId }], () =>
    fetchData(getMatchesUrl(competitionId, teamId))
  );
};

export { useFetchMatches };
