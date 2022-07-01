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
import { Match, Size } from "../../../../../Domain/Types";
import { ResultTableRow } from "./Row/ResultTableRow";
import { useResultTable } from "./useResultTable";

type Props = { leagueMatches: Match[]; size: Size };

const ResultTable: FC<Props> = ({ leagueMatches, size }): ReactElement => {
  const { abbreviations } = useResultTable(leagueMatches);

  return (
    <TableContainer component={Paper}>
      <Table size="small">
        <TableHead>
          <TableRow>
            <TableCell padding={"none"} align={"center"} />
            {abbreviations.map((a) => (
              <TableCell key={`Header: ${a}`} padding={"none"} align={"center"}>
                {a}
              </TableCell>
            ))}
          </TableRow>
        </TableHead>
        <TableBody>
          {abbreviations.map((a) => (
            <ResultTableRow
              key={a}
              matches={leagueMatches}
              abbreviations={abbreviations}
              teamAbbreviation={a}
              size={size}
            />
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export { ResultTable };
