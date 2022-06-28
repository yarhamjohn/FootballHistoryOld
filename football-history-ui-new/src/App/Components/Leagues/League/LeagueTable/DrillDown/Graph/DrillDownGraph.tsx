import { Alert, CircularProgress } from "@mui/material";
import { FC, ReactElement } from "react";
import { Competition } from "../../../../../../Domain/Types";
import { useFetchLeaguePositions } from "../../../../../../Hooks/useFetchLeaguePositions";
import { DrillDownPositionGraph } from "./DrillDownPositionGraph";

type Props = { competition: Competition; teamId: number };

const DrillDownGraph: FC<Props> = ({ competition, teamId }): ReactElement => {
  const positions = useFetchLeaguePositions(competition.id, teamId);

  return positions.isError ? (
    <Alert severity="error">{positions.error.message}</Alert>
  ) : positions.isSuccess ? (
    <DrillDownPositionGraph positions={positions.data} rules={competition.rules} />
  ) : (
    <CircularProgress />
  );
};

export { DrillDownGraph };
