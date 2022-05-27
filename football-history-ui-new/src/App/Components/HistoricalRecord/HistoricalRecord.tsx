import Alert from "@mui/material/Alert/Alert";
import CircularProgress from "@mui/material/CircularProgress/CircularProgress";
import { FC, ReactElement } from "react";
import { Team } from "../../Domain/Types";
import { useFetchHistoricalRecord } from "../../Hooks/useFetchHistoricalRecord";
import { HistoricalRecordGraph } from "./HistoricalRecordGraph/HistoricalRecordGraph";
import { HistoricalRecordSlider } from "./HistoricalRecordSlider/HistoricalRecordSlider";

type HistoricalPositionProps = { activeTeam: Team };

const HistoricalRecord: FC<HistoricalPositionProps> = ({ activeTeam }): ReactElement => {
  const { historicalRecordState, historicalRecordRange, updateHistoricalRecord } =
    useFetchHistoricalRecord(activeTeam.id);

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
        historicalRecordRange={historicalRecordRange}
        updateHistoricalRecord={updateHistoricalRecord}
      />
      <HistoricalRecordGraph
        historicalSeasons={historicalRecordState.data.historicalSeasons}
        historicalRecordRange={historicalRecordRange}
      />
    </>
  );
};

export { HistoricalRecord as HistoricalRecord };
