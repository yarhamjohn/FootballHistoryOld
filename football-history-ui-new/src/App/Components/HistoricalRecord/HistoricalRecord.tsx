import Alert from "@mui/material/Alert/Alert";
import CircularProgress from "@mui/material/CircularProgress/CircularProgress";
import { FC, ReactElement } from "react";
import { Team } from "../../Domain/Types";
import { useFetchHistoricalRecord } from "../../Hooks/useFetchHistoricalRecord";
import { HistoricalRecordGraph } from "./HistoricalRecordGraph/HistoricalRecordGraph";
import { HistoricalRecordSlider } from "./HistoricalRecordSlider/HistoricalRecordSlider";

type HistoricalPositionProps = { activeTeam: Team };

const HistoricalRecord: FC<HistoricalPositionProps> = ({ activeTeam }): ReactElement => {
  const { historicalRecordState, selectedRange, updateSelectedRange } = useFetchHistoricalRecord(
    activeTeam.id
  );

  if (historicalRecordState.status === "FETCH_NOT_STARTED") {
    return <></>;
  }

  if (historicalRecordState.status === "FETCH_IN_PROGRESS") {
    return <CircularProgress />;
  }

  if (historicalRecordState.status === "FETCH_ERROR") {
    return <Alert severity="error">{historicalRecordState.error.message}</Alert>;
  }

  return (
    <>
      <HistoricalRecordSlider
        selectedRange={selectedRange}
        updateSelectedRange={updateSelectedRange}
      />
      <HistoricalRecordGraph
        historicalSeasons={historicalRecordState.data.historicalSeasons}
        selectedRange={selectedRange}
      />
    </>
  );
};

export { HistoricalRecord as HistoricalRecord };
