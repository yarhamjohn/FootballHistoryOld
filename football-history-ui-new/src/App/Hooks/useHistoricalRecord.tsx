import { useContext, useState } from "react";
import { SeasonsContext } from "../Contexts/SeasonsContext";
import { Season } from "../Domain/Types";
import { useFetchHistoricalRecord } from "./useFetchHistoricalRecord";

const useHistoricalRecord = (teamId: number) => {
  const { seasons, seasonIds } = useContext(SeasonsContext);
  const [selectedSeasons, setSelectedSeasons] = useState<Season[]>(seasons);

  const historicalRecord = useFetchHistoricalRecord(teamId, seasonIds);

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

  return { historicalRecord, selectedRange, updateSelectedRange };
};

export { useHistoricalRecord };
