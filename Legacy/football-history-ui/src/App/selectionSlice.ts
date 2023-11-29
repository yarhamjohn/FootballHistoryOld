import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { Season, seasonsApi } from "./seasonsSlice";
import { Competition, competitionsApi } from "./competitionsSlice";
import { Team, teamsApi } from "./teamsSlice";

type SelectionState = {
  selectedSeason: Season | undefined;
  selectedCompetition: Competition | undefined;
  selectedTeam: Team | undefined;
};

const slice = createSlice({
  name: "selections",
  initialState: {
    selectedSeason: undefined,
    selectedCompetition: undefined,
    selectedTeam: undefined,
  } as SelectionState,
  reducers: {
    setSelectedSeason: (
      state,
      action: PayloadAction<{ season: Season; competitions: Competition[] }>
    ) => {
      state.selectedSeason = action.payload.season;

      const competitionsInSeason = action.payload.competitions.filter(
        (x) => x.season.id === action.payload.season.id
      );

      const competitionAtMatchingLevel = competitionsInSeason.filter(
        (x) => x.level === state.selectedCompetition?.level
      );

      state.selectedCompetition =
        competitionAtMatchingLevel.length === 1
          ? competitionAtMatchingLevel[0]
          : competitionsInSeason[0];
    },
    clearSelectedSeason: (state) => {
      state.selectedSeason = undefined;
    },
    setSelectedCompetition: (state, action: PayloadAction<Competition>) => {
      state.selectedSeason = action.payload.season;
      state.selectedCompetition = action.payload;
    },
    clearSelectedCompetition: (state) => {
      state.selectedCompetition = undefined;
    },
    setSelectedTeam: (state, action: PayloadAction<Team>) => {
      state.selectedTeam = action.payload;
    },
    clearSelectedTeam: (state) => {
      state.selectedTeam = undefined;
    },
  },
  extraReducers: (builder) => {
    builder.addMatcher(seasonsApi.endpoints.getAllSeasons.matchFulfilled, (state, { payload }) => {
      if (payload.length > 0) {
        state.selectedSeason = payload.reduce((prev, current) =>
          prev.startYear > current.startYear ? prev : current
        );
      }
    });
    builder.addMatcher(teamsApi.endpoints.getAllTeams.matchFulfilled, (state, { payload }) => {
      if (payload.length > 0) {
        state.selectedTeam = payload[0];
      }
    });
    builder.addMatcher(
      competitionsApi.endpoints.getAllCompetitions.matchFulfilled,
      (state, { payload }) => {
        if (payload.length > 0 && state.selectedSeason !== undefined) {
          const selectedCompetition = payload.filter(
            (x) => x.season.id === state.selectedSeason!.id && x.level === "1"
          );

          if (selectedCompetition.length === 1) {
            state.selectedCompetition = selectedCompetition[0];
          }
        }
      }
    );
  },
});

export const {
  setSelectedSeason,
  setSelectedCompetition,
  setSelectedTeam,
  clearSelectedSeason,
  clearSelectedCompetition,
  clearSelectedTeam,
} = slice.actions;

export const selectCurrentSeason = (state: SelectionState) => state.selectedSeason;
export const selectCurrentCompetition = (state: SelectionState) => state.selectedCompetition;
export const selectCurrentTeam = (state: SelectionState) => state.selectedTeam;

export default slice.reducer;
