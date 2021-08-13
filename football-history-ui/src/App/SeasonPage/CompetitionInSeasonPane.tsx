import React, { FunctionComponent } from "react";
import { Tab } from "semantic-ui-react";
import { useAppSelector } from "../../reduxHooks";
import { ErrorMessage } from "../components/ErrorMessage";
import { League } from "../components/League";

const CompetitionInSeasonPane: FunctionComponent = () => {
  const selectedCompetition = useAppSelector((state) => state.competition.selectedCompetition);
  const selectedSeason = useAppSelector((state) => state.season.selectedSeason);

  if (selectedSeason === undefined || selectedCompetition === undefined) {
    return (
      <ErrorMessage
        header={"Oops something went wrong"}
        content={"No season or competition is selected."}
      />
    );
  }

  return (
    <Tab.Pane>
      <League
        props={{
          season: selectedSeason,
          competition: selectedCompetition,
        }}
      />
    </Tab.Pane>
  );
};

export { CompetitionInSeasonPane };
