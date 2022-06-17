import { useQuery } from "react-query";
import { fetchData, getLeaguePositionsUrl } from "../Domain/Api";
import { LeaguePosition } from "../Domain/Types";

const useFetchLeaguePositions = (competitionId: number, teamId: number) => {
  return useQuery<LeaguePosition[], Error>(["league-positions", { competitionId, teamId }], () =>
    fetchData(getLeaguePositionsUrl(competitionId, teamId))
  );
};

export { useFetchLeaguePositions };
