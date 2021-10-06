import { Reducer, useEffect, useReducer, useState } from "react";
import { useApi } from "./useApi";
import { SeasonDateRange as SeasonDateRange } from "../components/HistoricalPositions";
import { Season } from "../seasonsSlice";
import { callApi } from "./useFetch";

export type HistoricalRecord = {
  teamId: number;
  historicalSeasons: HistoricalSeason[];
};

export type HistoricalSeason = {
  seasonId: number;
  seasonStartYear: number;
  boundaries: number[];
  historicalPosition: HistoricalPosition | null;
};

export type HistoricalPosition = {
  competitionId: number;
  competitionName: string;
  position: number;
  overallPosition: number;
  status: string | null;
};

export type HistoricalRecordState =
  | { type: "HISTORICAL_RECORD_UNLOADED" }
  | { type: "HISTORICAL_RECORD_LOADING"; record: HistoricalRecord }
  | {
      type: "HISTORICAL_RECORD_LOADED";
      record: HistoricalRecord;
    }
  | { type: "HISTORICAL_RECORD_LOAD_FAILED"; error: string };

export type HistoricalRecordAction =
  | { type: "LOAD_HISTORICAL_RECORD"; record: HistoricalRecord }
  | {
      type: "LOAD_HISTORICAL_RECORD_SUCCEEDED";
      record: HistoricalRecord;
    }
  | {
      type: "UPDATE_HISTORICAL_RECORD";
      record: HistoricalRecord;
    }
  | { type: "LOAD_HISTORICAL_RECORD_FAILED"; error: string };

const historicalRecordReducer = (
  state: HistoricalRecordState,
  action: HistoricalRecordAction
): HistoricalRecordState => {
  switch (action.type) {
    case "LOAD_HISTORICAL_RECORD":
      return { type: "HISTORICAL_RECORD_LOADING", record: action.record };
    case "UPDATE_HISTORICAL_RECORD":
    case "LOAD_HISTORICAL_RECORD_SUCCEEDED":
      return {
        type: "HISTORICAL_RECORD_LOADED",
        record: action.record,
      };
    case "LOAD_HISTORICAL_RECORD_FAILED":
      return { type: "HISTORICAL_RECORD_LOAD_FAILED", error: action.error };
    default:
      return { type: "HISTORICAL_RECORD_UNLOADED" };
  }
};

const useFetchHistoricalRecord = (teamId: number, allSeasons: Season[], range: SeasonDateRange) => {
  const api = useApi();
  const [allFetchedSeasons, setAllFetchedSeasons] = useState<HistoricalSeason[]>([]);
  const [url, setUrl] = useState<string>("");
  const [state, dispatch] = useReducer<Reducer<HistoricalRecordState, HistoricalRecordAction>>(
    historicalRecordReducer,
    {
      type: "HISTORICAL_RECORD_UNLOADED",
    }
  );

  const getSeasonsInRange = (seasons: HistoricalSeason[]) =>
    seasons.filter(
      (x) => x.seasonStartYear >= range.startYear && x.seasonStartYear <= range.endYear
    );

  const getInclusiveYears = (start: number, end: number) =>
    Array.from({ length: end - start + 1 }, (v, k) => k + start);

  const getUrl = (yearsToFetch: number[]) => {
    const seasonIds = allSeasons
      .filter((x) => yearsToFetch.includes(x.startYear))
      .map((y) => `&seasonIds=${y.id}`)
      .join("");

    return `${api}/api/v2/historical-record?teamId=${teamId}${seasonIds}`;
  };

  useEffect(() => {
    const allYearsInRange = getInclusiveYears(range.startYear, range.endYear);
    setAllFetchedSeasons([]);
    const newUrl = getUrl(allYearsInRange);

    setUrl(newUrl);
  }, [teamId]);

  useEffect(() => {
    const allYearsInRange = getInclusiveYears(range.startYear, range.endYear);
    const seasonsAlreadyFetched = allFetchedSeasons.filter((x) =>
      allYearsInRange.includes(x.seasonStartYear)
    );

    const yearsNotYetFetched = allYearsInRange.filter(
      (x) => !seasonsAlreadyFetched.map((y) => y.seasonStartYear).includes(x)
    );

    if (yearsNotYetFetched.length === 0) {
      dispatch({
        type: "UPDATE_HISTORICAL_RECORD",
        record: { teamId, historicalSeasons: getSeasonsInRange(seasonsAlreadyFetched) },
      });
    }

    const newUrl = getUrl(yearsNotYetFetched);

    setUrl(newUrl);
  }, [range]);

  useEffect(() => {
    const abortController = new AbortController();

    dispatch({
      type: "LOAD_HISTORICAL_RECORD",
      record: { teamId, historicalSeasons: getSeasonsInRange(allFetchedSeasons) },
    });

    callApi<HistoricalRecord>(url, abortController.signal)
      .then((data: HistoricalRecord) => {
        const allSeasons = [...allFetchedSeasons, ...data.historicalSeasons];
        setAllFetchedSeasons(allSeasons);
        dispatch({
          type: "LOAD_HISTORICAL_RECORD_SUCCEEDED",
          record: { teamId, historicalSeasons: getSeasonsInRange(allSeasons) },
        });
      })
      .catch((error: Error) => {
        if (!abortController.signal.aborted) {
          dispatch({ type: "LOAD_HISTORICAL_RECORD_FAILED", error: error.message });
        }
      });

    return () => {
      abortController.abort();
    };
  }, [url]);

  return { state };
};

export { useFetchHistoricalRecord };
