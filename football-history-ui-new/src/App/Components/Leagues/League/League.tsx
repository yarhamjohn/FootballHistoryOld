import { Divider } from "@mui/material";
import Alert from "@mui/material/Alert/Alert";
import Box from "@mui/material/Box/Box";
import CircularProgress from "@mui/material/CircularProgress/CircularProgress";
import { FC, ReactElement } from "react";
import { Competition, Size } from "../../../Domain/Types";
import { useFetchLeague } from "../../../Hooks/useFetchLeague";
import { LeagueTable } from "./LeagueTable/LeagueTable";
import { PointDeduction } from "./PointDeduction/PointDeduction";
import { ResultGrid } from "./ResultGrid/ResultGrid";

type Props = { competition: Competition; size: Size };

const League: FC<Props> = ({ competition, size }): ReactElement => {
  const league = useFetchLeague(competition.id);

  if (league.isError) {
    return <Alert severity="error">{league.error.message}</Alert>;
  }

  if (league.isSuccess) {
    return (
      <>
        <LeagueTable league={league.data} size={size} />
        <Box sx={{ marginTop: "1rem", width: "100%" }}>
          <PointDeduction leagueTableRows={league.data.table} />
        </Box>
        <Divider style={{ marginTop: "2rem", marginBottom: "2rem", width: "100%" }} />
        <Box sx={{ marginTop: "1rem", width: "100%" }}>
          <ResultGrid competitionId={competition.id} />
        </Box>
      </>
    );
  }

  return <CircularProgress />;
};

export { League };
