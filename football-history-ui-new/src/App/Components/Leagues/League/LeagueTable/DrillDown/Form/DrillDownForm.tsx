import { Alert, CircularProgress } from "@mui/material";
import { FC, ReactElement } from "react";
import { useFetchMatches } from "../../../../../../Hooks/useFetchMatches";
import { DrillDownMatchForm } from "./MatchForm/DrillDownMatchForm";

type Props = { competitionId: number; teamId: number };

const DrillDownForm: FC<Props> = ({ competitionId, teamId }): ReactElement => {
  const leagueMatches = useFetchMatches("League", competitionId, teamId);

  return leagueMatches.isError ? (
    <Alert severity="error">{leagueMatches.error.message}</Alert>
  ) : leagueMatches.isSuccess ? (
    <DrillDownMatchForm matches={leagueMatches.data} teamId={teamId} />
  ) : (
    <CircularProgress />
  );
};

export { DrillDownForm };
