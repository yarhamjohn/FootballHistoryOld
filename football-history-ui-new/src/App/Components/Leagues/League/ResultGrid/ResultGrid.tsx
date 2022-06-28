import { CircularProgress } from "@mui/material";
import Alert from "@mui/material/Alert/Alert";
import { FC, ReactElement } from "react";
import { useFetchMatches } from "../../../../Hooks/useFetchMatches";
import { ResultTable } from "./ResultTable/ResultTable";

type Props = { competitionId: number };

const ResultGrid: FC<Props> = ({ competitionId }): ReactElement => {
  const matches = useFetchMatches(competitionId);

  if (matches.isError) {
    return <Alert severity="error">{matches.error.message}</Alert>;
  }

  if (matches.isSuccess) {
    return <ResultTable matches={matches.data} />;
  }

  return <CircularProgress />;
};

export { ResultGrid };
