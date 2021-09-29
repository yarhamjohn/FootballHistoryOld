import React, { FunctionComponent } from "react";
import { Divider, Tab } from "semantic-ui-react";
import { useAppSelector } from "../../reduxHooks";
import { Competition } from "../competitionsSlice";
import { ErrorMessage } from "../components/ErrorMessage";
import { League } from "../components/League";
import { ResultsGrid } from "../LeaguePage/Matches/ResultsGrid";
import { Season } from "../seasonsSlice";
import { useFetchLeagueMatches } from "../shared/useFetchLeagueMatches";

const CompetitionInSeasonPane: FunctionComponent<{ competition: Competition; season: Season }> = ({
  competition,
  season,
}) => {
  const leagueMatches = useFetchLeagueMatches({ competitionId: competition.id });

  return (
    <Tab.Pane>
      <League props={{ season, competition }} />
      {leagueMatches.status === "LOAD_SUCCESSFUL" && (
        <>
          <Divider />
          <ResultsGrid matches={leagueMatches.data} />
        </>
      )}
    </Tab.Pane>
  );
};

export { CompetitionInSeasonPane };
