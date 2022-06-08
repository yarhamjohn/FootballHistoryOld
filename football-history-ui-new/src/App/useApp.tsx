import { Competition, Season, Team } from "./Domain/Types";
import { useFetchCompetitions } from "./Hooks/useFetchCompetitions";
import { useFetchSeasons } from "./Hooks/useFetchSeasons";
import { useFetchTeams } from "./Hooks/useFetchTeams";

type AppState =
  | { status: "LOADING" }
  | {
      status: "LOAD_SUCCESS";
      data: { teams: Team[]; seasons: Season[]; competitions: Competition[] };
    }
  | { status: "LOAD_FAILED"; error: Error };

const useApp = (): AppState => {
  const teamsState = useFetchTeams();
  const seasonsState = useFetchSeasons();
  const competitionsState = useFetchCompetitions();

  if (teamsState.isError) {
    return { status: "LOAD_FAILED", error: teamsState.error };
  }

  if (seasonsState.isError) {
    return { status: "LOAD_FAILED", error: seasonsState.error };
  }

  if (competitionsState.isError) {
    return { status: "LOAD_FAILED", error: competitionsState.error };
  }

  if (teamsState.isSuccess && seasonsState.isSuccess && competitionsState.isSuccess) {
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
