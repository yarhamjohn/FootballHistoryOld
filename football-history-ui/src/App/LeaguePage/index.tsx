import { FunctionComponent } from "react";
import { Divider } from "semantic-ui-react";
import { CompetitionFilter } from "../components/Filters/CompetitionFilter";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { Matches } from "./Matches";
import { League } from "../components/League";
import { useAppSelector } from "../../reduxHooks";

const LeaguePage: FunctionComponent = () => {
  const selectedSeason = useAppSelector((state) => state.selected.selectedSeason);
  const selectedCompetition = useAppSelector((state) => state.selected.selectedCompetition);

  return (
    <>
      {selectedSeason && <CompetitionFilter />}
      <Divider />
      {selectedCompetition && (
        <>
          <SeasonFilter />
          {selectedSeason && (
            <>
              <League
                props={{
                  season: selectedSeason,
                  competition: selectedCompetition,
                }}
              />
              <div style={{ display: "grid", gridGap: "1rem" }}>
                {selectedSeason ? <Matches competitionId={selectedCompetition.id} /> : null}
              </div>
            </>
          )}
        </>
      )}
    </>
  );
};

export { LeaguePage };
