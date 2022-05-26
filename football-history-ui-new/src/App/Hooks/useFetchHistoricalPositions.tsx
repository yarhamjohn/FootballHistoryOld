import { useEffect } from "react";
import { getHistoricalPositionsUrl } from "../Domain/Api";
import { HistoricalPosition } from "../Domain/Types";
import { useFetch } from "./useFetch";

const useFetchHistoricalPositions = () => {
  const { state, callApi } = useFetch<HistoricalPosition[]>();

  useEffect(() => {
    callApi(getHistoricalPositionsUrl());
  }, []);

  return { historicalPositionsState: state };
};

export { useFetchHistoricalPositions };
