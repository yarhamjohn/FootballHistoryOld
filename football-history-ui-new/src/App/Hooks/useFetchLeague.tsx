import { useQuery } from "react-query";
import {
  fetchData,
  getLeagueTableByCompetitionUrl,
  getLeagueTableBySeasonAndTeamUrl
} from "../Domain/Api";
import { League } from "../Domain/Types";

const useFetchLeagueByCompetition = (competitionId: number) =>
  useQuery<League, Error>(["league-table-competition", { competitionId }], () =>
    fetchData(getLeagueTableByCompetitionUrl(competitionId))
  );

const useFetchLeagueBySeasonAndTeam = (seasonId: number, teamId: number) =>
  useQuery<League, Error>(["league-table-season-team", { seasonId, teamId }], () =>
    fetchData(getLeagueTableBySeasonAndTeamUrl(seasonId, teamId))
  );

export { useFetchLeagueByCompetition, useFetchLeagueBySeasonAndTeam };
