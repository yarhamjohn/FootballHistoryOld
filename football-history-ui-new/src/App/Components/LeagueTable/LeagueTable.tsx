import {
  Alert,
  CircularProgress,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow
} from "@mui/material";
import { FC, ReactElement } from "react";
import { Competition, Size } from "../../Domain/Types";
import { useFetchLeague } from "../../Hooks/useFetchLeague";
import { LeagueTableRow } from "./LeagueTableRow";

type Props = { competition: Competition; size: Size };

const LeagueTable: FC<Props> = ({ competition, size }): ReactElement => {
  const league = useFetchLeague(competition.id);

  if (league.isError) {
    return <Alert severity="error">{league.error.message}</Alert>;
  }

  if (league.isSuccess) {
    return (
      <TableContainer component={Paper}>
        <Table size="small">
          <TableHead>
            <TableRow>
              <TableCell />
              <TableCell>Pos</TableCell>
              <TableCell>Team</TableCell>
              <TableCell>P</TableCell>
              <TableCell>W</TableCell>
              <TableCell>D</TableCell>
              <TableCell>L</TableCell>
              {size === "large" && <TableCell>GF</TableCell>}
              {size === "large" && <TableCell>GF</TableCell>}
              <TableCell>Diff</TableCell>
              {size === "large" && <TableCell>GAv</TableCell>}
              {size === "large" && <TableCell>PPG</TableCell>}
              <TableCell>Points</TableCell>
              <TableCell />
            </TableRow>
          </TableHead>
          <TableBody>
            {league.data.table
              .sort((a, b) => a.position - b.position)
              .map((row) => (
                <LeagueTableRow
                  key={row.teamId}
                  row={row}
                  size={size}
                  competition={league.data.competition}
                />
              ))}
          </TableBody>
        </Table>
      </TableContainer>
    );
  }

  return <CircularProgress />;
};

export { LeagueTable };
