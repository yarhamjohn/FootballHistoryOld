import { Alert, CircularProgress } from "@mui/material";
import { FC, ReactElement } from "react";
import { useFetchMatches } from "../../../../Hooks/useFetchMatches";
import { DrillDownMatchForm } from "./DrillDownMatchForm";

type Props = { competitionId: number; teamId: number };

const DrillDownForm: FC<Props> = ({ competitionId, teamId }): ReactElement => {
  const matches = useFetchMatches(competitionId, teamId);

  return matches.isError ? (
    <Alert severity="error">{matches.error.message}</Alert>
  ) : matches.isSuccess ? (
    <DrillDownMatchForm matches={matches.data} teamId={teamId} />
  ) : (
    <CircularProgress />
  );
};

export { DrillDownForm };
