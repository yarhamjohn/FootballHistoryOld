import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import { FC, ReactElement } from "react";
import { Competition } from "../../Domain/Types";
import { DrillDownMatchForm } from "./DrillDown/DrillDownMatchForm";
import { DrillDownPosition } from "./DrillDown/DrillDownPosition";

type Props = { competition: Competition; teamId: number };

const LeagueTableDrillDown: FC<Props> = ({ competition, teamId }): ReactElement => {
  return (
    <Card>
      <CardContent>
        <DrillDownMatchForm />
        <div style={{ height: "200px", position: "relative" }}>
          <DrillDownPosition competition={competition} teamId={teamId} />
        </div>
      </CardContent>
    </Card>
  );
};

export { LeagueTableDrillDown };
