import Alert from "@mui/material/Alert/Alert";
import CircularProgress from "@mui/material/CircularProgress/CircularProgress";
import { FC, ReactElement } from "react";
import { useFetchHistoricalPositions } from "../../Hooks/useFetchHistoricalPositions";

const HistoricalPositions: FC = (): ReactElement => {
  const { historicalPositionsState } = useFetchHistoricalPositions();

  if (historicalPositionsState.status === "FETCH_NOT_STARTED") {
    return <></>;
  }

  if (historicalPositionsState.status === "FETCH_IN_PROGRESS") {
    return <CircularProgress />;
  }

  if (historicalPositionsState.status === "FETCH_ERROR") {
    return <Alert severity="error">{historicalPositionsState.error.message}</Alert>;
  }

  return <></>;
};

export { HistoricalPositions };
