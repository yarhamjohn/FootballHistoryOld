import { useApi } from "./useApi";
import { useFetch } from "./useFetch";

export type Position = {
  id: number;
  competitionId: number;
  competitionName: string;
  teamId: number;
  teamName: string;
  position: number;
  status: string | null;
};

const useFetchPositions = (seasonId: number | undefined) => {
  const api = useApi();

  const url = `${api}/api/v2/positions/season/${seasonId}`;

  const result = useFetch(url);
  return result.status === "LOAD_SUCCESSFUL"
    ? { ...result, data: result.data as Position[] }
    : result;
};

export { useFetchPositions };
