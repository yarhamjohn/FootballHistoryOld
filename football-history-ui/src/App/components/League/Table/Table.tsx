import React, { FunctionComponent } from "react";
import { Table } from "semantic-ui-react";
import { League } from "../../../shared/useFetchLeague";
import { LeagueTableRow } from "./Row";

const LeagueTable: FunctionComponent<{
  league: League;
  highlightSelectedTeam: boolean;
}> = ({ league, highlightSelectedTeam }) => {
  const rows = league.table
    .sort((a, b) => a.position - b.position)
    .map((r) => (
      <LeagueTableRow
        key={r.position}
        row={r}
        numRows={league.table.length}
        rules={league.competition.rules}
        competitionId={league.competition.id}
        highlightSelectedTeam={highlightSelectedTeam}
      />
    ));

  return (
    <Table basic compact>
      <Table.Header>
        <Table.Row>
          <Table.HeaderCell></Table.HeaderCell>
          <Table.HeaderCell></Table.HeaderCell>
          <Table.HeaderCell></Table.HeaderCell>
          <Table.HeaderCell>P</Table.HeaderCell>
          <Table.HeaderCell>W</Table.HeaderCell>
          <Table.HeaderCell>D</Table.HeaderCell>
          <Table.HeaderCell>L</Table.HeaderCell>
          <Table.HeaderCell>GF</Table.HeaderCell>
          <Table.HeaderCell>GA</Table.HeaderCell>
          <Table.HeaderCell>Diff</Table.HeaderCell>
          <Table.HeaderCell>GAv</Table.HeaderCell>
          <Table.HeaderCell>PPG</Table.HeaderCell>
          <Table.HeaderCell>Points</Table.HeaderCell>
          <Table.HeaderCell></Table.HeaderCell>
        </Table.Row>
      </Table.Header>
      <Table.Body>{rows}</Table.Body>
    </Table>
  );
};

export { LeagueTable };
