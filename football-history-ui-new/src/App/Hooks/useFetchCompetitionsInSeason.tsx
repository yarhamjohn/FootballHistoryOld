import { useQuery } from "react-query";
import { fetchData, getCompetitionsUrl } from "../Domain/Api";
import { Competition } from "../Domain/Types";

const useFetchCompetitionsInSeason = (seasonId: number) =>
  useQuery<Competition[], Error>(["competitionsInSeason", { seasonId }], () =>
    fetchData(getCompetitionsUrl(seasonId))
  );

export { useFetchCompetitionsInSeason };
