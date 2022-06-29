import Divider from "@mui/material/Divider/Divider";
import { FC, ReactElement } from "react";
import { Leagues } from "../../Components/Leagues/Leagues";
import { SeasonFilter } from "../../Components/SeasonFilter/SeasonFilter";

const Seasons: FC = (): ReactElement => {
  return (
    <div style={{ width: "100%", alignItems: "center", display: "flex", flexDirection: "column" }}>
      <SeasonFilter />
      <Divider style={{ width: "100%", marginBottom: "2rem" }} />
      <div style={{ width: "80%" }}>
        <Leagues />
      </div>
    </div>
  );
};

export { Seasons };
