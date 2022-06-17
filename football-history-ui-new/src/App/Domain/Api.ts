const apiUrl =
  process.env.NODE_ENV === "development"
    ? "https://localhost:5001"
    : "https://football-history-api.azurewebsites.net";

const getTeamsUrl = () => `${apiUrl}/api/v2/teams`;

const getSeasonsUrl = () => `${apiUrl}/api/v2/seasons`;

const getLeaguePositionsUrl = (competitionId: number, teamId: number) =>
  `${apiUrl}/api/v2/league-positions/competition/${competitionId}/team/${teamId}`;

const getLeagueTableUrl = (competitionId: number) =>
  `${apiUrl}/api/v2/league-table/competition/${competitionId}`;

const getCompetitionsUrl = (seasonId?: number) =>
  `${apiUrl}/api/v2/competitions${seasonId === undefined ? "" : `/season/${seasonId}`}`;

const getHistoricalPositionsUrl = (teamId: number, seasonIds: number[]) => {
  const seasonIdsForQuery = seasonIds
    .map((id, index) => `${index === 0 ? "?" : "&"}seasonIds=${id}`)
    .join("");

  return `${apiUrl}/api/v2/historical-record/teamId/${teamId}${seasonIdsForQuery}`;
};

const fetchData = (url: string) =>
  fetch(url).then((response) => {
    if (response.ok) {
      return response.json();
    }

    throw new Error(`Oops something went wrong.`);
  });

export {
  fetchData,
  getTeamsUrl,
  getSeasonsUrl,
  getLeagueTableUrl,
  getCompetitionsUrl,
  getHistoricalPositionsUrl,
  getLeaguePositionsUrl
};
