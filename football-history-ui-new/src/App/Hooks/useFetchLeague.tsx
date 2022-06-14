import { useQuery } from "react-query";
import { fetchData, getLeagueTableUrl } from "../Domain/Api";
import { League } from "../Domain/Types";

const useFetchLeague = (competitionId: number) =>
  useQuery<League, Error>(["league-table", { competitionId }], () =>
    fetchData(getLeagueTableUrl(competitionId))
  );

export { useFetchLeague };
