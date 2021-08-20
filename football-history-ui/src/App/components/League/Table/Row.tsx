import React, { FunctionComponent, useEffect, useState } from "react";
import { Row } from "../../../shared/useFetchLeague";
import { Icon, Table } from "semantic-ui-react";
import { LeagueTableDrillDown } from "../DrillDown/DrillDown";
import { LeagueTableRowCell } from "./Cell";
import { useLeagueTableRow } from "./useLeagueTableRow";
import { selectTeamById, setSelectedTeam, Team } from "../../../teamsSlice";
import { useAppDispatch, useAppSelector } from "../../../../reduxHooks";
import { CompetitionRules } from "../../../competitionsSlice";
import { Color, getLeagueStatusColor } from "../../../shared/functions";

const LeagueTableRow: FunctionComponent<{
  row: Row;
  numRows: number;
  rules: CompetitionRules;
  competitionId: number;
  highlightSelectedTeam: boolean;
}> = ({ row, numRows, rules, competitionId, highlightSelectedTeam }) => {
  const dispatch = useAppDispatch();
  const teamState = useAppSelector((state) => state.team);

  const [selectedTeam] = useState<boolean>(
    highlightSelectedTeam && row.team === teamState.selectedTeam?.name
  );
  const [rowColor] = useState<Color | null>(
    getLeagueStatusColor(row.status) == null && selectedTeam
      ? Color.Grey
      : getLeagueStatusColor(row.status)
  );

  const [showDrillDown, setShowDrillDown] = useState<boolean>(
    highlightSelectedTeam && selectedTeam
  );

  const selectTeam = (teamId: number) => {
    const team = selectTeamById(teamState, teamId);
    setShowDrillDown(true);
    dispatch(setSelectedTeam(team));
  };

  return (
    <>
      <Table.Row
        style={{
          cursor: "pointer",
        }}
        onClick={() => setShowDrillDown(!showDrillDown)}
      >
        <LeagueTableRowCell bold={selectedTeam} color={rowColor}>
          {showDrillDown ? <Icon name="chevron down" /> : <Icon name="chevron right" />}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={selectedTeam} color={rowColor}>
          {row.position}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={selectedTeam} color={rowColor}>
          {row.team}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={selectedTeam} color={rowColor}>
          {row.played}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={selectedTeam} color={rowColor}>
          {row.won}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={selectedTeam} color={rowColor}>
          {row.drawn}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={selectedTeam} color={rowColor}>
          {row.lost}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={selectedTeam} color={rowColor}>
          {row.goalsFor}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={selectedTeam} color={rowColor}>
          {row.goalsAgainst}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={selectedTeam} color={rowColor}>
          {row.goalDifference}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={selectedTeam} color={rowColor}>
          {Number(Math.round(parseFloat(row.goalAverage + "e4")) + "e-4")}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={selectedTeam} color={rowColor}>
          {Number(Math.round(parseFloat(row.pointsPerGame + "e2")) + "e-2")}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={selectedTeam} color={rowColor}>
          {row.points}
          {row.pointsDeducted > 0 ? " *" : ""}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={selectedTeam} color={rowColor}>
          {row.status}
        </LeagueTableRowCell>
      </Table.Row>
      {showDrillDown ? (
        <LeagueTableDrillDown
          teamId={row.teamId}
          competitionId={competitionId}
          numRows={numRows}
          rules={rules}
        />
      ) : null}
    </>
  );
};

export { LeagueTableRow };
