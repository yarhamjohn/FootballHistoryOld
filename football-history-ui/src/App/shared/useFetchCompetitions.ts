import { useApi } from "./useApi";
import { useFetch } from "./useFetch";
import { Season } from "./seasonsSlice";

export type CompetitionRules = {
  pointsForWin: number;
  totalPlaces: number;
  promotionPlaces: number;
  relegationPlaces: number;
  playOffPlaces: number;
  relegationPlayOffPlaces: number;
  reElectionPlaces: number;
  failedReElectionPosition: number | null;
};

export type Competition = {
  id: number;
  name: string;
  season: Season;
  level: string;
  comment: string | null;
  rules: CompetitionRules;
};
