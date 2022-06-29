import { Typography } from "@mui/material";
import Avatar from "@mui/material/Avatar/Avatar";
import Chip from "@mui/material/Chip/Chip";
import Paper from "@mui/material/Paper/Paper";
import Box from "@mui/system/Box/Box";
import { FC, ReactElement } from "react";
import { Row } from "../../../../Domain/Types";

type Props = {
  leagueTableRows: Row[];
};

const usePointDeduction = (
  leagueTableRows: Row[]
): { deductions: { pointsDeducted: string; label: string }[] } => {
  const deductions = leagueTableRows
    .filter((r) => r.pointsDeducted !== 0)
    .map((r) => {
      return {
        pointsDeducted: `-${r.pointsDeducted}`,
        label: `${r.team} (${r.pointsDeductionReason})`
      };
    });

  return { deductions };
};

const PointDeduction: FC<Props> = ({ leagueTableRows }): ReactElement => {
  const { deductions } = usePointDeduction(leagueTableRows);

  if (deductions.length === 0) {
    return <></>;
  }

  return (
    <Paper style={{ paddingTop: "1rem", paddingBottom: "0.5rem", paddingLeft: "2rem" }}>
      <Typography gutterBottom variant={"h6"}>
        * Points deducted
      </Typography>
      <Box sx={{ paddingTop: "1rem", paddingBottom: "1rem", display: "flex", columnGap: "1rem" }}>
        {deductions.map((r) => (
          <Chip avatar={<Avatar>{r.pointsDeducted}</Avatar>} label={r.label} />
        ))}
      </Box>
    </Paper>
  );
};

export { PointDeduction };
