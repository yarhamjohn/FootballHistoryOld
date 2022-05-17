import Typography from "@mui/material/Typography/Typography";
import { FC, ReactElement } from "react";

const Summary: FC = (): ReactElement => {
  return (
    <div style={{ marginBottom: "1.25rem" }}>
      <Typography gutterBottom variant={"h3"}>
        History of the English Football League
      </Typography>
      <Typography gutterBottom variant={"body1"}>
        This website provides a variety of metrics including league tables, match results and
        historical performance data for each of the teams to have featured in the top 4 divisions of
        the English Football League since its founding in 1888.
      </Typography>
    </div>
  );
};

export { Summary };
