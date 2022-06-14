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
import { Competition } from "../../Domain/Types";
import { useFetchLeague } from "../../Hooks/useFetchLeague";
import { LeagueTableRow } from "./LeagueTableRow";

type Props = { competition: Competition };

const LeagueTable: FC<Props> = ({ competition }): ReactElement => {
  const leagueTable = useFetchLeague(competition.id);

  if (leagueTable.isError) {
    return <Alert severity="error">{leagueTable.error.message}</Alert>;
  }

  if (leagueTable.isSuccess) {
    return (
      <TableContainer component={Paper}>
        <Table size="small">
          <TableHead>
            <TableRow>
              <TableCell />
              <TableCell />
              <TableCell />
              <TableCell>P</TableCell>
              <TableCell>W</TableCell>
              <TableCell>D</TableCell>
              <TableCell>L</TableCell>
              <TableCell>GF</TableCell>
              <TableCell>GA</TableCell>
              <TableCell>Diff</TableCell>
              <TableCell>GAv</TableCell>
              <TableCell>PPG</TableCell>
              <TableCell>Points</TableCell>
              <TableCell />
            </TableRow>
          </TableHead>
          <TableBody>
            {leagueTable.data.table
              .sort((a, b) => a.position - b.position)
              .map((row) => (
                <LeagueTableRow key={row.teamId} row={row} />
              ))}
          </TableBody>
        </Table>
      </TableContainer>
    );
  }

  return <CircularProgress />;
};

export { LeagueTable };
