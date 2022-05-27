type Team = {
  id: number;
  name: string;
  abbreviation: string;
  notes: string | null;
};

type Season = {
  id: number;
  startYear: number;
  endYear: number;
};

type CompetitionRules = {
  pointsForWin: number;
  totalPlaces: number;
  promotionPlaces: number;
  relegationPlaces: number;
  playOffPlaces: number;
  relegationPlayOffPlaces: number;
  reElectionPlaces: number;
  failedReElectionPosition: number | null;
};

type Competition = {
  id: number;
  name: string;
  season: Season;
  level: string;
  comment: string | null;
  rules: CompetitionRules;
};

type HistoricalRecord = {
  teamId: number;
  historicalSeasons: HistoricalSeason[];
};

type HistoricalSeason = {
  seasonId: number;
  seasonStartYear: number;
  boundaries: number[];
  historicalPosition: HistoricalPosition | null;
};

type HistoricalPosition = {
  competitionId: number;
  competitionName: string;
  position: number;
  overallPosition: number;
  status: string | null;
};

export { Team, Season, Competition, HistoricalRecord, HistoricalSeason };
