import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import { FC, ReactElement } from "react";
import { Competition } from "../../../../../Domain/Types";
import { DrillDownForm } from "./Form/DrillDownForm";
import { DrillDownGraph } from "./Graph/DrillDownGraph";

type Props = { competition: Competition; teamId: number };

const LeagueTableDrillDown: FC<Props> = ({ competition, teamId }): ReactElement => {
  return (
    <Card>
      <CardContent>
        <DrillDownForm competitionId={competition.id} teamId={teamId} />
        <div style={{ height: "200px", position: "relative" }}>
          <DrillDownGraph competition={competition} teamId={teamId} />
        </div>
      </CardContent>
    </Card>
  );
};

export { LeagueTableDrillDown };
