import { createAsyncThunk, createSelector, createSlice, PayloadAction } from "@reduxjs/toolkit";
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

type CompetitionState = {
  status: "UNLOADED" | "LOADING" | "LOADED" | "LOAD_FAILED";
  competitions: Competition[];
  selectedCompetition: Competition | undefined;
  error: string | undefined;
};

const initialState: CompetitionState = {
  status: "UNLOADED",
  competitions: [],
  selectedCompetition: undefined,
  error: undefined,
};

export const fetchCompetitions = createAsyncThunk("competitions/fetchAll", async () => {
  const response = await fetch(`${api}/api/v2/competitions`);
  return (await response.json()).result as Competition[];
});

export const competitionsSlice = createSlice({
  name: "competitions",
  initialState,
  reducers: {
    setSelectedCompetition: (state, action: PayloadAction<Competition>) => {
      state.selectedCompetition = action.payload;
    },
    clearSelectedCompetition: (state) => {
      state.selectedCompetition = undefined;
    },
  },
  extraReducers: (builder) => {
    builder.addCase(fetchCompetitions.pending, (state, _) => {
      state.status = "LOADING";
    });
    builder.addCase(fetchCompetitions.fulfilled, (state, action) => {
      state.status = "LOADED";
      state.competitions = action.payload;
    });
    builder.addCase(fetchCompetitions.rejected, (state, action) => {
      state.status = "LOAD_FAILED";
      state.error = action.error.message;
    });
  },
});

const selectCompetitions = (state: CompetitionState) => state.competitions;
const selectSeasonId = (_, seasonId: number) => seasonId;
export const selectCompetitionsBySeasonId = createSelector(
  [selectCompetitions, selectSeasonId],
  (competitions, seasonId) => competitions.filter((x) => x.season.id === seasonId)
);

export const { setSelectedCompetition, clearSelectedCompetition } = competitionsSlice.actions;

export default competitionsSlice.reducer;
