import { Competition, Season, Team } from "./Domain/Types";
import { useFetchCompetitions } from "./Hooks/useFetchCompetitions";
import { useFetchSeasons } from "./Hooks/useFetchSeasons";
import { useFetchTeams } from "./Hooks/useFetchTeams";

type AppState =
  | { status: "NOT_LOADED" }
  | { status: "LOADING" }
  | {
      status: "LOAD_SUCCESS";
      data: { teams: Team[]; seasons: Season[]; competitions: Competition[] };
    }
  | { status: "LOAD_FAILED"; error: Error };

const useApp = (): AppState => {
  const { teamsState } = useFetchTeams();
  const { seasonsState } = useFetchSeasons();
  const { competitionsState } = useFetchCompetitions();

  if (teamsState.status === "FETCH_ERROR") {
    return { status: "LOAD_FAILED", error: teamsState.error };
  }

  if (seasonsState.status === "FETCH_ERROR") {
    return { status: "LOAD_FAILED", error: seasonsState.error };
  }

  if (competitionsState.status === "FETCH_ERROR") {
    return { status: "LOAD_FAILED", error: competitionsState.error };
  }

  if (
    teamsState.status === "FETCH_SUCCESS" &&
    seasonsState.status === "FETCH_SUCCESS" &&
    competitionsState.status === "FETCH_SUCCESS"
  ) {
    return {
      status: "LOAD_SUCCESS",
      data: {
        teams: teamsState.data,
        seasons: seasonsState.data,
        competitions: competitionsState.data
      }
    };
  }

  return { status: "LOADING" };
};

export { useApp };
