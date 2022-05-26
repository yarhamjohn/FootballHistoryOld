const apiUrl =
  process.env.NODE_ENV === "development"
    ? "https://localhost:5001"
    : "https://football-history-api.azurewebsites.net";

const getTeamsUrl = () => `${apiUrl}/api/v2/teams`;

const getSeasonsUrl = () => `${apiUrl}/api/v2/seasons`;

const getHistoricalPositionsUrl = () => "";

export { getTeamsUrl, getSeasonsUrl, getHistoricalPositionsUrl };
