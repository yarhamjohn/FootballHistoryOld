import { Collapse, IconButton, TableCell, TableRow } from "@mui/material";
import { FC, ReactElement, useContext, useState } from "react";
import { Competition, Row, Size } from "../../../../../Domain/Types";
import KeyboardArrowDownIcon from "@mui/icons-material/KeyboardArrowDown";
import KeyboardArrowUpIcon from "@mui/icons-material/KeyboardArrowUp";
import { LeagueTableDrillDown } from "../DrillDown/LeagueTableDrilldown";
import { useLeagueTableRow } from "./useLeagueTableRow";
import { TeamsContext } from "../../../../../Contexts/TeamsContext";

type Props = { row: Row; size: Size; competition: Competition; openActiveTeamRow: boolean };

const LeagueTableRow: FC<Props> = ({ row, size, competition, openActiveTeamRow }): ReactElement => {
  const { activeTeam } = useContext(TeamsContext);
  const [open, setOpen] = useState<boolean>(openActiveTeamRow && activeTeam?.id === row.teamId);
  const { rowColor, fontColor, goalAverage, pointsPerGame, points } = useLeagueTableRow(row);

  return (
    <>
      <TableRow sx={{ backgroundColor: rowColor }}>
        <TableCell>
          <IconButton size="small" onClick={() => setOpen(!open)}>
            {open ? (
              <KeyboardArrowUpIcon sx={{ color: fontColor }} />
            ) : (
              <KeyboardArrowDownIcon sx={{ color: fontColor }} />
            )}
          </IconButton>
        </TableCell>
        <TableCell sx={{ color: fontColor }}>{row.position}</TableCell>
        <TableCell sx={{ color: fontColor }}>{row.team}</TableCell>
        <TableCell sx={{ color: fontColor }}>{row.played}</TableCell>
        <TableCell sx={{ color: fontColor }}>{row.won}</TableCell>
        <TableCell sx={{ color: fontColor }}>{row.drawn}</TableCell>
        <TableCell sx={{ color: fontColor }}>{row.lost}</TableCell>
        {size === "large" && <TableCell sx={{ color: fontColor }}>{row.goalsFor}</TableCell>}
        {size === "large" && <TableCell sx={{ color: fontColor }}>{row.goalsAgainst}</TableCell>}
        <TableCell sx={{ color: fontColor }}>{row.goalDifference}</TableCell>
        {size === "large" && <TableCell sx={{ color: fontColor }}>{goalAverage}</TableCell>}
        {size === "large" && <TableCell sx={{ color: fontColor }}>{pointsPerGame}</TableCell>}
        <TableCell sx={{ color: fontColor }}>{points}</TableCell>
        <TableCell sx={{ color: fontColor }}>{row.status}</TableCell>
      </TableRow>
      <TableRow>
        <TableCell colSpan={size === "large" ? 14 : 10} style={{ padding: 0 }}>
          <Collapse in={open} timeout="auto">
            {open ? <LeagueTableDrillDown competition={competition} teamId={row.teamId} /> : null}
          </Collapse>
        </TableCell>
      </TableRow>
    </>
  );
};

export { LeagueTableRow };
