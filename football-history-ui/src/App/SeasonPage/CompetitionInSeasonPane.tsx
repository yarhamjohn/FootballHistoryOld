import { FunctionComponent } from "react";
import { Divider, Tab } from "semantic-ui-react";
import { Competition } from "../competitionsSlice";
import { League } from "../components/League";
import { PlayOffs } from "../LeaguePage/Matches/PlayOffs";
import { ResultsGrid } from "../LeaguePage/Matches/ResultsGrid";
import { Season } from "../seasonsSlice";
import { useFetchLeagueMatches } from "../shared/useFetchLeagueMatches";
import { useFetchPlayOffMatches } from "../shared/useFetchPlayOffMatches";

const CompetitionInSeasonPane: FunctionComponent<{ competition: Competition; season: Season }> = ({
  competition,
  season,
}) => {
  const leagueMatches = useFetchLeagueMatches({ competitionId: competition.id });
  const playOffMatches = useFetchPlayOffMatches(competition.id);

  return (
    <Tab.Pane>
      <League props={{ season, competition }} />
      {leagueMatches.status === "LOAD_SUCCESSFUL" && (
        <>
          <Divider />
          <ResultsGrid matches={leagueMatches.data} />
        </>
      )}
      {playOffMatches.status === "LOAD_SUCCESSFUL" && playOffMatches.data.length > 0 && (
        <>
          <Divider />
          <PlayOffs matches={playOffMatches.data} />
        </>
      )}
    </Tab.Pane>
  );
};

export { CompetitionInSeasonPane };
