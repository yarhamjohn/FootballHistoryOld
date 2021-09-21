import React, { FunctionComponent, useState } from "react";
import { Tab } from "semantic-ui-react";
import {
  Competition,
  selectCompetitionsBySeasonId,
  setSelectedCompetition,
} from "../competitionsSlice";
import { useAppDispatch, useAppSelector } from "../../reduxHooks";
import { ErrorMessage } from "../components/ErrorMessage";
import { useEffect } from "react";
import { CompetitionInSeasonPane } from "./CompetitionInSeasonPane";

const CompetitionsInSeason: FunctionComponent = () => {
  const [competitions, setCompetitions] = useState<Competition[]>([]);
  const [tabIndex, setTabIndex] = useState<number>(0);

  const dispatch = useAppDispatch();
  const competitionState = useAppSelector((state) => state.competition);
  const selectedSeasonId = useAppSelector((state) => state.season.selectedSeason?.id);

  useEffect(() => {
    const competitions = selectCompetitionsBySeasonId(competitionState, selectedSeasonId);
    setCompetitions(competitions);
  }, [competitionState, selectedSeasonId]);

  useEffect(() => {
    if (competitions.length === 0) {
      return;
    }

    if (tabIndex >= competitions.length) {
      setTabIndex(0);
    }

    dispatch(setSelectedCompetition({ ...competitions[tabIndex] }));
  }, [tabIndex, competitions]);

  if (competitions.length === 0) {
    return (
      <ErrorMessage
        header={"No competitions were found for the selected season"}
        content={"Please ensure a season is selected."}
      />
    );
  }

  const panes = competitions.map((x) => {
    return {
      menuItem: x.name,
      render: () => <CompetitionInSeasonPane />,
    };
  });

  return (
    <Tab
      panes={panes}
      activeIndex={tabIndex}
      onTabChange={(_, { activeIndex }) => setTabIndex(activeIndex as number)}
    />
  );
};

export { CompetitionsInSeason };
