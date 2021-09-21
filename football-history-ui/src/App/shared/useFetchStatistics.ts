import { useApi } from "./useApi";
import { useFetch } from "./useFetch";

export type Statistics = {
  category: "Points" | "Goals" | "Results";
  statistics: Statistic[];
};

export type Statistic = {
  name: string;
  value: number;
  teamName: string;
  competitionName: string;
};

const useFetchStatistics = (seasonId: number) => {
  const api = useApi();

  const url = `${api}/api/v2/statistics/season/${seasonId}`;

  const result = useFetch(url);
  return result.status === "LOAD_SUCCESSFUL"
    ? { ...result, data: result.data as Statistics[] }
    : result;
};

export { useFetchStatistics };
