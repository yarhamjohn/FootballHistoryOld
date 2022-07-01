import { Alert, CircularProgress } from "@mui/material";
import { FC, ReactElement } from "react";
import { useFetchMatches } from "../../../../Hooks/useFetchMatches";
import { PlayOff } from "./PlayOff";
import { TestMatch } from "./TestMatch";

type Props = { competitionId: number };

const PlayOffs: FC<Props> = ({ competitionId }): ReactElement => {
  const playOffMatches = useFetchMatches("PlayOff", competitionId);

  if (playOffMatches.isError) {
    return <Alert severity="error">{playOffMatches.error.message}</Alert>;
  }

  if (playOffMatches.isSuccess) {
    const testMatches =
      playOffMatches.data.filter((x) => new Date(x.matchDate) < new Date(1, 1, 1950)).length > 0;

    return testMatches ? (
      <TestMatch matches={playOffMatches.data} />
    ) : (
      <PlayOff matches={playOffMatches.data} />
    );
  }

  return <CircularProgress />;
};

export { PlayOffs };
