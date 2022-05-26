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

type HistoricalPosition = undefined;

export { Team, Season, HistoricalPosition };
