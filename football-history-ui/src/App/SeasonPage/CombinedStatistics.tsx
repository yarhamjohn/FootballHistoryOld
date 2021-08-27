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

  console.log(statistics.data);

  return (
    <>
      {statistics.data.map((x) => (
        <Table color="red" key={x.category}>
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
            {x.statistics.map((y, i) => (
              <Table.Row key={i}>
                {i == 0 && <Table.Cell rowSpan={x.statistics.length}>{x.category}</Table.Cell>}
                <Table.Cell>{y.name}</Table.Cell>
                <Table.Cell>{Number(y.value.toFixed(2))}</Table.Cell>
                <Table.Cell>{y.teamName}</Table.Cell>
                <Table.Cell>{y.competitionName}</Table.Cell>
              </Table.Row>
            ))}
          </Table.Body>
        </Table>
      ))}
    </>
  );
};

export { CombinedStatistics };
