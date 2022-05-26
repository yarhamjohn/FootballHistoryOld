const apiUrl =
  process.env.NODE_ENV === "development"
    ? "https://localhost:5001"
    : "https://football-history-api.azurewebsites.net";

const getTeamsUrl = () => `${apiUrl}/api/v2/teams`;

const getHistoricalPositionsUrl = () => "";

export { getTeamsUrl, getHistoricalPositionsUrl };
