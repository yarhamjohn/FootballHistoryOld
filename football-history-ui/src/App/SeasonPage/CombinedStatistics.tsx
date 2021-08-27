import React, { FunctionComponent } from "react";
import { Loader, Table } from "semantic-ui-react";
import { ErrorMessage } from "../components/ErrorMessage";
import { useFetchStatistics } from "../shared/useFetchStatistics";

const CombinedStatistics: FunctionComponent<{ seasonId: number }> = ({ seasonId }) => {
  const statistics = useFetchStatistics(seasonId);

  if (statistics.status === "LOADING") {
    return <Loader />;
  }

  if (statistics.status === "LOAD_FAILED") {
    return (
      <ErrorMessage
        header="Failed to load statistics"
        content="Something went wrong attempting to load the season statistics"
      />
    );
  }

  if (statistics.status === "UNLOADED") {
    return null;
  }

  return (
    <Table color="red">
      <Table.Header>
        <Table.Row>
          <Table.HeaderCell>Category</Table.HeaderCell>
          <Table.HeaderCell>Statistic</Table.HeaderCell>
          <Table.HeaderCell>Value</Table.HeaderCell>
          <Table.HeaderCell>Team</Table.HeaderCell>
          <Table.HeaderCell>Competition</Table.HeaderCell>
        </Table.Row>
      </Table.Header>
      <Table.Body>
        {statistics.data.map((s, i) => (
          <Table.Row key={i}>
            <Table.Cell>{s.category}</Table.Cell>
            <Table.Cell>{s.name}</Table.Cell>
            <Table.Cell>{Number(s.value.toFixed(2))}</Table.Cell>
            <Table.Cell>{s.teamName}</Table.Cell>
            <Table.Cell>{s.competitionName}</Table.Cell>
          </Table.Row>
        ))}
      </Table.Body>
    </Table>
  );
};

export { CombinedStatistics };
