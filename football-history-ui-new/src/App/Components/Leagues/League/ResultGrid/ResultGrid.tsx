import { CircularProgress } from "@mui/material";
import Alert from "@mui/material/Alert/Alert";
import { FC, ReactElement } from "react";
import { Size } from "../../../../Domain/Types";
import { useFetchMatches } from "../../../../Hooks/useFetchMatches";
import { ResultTable } from "./ResultTable/ResultTable";

type Props = { competitionId: number; size: Size };

const ResultGrid: FC<Props> = ({ competitionId, size }): ReactElement => {
  const leagueMatches = useFetchMatches("League", competitionId);

  if (leagueMatches.isError) {
    return <Alert severity="error">{leagueMatches.error.message}</Alert>;
  }

  if (leagueMatches.isSuccess) {
    return <ResultTable leagueMatches={leagueMatches.data} size={size} />;
  }

  return <CircularProgress />;
};

export { ResultGrid };
