import Alert from "@mui/material/Alert/Alert";
import CircularProgress from "@mui/material/CircularProgress/CircularProgress";
import { FC, ReactElement } from "react";
import { useHistoricalRecord } from "../../Hooks/useHistoricalRecord";
import { HistoricalRecordGraph } from "./HistoricalRecordGraph/HistoricalRecordGraph";
import { HistoricalRecordSlider } from "./HistoricalRecordSlider/HistoricalRecordSlider";

type HistoricalPositionProps = { teamId: number };

const HistoricalRecord: FC<HistoricalPositionProps> = ({ teamId }): ReactElement => {
  const { historicalRecord, selectedRange, updateSelectedRange } = useHistoricalRecord(teamId);

  if (historicalRecord.isError) {
    return <Alert severity="error">{historicalRecord.error.message}</Alert>;
  }

  if (historicalRecord.isSuccess) {
    return (
      <>
        <HistoricalRecordSlider
          selectedRange={selectedRange}
          updateSelectedRange={updateSelectedRange}
        />
        <HistoricalRecordGraph
          historicalSeasons={historicalRecord.data.historicalSeasons}
          selectedRange={selectedRange}
        />
      </>
    );
  }

  return <CircularProgress />;
};

export { HistoricalRecord as HistoricalRecord };
