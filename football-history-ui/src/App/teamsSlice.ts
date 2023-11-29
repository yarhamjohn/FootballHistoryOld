import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { api } from "./shared/useApi";

export type Team = {
  id: number;
  name: string;
  abbreviation: string;
  notes: string | null;
};

export const teamsApi = createApi({
  reducerPath: "teamsApi",
  baseQuery: fetchBaseQuery({ baseUrl: `${api}/api/v2/teams` }),
  endpoints: (builder) => ({
    getAllTeams: builder.query<Team[], void>({
      query: () => "",
    }),
  }),
});

export const { useGetAllTeamsQuery } = teamsApi;
