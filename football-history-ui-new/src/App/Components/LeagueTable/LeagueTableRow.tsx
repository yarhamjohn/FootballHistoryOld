import { Box, Collapse, IconButton, TableCell, TableRow } from "@mui/material";
import { FC, ReactElement, useState } from "react";
import { Row } from "../../Domain/Types";
import KeyboardArrowDownIcon from "@mui/icons-material/KeyboardArrowDown";
import KeyboardArrowUpIcon from "@mui/icons-material/KeyboardArrowUp";
import { getLeagueStatusColor } from "../../Domain/Colors";

type Props = { row: Row };

const LeagueTableRow: FC<Props> = ({ row }): ReactElement => {
  const [open, setOpen] = useState<boolean>(false);

  return (
    <>
      <TableRow sx={{ backgroundColor: getLeagueStatusColor(row.status) }}>
        <TableCell>
          <IconButton size="small" onClick={() => setOpen(!open)}>
            {open ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
          </IconButton>
        </TableCell>
        <TableCell>{row.position}</TableCell>
        <TableCell>{row.team}</TableCell>
        <TableCell>{row.played}</TableCell>
        <TableCell>{row.won}</TableCell>
        <TableCell>{row.drawn}</TableCell>
        <TableCell>{row.lost}</TableCell>
        <TableCell>{row.goalsFor}</TableCell>
        <TableCell>{row.goalsAgainst}</TableCell>
        <TableCell>{row.goalDifference}</TableCell>
        <TableCell>{+(Math.round(parseFloat(row.goalAverage + "e+2")) + "e-2")}</TableCell>
        <TableCell>{+(Math.round(parseFloat(row.pointsPerGame + "e+2")) + "e-2")}</TableCell>
        <TableCell>{row.points}</TableCell>
        <TableCell>{row.status}</TableCell>
      </TableRow>
      <TableRow>
        <TableCell colSpan={14} style={{ padding: 0 }}>
          <Collapse in={open} timeout="auto">
            <Box>
              <div>show something</div>
            </Box>
          </Collapse>
        </TableCell>
      </TableRow>
    </>
  );
};

export { LeagueTableRow };
