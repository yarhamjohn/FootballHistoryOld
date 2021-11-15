import { useApi } from "./useApi";
import { useFetch } from "./useFetch";
import { Competition } from "../competitionsSlice";

export type Row = {
  position: number;
  teamId: number;
  team: string;
  played: number;
  won: number;
  drawn: number;
  lost: number;
  goalsFor: number;
  goalsAgainst: number;
  goalDifference: number;
  goalAverage: number;
  points: number;
  pointsPerGame: number;
  pointsDeducted: number;
  pointsDeductionReason: string | null;
  status: string | null;
};

export type League = {
  table: Row[];
  competition: Competition;
};

type FetchLeagueProps =
  | {
      competitionId: number;
    }
  | {
      teamId: number;
      seasonId: number;
    };

const useFetchLeague = (props: FetchLeagueProps) => {
  const api = useApi();

  const url =
    "competitionId" in props
      ? `${api}/api/v2/league-table/competition/${props.competitionId}`
      : `${api}/api/v2/league-table/season/${props.seasonId}/team/${props.teamId}`;

  return useFetch<League>(url);
};

export { useFetchLeague };
