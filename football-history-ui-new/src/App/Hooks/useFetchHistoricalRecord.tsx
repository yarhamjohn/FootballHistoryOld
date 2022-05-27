import { useContext, useEffect, useState } from "react";
import { SeasonsContext } from "../Contexts/SeasonsContext";
import { getHistoricalPositionsUrl } from "../Domain/Api";
import { HistoricalRecord, Season } from "../Domain/Types";
import { FetchState } from "./fetchReducer";
import { useFetch } from "./useFetch";

const useFetchHistoricalRecord = (
  teamId: number
): {
  historicalRecordState: FetchState<HistoricalRecord>;
  selectedRange: number[];
  updateSelectedRange: (newRange: number[]) => void;
} => {
  const { state, callApi } = useFetch<HistoricalRecord>();
  const { seasons, seasonIds } = useContext(SeasonsContext);
  const [selectedSeasons, setSelectedSeasons] = useState<Season[]>(seasons);

  useEffect(() => {
    const url = getHistoricalPositionsUrl(teamId, seasonIds);

    callApi(url);
  }, [teamId, seasonIds]);

  const selectedRange = [
    Math.min(...selectedSeasons.map((x) => x.startYear)),
    Math.max(...selectedSeasons.map((x) => x.startYear))
  ];

  const updateSelectedRange = (newRange: number[]) => {
    setSelectedSeasons(
      seasons.filter(
        (s) => s.startYear >= Math.min(...newRange) && s.startYear <= Math.max(...newRange)
      )
    );
  };

  return {
    historicalRecordState: state,
    selectedRange,
    updateSelectedRange
  };
};

export { useFetchHistoricalRecord };
