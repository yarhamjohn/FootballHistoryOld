import Card from "@mui/material/Card/Card";
import CardContent from "@mui/material/CardContent/CardContent";
import Typography from "@mui/material/Typography/Typography";
import { FC, ReactElement } from "react";
import { ScheduleChanges } from "./ScheduleChanges/ScheduleChanges";
import { Classification } from "./Classification/Classification";
import { Points } from "./Points/Points";
import { PromotionAndRelegation } from "./PromotionAndRelegation/PromotionAndRelegation";

const RulesAndRegulations: FC = (): ReactElement => {
  return (
    <Card>
      <CardContent>
        <Typography gutterBottom variant={"h4"}>
          Rules and regulations
        </Typography>
        <div style={{ display: "flex", marginBottom: "1.25rem" }}>
          <Points />
          <Classification />
          <ScheduleChanges />
        </div>
        <PromotionAndRelegation />
      </CardContent>
    </Card>
  );
};

export { RulesAndRegulations };
