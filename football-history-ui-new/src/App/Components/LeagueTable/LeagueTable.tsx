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

type Props = { competition: Competition; size: "small" | "large" };

const LeagueTable: FC<Props> = ({ competition, size }): ReactElement => {
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
            {leagueTable.data.table
              .sort((a, b) => a.position - b.position)
              .map((row) => (
                <LeagueTableRow key={row.teamId} row={row} size={size} />
              ))}
          </TableBody>
        </Table>
      </TableContainer>
    );
  }

  return <CircularProgress />;
};

export { LeagueTable };
