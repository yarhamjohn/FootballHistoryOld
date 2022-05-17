import Card from "@mui/material/Card/Card";
import CardContent from "@mui/material/CardContent/CardContent";
import Typography from "@mui/material/Typography/Typography";
import { CSSProperties, FC, ReactElement } from "react";
import { LeagueStructureTimeline } from "./LeagueStructureTimeline/LeagueStructureTimeline";

type Props = {
  style?: CSSProperties;
};

const LeagueStructure: FC<Props> = ({ style }): ReactElement => {
  return (
    <Card style={{ ...style }}>
      <CardContent>
        <Typography gutterBottom variant={"h4"}>
          Timeline of League structure
        </Typography>
        <LeagueStructureTimeline />
      </CardContent>
    </Card>
  );
};

export { LeagueStructure };
