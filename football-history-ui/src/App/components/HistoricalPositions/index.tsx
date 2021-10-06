import { FunctionComponent, useState } from "react";
import { useFetchHistoricalRecord } from "../../shared/useFetchHistoricalRecord";
import { YearSlider } from "../Filters/YearSlider";
import { ErrorMessage } from "../ErrorMessage";
import { HistoricalPositionsGraph } from "./Graph";
import { useAppSelector } from "../../../reduxHooks";

export type SeasonDateRange = {
  startYear: number;
  endYear: number;
};

const HistoricalPositions: FunctionComponent<{ teamId: number }> = ({ teamId }) => {
  const seasonState = useAppSelector((state) => state.season);

  const getFirstSeasonStartYear = () => Math.min(...seasonState.seasons.map((s) => s.startYear));
  const getLastSeasonStartYear = () => Math.max(...seasonState.seasons.map((s) => s.startYear));

  const [selectedRange, setSelectedRange] = useState<SeasonDateRange>({
    startYear: getLastSeasonStartYear() - 25, // To speed initial load, only the last 25 seasons are fetched to begin with);
    endYear: getLastSeasonStartYear(),
  });

  const { state } = useFetchHistoricalRecord(teamId, seasonState.seasons, selectedRange);

  return (
    <div style={{ marginBottom: "5rem" }}>
      <YearSlider
        sliderRange={[getFirstSeasonStartYear(), getLastSeasonStartYear()]}
        selectedRange={selectedRange}
        setSelectedRange={setSelectedRange}
      />
      {state.type === "HISTORICAL_RECORD_UNLOADED" ? null : state.type ===
        "HISTORICAL_RECORD_LOAD_FAILED" ? (
        <ErrorMessage header={"Sorry, something went wrong."} content={state.error} />
      ) : (
        <HistoricalPositionsGraph
          isLoading={state.type === "HISTORICAL_RECORD_LOADING"}
          seasons={state.record.historicalSeasons}
          range={selectedRange}
        />
      )}
    </div>
  );
};

export { HistoricalPositions };
