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
  historicalRecordRange: number[];
  updateHistoricalRecord: (newRange: number[]) => void;
} => {
  const { seasons } = useContext(SeasonsContext);

  const [selectedSeasons, setSelectedSeasons] = useState<Season[]>(seasons);
  const { state, callApi } = useFetch<HistoricalRecord>();

  useEffect(() => {
    const url = getHistoricalPositionsUrl(
      teamId,
      seasons.map((x) => x.id)
    );

    callApi(url);
  }, [teamId, seasons]);

  const historicalRecordRange = [
    Math.min(...selectedSeasons.map((x) => x.startYear)),
    Math.max(...selectedSeasons.map((x) => x.startYear))
  ];

  const updateHistoricalRecord = (newRange: number[]) => {
    setSelectedSeasons(
      seasons.filter(
        (s) => s.startYear >= Math.min(...newRange) && s.startYear <= Math.max(...newRange)
      )
    );
  };

  return {
    historicalRecordState: state,
    historicalRecordRange,
    updateHistoricalRecord
  };
};

export { useFetchHistoricalRecord };
