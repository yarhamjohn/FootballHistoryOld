import { Typography } from "@mui/material";
import Avatar from "@mui/material/Avatar/Avatar";
import Chip from "@mui/material/Chip/Chip";
import Paper from "@mui/material/Paper/Paper";
import { FC, ReactElement } from "react";
import { Row } from "../../../../Domain/Types";

type Props = {
  leagueTableRows: Row[];
};

const PointDeduction: FC<Props> = ({ leagueTableRows }): ReactElement => {
  const rowsWithDeductions = leagueTableRows.filter((r) => r.pointsDeducted !== 0);

  if (rowsWithDeductions.length === 0) {
    return <></>;
  }

  return (
    <Paper style={{ paddingTop: "1rem", paddingBottom: "0.5rem", paddingLeft: "2rem" }}>
      <Typography gutterBottom variant={"h6"}>
        * Points deducted
      </Typography>
      {rowsWithDeductions.map((r) => (
        <p key={r.position}>
          <Chip
            avatar={<Avatar>{`-${r.pointsDeducted}`}</Avatar>}
            label={`${r.team} (${r.pointsDeductionReason})`}
          />
        </p>
      ))}
    </Paper>
  );
};

export { PointDeduction };
