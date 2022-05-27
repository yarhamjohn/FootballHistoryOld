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

type HistoricalPosition = undefined;

export { Team, Season, Competition, HistoricalPosition };
