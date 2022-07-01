import { useQuery } from "react-query";
import { fetchData, getMatchesUrl } from "../Domain/Api";
import { Match } from "../Domain/Types";

const useFetchMatches = (type: "League" | "PlayOff", competitionId: number, teamId?: number) => {
  return useQuery<Match[], Error>(["matches", { competitionId, teamId, type }], () =>
    fetchData(getMatchesUrl(type, competitionId, teamId))
  );
};

export { useFetchMatches };
