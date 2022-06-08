import { useQuery } from "react-query";
import { fetchData, getHistoricalPositionsUrl } from "../Domain/Api";
import { HistoricalRecord } from "../Domain/Types";

const useFetchHistoricalRecord = (teamId: number, seasonIds: number[]) =>
  useQuery<HistoricalRecord, Error>(["historical-positions", { teamId, seasonIds }], () =>
    fetchData(getHistoricalPositionsUrl(teamId, seasonIds))
  );

export { useFetchHistoricalRecord };
