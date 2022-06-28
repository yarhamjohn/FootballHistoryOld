import Paper from "@mui/material/Paper/Paper";
import { FC, ReactElement } from "react";
import { Row } from "../../../../Domain/Types";

type Props = {
  leagueTableRows: Row[];
};

const PointDeduction: FC<Props> = ({ leagueTableRows }): ReactElement => {
  return (
    <Paper style={{ paddingTop: "0.5rem", paddingBottom: "0.5rem" }}>
      {leagueTableRows
        .filter((r) => r.pointsDeducted !== 0)
        .map((r) => (
          <p key={r.position} style={{ paddingLeft: "2rem" }}>
            * {r.team}: {r.pointsDeducted} point{r.pointsDeducted === 1 ? "" : "s"} deducted -{" "}
            {r.pointsDeductionReason}
          </p>
        ))}
    </Paper>
  );
};

export { PointDeduction };
