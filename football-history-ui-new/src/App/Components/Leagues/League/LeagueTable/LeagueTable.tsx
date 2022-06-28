import {
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow
} from "@mui/material";
import { FC, ReactElement } from "react";
import { League, Size } from "../../../../Domain/Types";
import { LeagueTableRow } from "./Row/LeagueTableRow";

type Props = { league: League; size: Size };

const LeagueTable: FC<Props> = ({ league, size }): ReactElement => {
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
          {league.table
            .sort((a, b) => a.position - b.position)
            .map((row) => (
              <LeagueTableRow
                key={row.teamId}
                row={row}
                size={size}
                competition={league.competition}
              />
            ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export { LeagueTable };
