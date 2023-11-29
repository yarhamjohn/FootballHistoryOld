import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { api } from "./shared/useApi";
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

export const competitionsApi = createApi({
  reducerPath: "competitionsApi",
  baseQuery: fetchBaseQuery({ baseUrl: `${api}/api/v2/competitions` }),
  endpoints: (builder) => ({
    getAllCompetitions: builder.query<Competition[], void>({
      query: () => "",
    }),
  }),
});

export const { useGetAllCompetitionsQuery } = competitionsApi;
