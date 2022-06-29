import { CircularProgress } from "@mui/material";
import Alert from "@mui/material/Alert/Alert";
import { FC, ReactElement } from "react";
import { Size } from "../../../../Domain/Types";
import { useFetchMatches } from "../../../../Hooks/useFetchMatches";
import { ResultTable } from "./ResultTable/ResultTable";

type Props = { competitionId: number; size: Size };

const ResultGrid: FC<Props> = ({ competitionId, size }): ReactElement => {
  const matches = useFetchMatches(competitionId);

  if (matches.isError) {
    return <Alert severity="error">{matches.error.message}</Alert>;
  }

  if (matches.isSuccess) {
    return <ResultTable matches={matches.data} size={size} />;
  }

  return <CircularProgress />;
};

export { ResultGrid };
