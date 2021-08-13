import React, { FunctionComponent } from "react";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { Divider, Tab } from "semantic-ui-react";
import { selectCompetitionsBySeasonId } from "../competitionsSlice";
import { useAppSelector } from "../../reduxHooks";
import { ErrorMessage } from "../components/ErrorMessage";

const SeasonPage: FunctionComponent = () => {
  const competitionState = useAppSelector((state) => state.competition);
  const selectedSeasonId = useAppSelector((state) => state.season.selectedSeason?.id);

  const competitionsInSeason =
    selectedSeasonId === undefined
      ? []
      : selectCompetitionsBySeasonId(competitionState, selectedSeasonId);

  return (
    <>
      <SeasonFilter />
      <Divider />
      <h1>Records and comments</h1>
      <p>Here will go some stuff about the season</p>
      <Divider />
      <h1>Leagues</h1>
      {competitionsInSeason === undefined ? (
        <ErrorMessage
          header={"No competitions were found for the selected season"}
          content={"Please ensure a season is selected."}
        />
      ) : (
        <Tab
          panes={competitionsInSeason.map((x) => {
            return { menuItem: x.name, render: () => <Tab.Pane>Something</Tab.Pane> };
          })}
          defaultActiveIndex={
            competitionsInSeason.findIndex(
              (x) => x.id === competitionState.selectedCompetition?.id
            ) === -1
              ? 0
              : competitionsInSeason.findIndex(
                  (x) => x.id === competitionState.selectedCompetition?.id
                )
          }
        />
      )}
    </>
  );
};

export { SeasonPage };
