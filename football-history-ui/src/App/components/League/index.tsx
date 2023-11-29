import React, { FunctionComponent } from "react";
import { Loader } from "semantic-ui-react";
import { PointDeductionSummary } from "./PointDeductionSummary";
import { useFetchLeague } from "../../shared/useFetchLeague";
import { ErrorMessage } from "../ErrorMessage";
import { LeagueTable } from "./Table/Table";
import { Season } from "../../seasonsSlice";
import { Team } from "../../teamsSlice";
import { Competition } from "../../competitionsSlice";

type FetchLeagueProps =
  | {
      season: Season;
      competition: Competition;
    }
  | {
      season: Season;
      team: Team;
    };

const League: FunctionComponent<{ props: FetchLeagueProps }> = ({ props }) => {
  const league = useFetchLeague(
    "team" in props
      ? { seasonId: props.season.id, teamId: props.team.id }
      : { competitionId: props.competition.id }
  );

  if (league.status === "UNLOADED") {
    return null;
  }

  if (league.status === "LOADING") {
    return (
      <div style={{ display: "flex", justifyContent: "center" }}>
        <Loader active inline size={"huge"} />
      </div>
    );
  }

  if (league.status === "LOAD_FAILED") {
    return <ErrorMessage header={"Something went wrong"} content={league.error} />;
  }

  return (
    <div>
      <LeagueTable league={league.data} highlightSelectedTeam={"team" in props} />
      <PointDeductionSummary leagueTable={league.data.table} />
    </div>
  );
};

export { League };
