import { useApi } from "./useApi";
import { useFetch } from "./useFetch";

export type Statistic = {
  category: "Points" | "Goals" | "Results";
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
    ? { ...result, data: result.data as Statistic[] }
    : result;
};

export { useFetchStatistics };
