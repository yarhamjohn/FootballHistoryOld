import React, { FunctionComponent } from "react";
import { Table } from "semantic-ui-react";

const CombinedStatistics: FunctionComponent = () => {
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
        <Table.Row>
          <Table.Cell rowSpan="3">Points</Table.Cell>
          <Table.Cell>Most Points</Table.Cell>
          <Table.Cell>3</Table.Cell>
          <Table.Cell>Norwich City</Table.Cell>
          <Table.Cell>Championship</Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell>Fewest Points</Table.Cell>
          <Table.Cell>3</Table.Cell>
          <Table.Cell>Norwich City</Table.Cell>
          <Table.Cell>Championship</Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell>Best Points Per Game</Table.Cell>
          <Table.Cell>3</Table.Cell>
          <Table.Cell>Norwich City</Table.Cell>
          <Table.Cell>Championship</Table.Cell>
        </Table.Row>

        <Table.Row>
          <Table.Cell rowSpan="4">Goals</Table.Cell>
          <Table.Cell>Most Goals</Table.Cell>
          <Table.Cell>3</Table.Cell>
          <Table.Cell>Norwich City</Table.Cell>
          <Table.Cell>Championship</Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell>Fewest Goals</Table.Cell>
          <Table.Cell>3</Table.Cell>
          <Table.Cell>Norwich City</Table.Cell>
          <Table.Cell>Championship</Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell>Best Goal Difference</Table.Cell>
          <Table.Cell>3</Table.Cell>
          <Table.Cell>Norwich City</Table.Cell>
          <Table.Cell>Championship</Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell>Best Goal Average</Table.Cell>
          <Table.Cell>3</Table.Cell>
          <Table.Cell>Norwich City</Table.Cell>
          <Table.Cell>Championship</Table.Cell>
        </Table.Row>

        <Table.Row>
          <Table.Cell rowSpan="6">Results</Table.Cell>
          <Table.Cell>Most Wins</Table.Cell>
          <Table.Cell>3</Table.Cell>
          <Table.Cell>Norwich City</Table.Cell>
          <Table.Cell>Championship</Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell>Most Draws</Table.Cell>
          <Table.Cell>3</Table.Cell>
          <Table.Cell>Norwich City</Table.Cell>
          <Table.Cell>Championship</Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell>Most Losses</Table.Cell>
          <Table.Cell>3</Table.Cell>
          <Table.Cell>Norwich City</Table.Cell>
          <Table.Cell>Championship</Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell>Most Consecutive Wins</Table.Cell>
          <Table.Cell>3</Table.Cell>
          <Table.Cell>Norwich City</Table.Cell>
          <Table.Cell>Championship</Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell>Most Consecutive Draws</Table.Cell>
          <Table.Cell>3</Table.Cell>
          <Table.Cell>Norwich City</Table.Cell>
          <Table.Cell>Championship</Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell>Most Consecutive Losses</Table.Cell>
          <Table.Cell>3</Table.Cell>
          <Table.Cell>Norwich City</Table.Cell>
          <Table.Cell>Championship</Table.Cell>
        </Table.Row>
      </Table.Body>
    </Table>
  );
};

export { CombinedStatistics };
