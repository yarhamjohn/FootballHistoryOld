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

type Row = {
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

type League = {
  table: Row[];
  competition: Competition;
};

type LeaguePosition = {
  date: Date;
  position: number;
};

type MatchCompetition = {
  id: number;
  name: string;
  startYear: number;
  endYear: number;
  level: string;
};

type MatchRules = {
  type: string;
  stage: string;
  extraTime: boolean;
  penalties: boolean;
  numLegs: number;
  awayGoals: boolean;
  replays: boolean;
};

type MatchTeam = {
  id: number;
  name: string;
  abbreviation: string;
  goals: number;
  goalsExtraTime: number;
  penaltiesTaken: number;
  penaltiesScored: number;
};

type Match = {
  id: number;
  matchDate: Date;
  competition: MatchCompetition;
  rules: MatchRules;
  homeTeam: MatchTeam;
  awayTeam: MatchTeam;
};

type Size = "small" | "large";

type Outcome = "W" | "D" | "L";

type Form = { outcome: Outcome; color: string; title: string }[];

export {
  Team,
  Season,
  Competition,
  HistoricalRecord,
  HistoricalSeason,
  League,
  Row,
  Size,
  LeaguePosition,
  CompetitionRules,
  Match,
  Form,
  Outcome
};
