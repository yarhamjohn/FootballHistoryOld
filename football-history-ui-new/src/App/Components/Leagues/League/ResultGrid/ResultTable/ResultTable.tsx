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
import { Match } from "../../../../../Domain/Types";
import { ResultTableRow } from "./Row/ResultTableRow";

type Props = { matches: Match[] };

const ResultTable: FC<Props> = ({ matches }): ReactElement => {
  const leagueMatches = matches.filter((m) => m.rules.type === "League");
  const abbreviations = Array.from(
    new Set(leagueMatches.map((m) => m.homeTeam.abbreviation))
  ).sort();

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
            />
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export { ResultTable };
