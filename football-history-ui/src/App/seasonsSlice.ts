import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { api } from "./shared/useApi";

export type Season = {
  id: number;
  startYear: number;
  endYear: number;
};

export const seasonsApi = createApi({
  reducerPath: "seasonsApi",
  baseQuery: fetchBaseQuery({ baseUrl: `${api}/api/v2/seasons` }),
  endpoints: (builder) => ({
    getAllSeasons: builder.query<Season[], void>({
      query: () => "",
    }),
  }),
});

export const { useGetAllSeasonsQuery } = seasonsApi;
