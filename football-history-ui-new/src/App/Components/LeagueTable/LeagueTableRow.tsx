import { Collapse, IconButton, TableCell, TableRow } from "@mui/material";
import { FC, ReactElement, useContext, useState } from "react";
import { Competition, Row, Size } from "../../Domain/Types";
import KeyboardArrowDownIcon from "@mui/icons-material/KeyboardArrowDown";
import KeyboardArrowUpIcon from "@mui/icons-material/KeyboardArrowUp";
import { getLeagueStatusColor } from "../../Domain/Colors";
import { LeagueTableDrillDown } from "./LeagueTableDrilldown";
import { blue, red } from "@mui/material/colors";
import { ColorModeContext } from "../../Contexts/ColorModeContext";

type Props = { row: Row; size: Size; competition: Competition };

const LeagueTableRow: FC<Props> = ({ row, size, competition }): ReactElement => {
  const [open, setOpen] = useState<boolean>(false);
  const { mode } = useContext(ColorModeContext);

  const rowColor = getLeagueStatusColor(row.status);
  const fontColor =
    rowColor === red[500] || rowColor === blue[500] || (rowColor === null && mode === "dark")
      ? "white"
      : "black";

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
        {size === "large" && (
          <TableCell sx={{ color: fontColor }}>
            {+(Math.round(parseFloat(row.goalAverage + "e+2")) + "e-2")}
          </TableCell>
        )}
        {size === "large" && (
          <TableCell sx={{ color: fontColor }}>
            {+(Math.round(parseFloat(row.pointsPerGame + "e+2")) + "e-2")}
          </TableCell>
        )}
        <TableCell sx={{ color: fontColor }}>{row.points}</TableCell>
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
