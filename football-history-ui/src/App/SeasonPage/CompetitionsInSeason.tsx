import { FunctionComponent, useState } from "react";
import { Tab } from "semantic-ui-react";
import { Competition, useGetAllCompetitionsQuery } from "../competitionsSlice";
import { useAppDispatch, useAppSelector } from "../../reduxHooks";
import { ErrorMessage } from "../components/ErrorMessage";
import { useEffect } from "react";
import { CompetitionInSeasonPane } from "./CompetitionInSeasonPane";
import { setSelectedCompetition } from "../selectionSlice";

const CompetitionsInSeason: FunctionComponent = () => {
  const [competitions, setCompetitions] = useState<Competition[]>([]);
  const [tabIndex, setTabIndex] = useState<number>(0);

  const dispatch = useAppDispatch();
  const selectedSeason = useAppSelector((state) => state.selected.selectedSeason);

  const { competitionsInSeason } = useGetAllCompetitionsQuery(undefined, {
    selectFromResult: ({ data }) => ({
      competitionsInSeason:
        data === undefined ? [] : data.filter((x) => x.season.id === selectedSeason?.id),
    }),
  });

  useEffect(() => {
    setCompetitions(competitionsInSeason);
  }, [selectedSeason]);

  useEffect(() => {
    if (competitions.length === 0) {
      return;
    }

    if (tabIndex >= competitions.length) {
      setTabIndex(0);
    }

    dispatch(setSelectedCompetition({ ...competitions[tabIndex] }));
  }, [tabIndex, competitions]);

  if (selectedSeason === undefined) {
    return (
      <ErrorMessage
        header={"Oops something went wrong - no season was selected!"}
        content={"Please ensure a season is selected."}
      />
    );
  }

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
      render: () => <CompetitionInSeasonPane competition={x} season={selectedSeason} />,
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
