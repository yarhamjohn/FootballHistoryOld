import Card from "@mui/material/Card/Card";
import CardContent from "@mui/material/CardContent/CardContent";
import Typography from "@mui/material/Typography/Typography";
import { FC, ReactElement } from "react";
import { LeagueStructureTimeline } from "./LeagueStructureTimeline/LeagueStructureTimeline";

const LeagueStructure: FC = (): ReactElement => {
  return (
    <Card>
      <CardContent>
        <Typography gutterBottom variant={"h4"}>
          Timeline of structural changes to the leagues
        </Typography>
        <LeagueStructureTimeline />
      </CardContent>
    </Card>
  );
};

export { LeagueStructure };
