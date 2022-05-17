import { FC, ReactElement } from "react";
import { LeagueStructure } from "./LeagueStructure/LeagueStructure";
import { RulesAndRegulations } from "./RulesAndRegulations/RulesAndRegulations";
import { Summary } from "./Summary/Summary";

const Home: FC = (): ReactElement => {
  return (
    <div>
      <Summary />
      <LeagueStructure style={{ marginBottom: "1.25rem" }} />
      <RulesAndRegulations />
    </div>
  );
};

export { Home };
