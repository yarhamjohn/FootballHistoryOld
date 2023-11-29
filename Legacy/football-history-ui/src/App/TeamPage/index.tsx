import { FunctionComponent } from "react";
import { Divider, Message } from "semantic-ui-react";
import { TeamFilter } from "../components/Filters/TeamFilter";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { Matches } from "./Matches";
import { HistoricalPositions } from "../components/HistoricalPositions";
import { League } from "../components/League";
import { useAppSelector } from "../../reduxHooks";

const TeamPage: FunctionComponent = () => {
  const selectedSeason = useAppSelector((state) => state.selected.selectedSeason);
  const selectedTeam = useAppSelector((state) => state.selected.selectedTeam);

  return (
    <>
      <TeamFilter />
      <Divider />
      {selectedTeam === undefined ? (
        <Message info>Please select a team from the dropdown filter box.</Message>
      ) : (
        <>
          <h2>League positions by season</h2>
          <HistoricalPositions teamId={selectedTeam.id} />
          {selectedSeason && (
            <>
              <Divider />
              <div
                style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}
              >
                <h2>League table for season:</h2>
                <SeasonFilter />
              </div>
              <League props={{ season: selectedSeason, team: selectedTeam }} />
              <h2>League matches</h2>
              <Matches selectedSeason={selectedSeason} selectedTeam={selectedTeam} />
            </>
          )}
        </>
      )}
    </>
  );
};

export { TeamPage };
