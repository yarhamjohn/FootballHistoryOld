import CircularProgress from "@mui/material/CircularProgress";
import Alert from "@mui/material/Alert";
import { FC, ReactElement } from "react";
import { useFetchLeaguePositions } from "../../../Hooks/useFetchLeaguePositions";
import { DrillDownPositionGraph } from "./DrillDownPositionGraph";
import { Competition } from "../../../Domain/Types";

type Props = { competition: Competition; teamId: number };

const DrillDownPosition: FC<Props> = ({ competition, teamId }): ReactElement => {
  const positions = useFetchLeaguePositions(competition.id, teamId);

  if (positions.isError) {
    return <Alert severity="error">{positions.error.message}</Alert>;
  }

  if (positions.isSuccess) {
    return <DrillDownPositionGraph positions={positions.data} rules={competition.rules} />;
  }

  return <CircularProgress />;
};

export { DrillDownPosition };
