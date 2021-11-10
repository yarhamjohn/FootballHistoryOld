import { FunctionComponent, useState } from "react";
import { useFetchHistoricalRecord } from "../../shared/useFetchHistoricalRecord";
import { YearSlider } from "../Filters/YearSlider";
import { ErrorMessage } from "../ErrorMessage";
import { HistoricalPositionsGraph } from "./Graph";
import { useGetAllSeasonsQuery } from "../../seasonsSlice";

export type SeasonDateRange = {
  startYear: number;
  endYear: number;
};

const HistoricalPositions: FunctionComponent<{ teamId: number }> = ({ teamId }) => {
  const seasonState = useGetAllSeasonsQuery();

  //TODO: must be a better way
  const getFirstSeasonStartYear =
    seasonState.data === undefined ? 0 : Math.min(...seasonState.data.map((s) => s.startYear));
  const getLastSeasonStartYear =
    seasonState.data === undefined ? 0 : Math.max(...seasonState.data.map((s) => s.startYear));

  const [selectedRange, setSelectedRange] = useState<SeasonDateRange>({
    startYear: getFirstSeasonStartYear,
    endYear: getLastSeasonStartYear,
  });

  //TODO: equally this is crazy
  const { state } = useFetchHistoricalRecord(teamId, seasonState.data!, selectedRange);

  return (
    <div style={{ marginBottom: "5rem" }}>
      <YearSlider
        sliderRange={[getFirstSeasonStartYear, getLastSeasonStartYear]}
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
