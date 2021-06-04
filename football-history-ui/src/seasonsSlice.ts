import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { Season } from "./App/shared/useFetchSeasons";

type SeasonState = {
  status: "UNLOADED" | "LOADING" | "LOADED" | "LOAD_FAILED";
  seasons: Season[];
  selectedSeason: Season | undefined;
  error: string | undefined;
};

const initialState: SeasonState = {
  status: "UNLOADED",
  seasons: [],
  selectedSeason: undefined,
  error: undefined,
};

export const fetchSeasons = createAsyncThunk("seasons/fetchSeasons", async () => {
  const response = await fetch("https://localhost:5001/api/v2/seasons");
  return (await response.json()).result as Season[];
});

export const seasonSlice = createSlice({
  name: "seasons",
  initialState,
  reducers: {
    selectSeason: (state, action) => {
      state.selectedSeason = action.payload;
    },
    clearSelectedSeason: (state) => {
      state.selectedSeason = undefined;
    },
  },
  extraReducers: (builder) => {
    builder.addCase(fetchSeasons.pending, (state, _) => {
      state.status = "LOADING";
    });
    builder.addCase(fetchSeasons.fulfilled, (state, action) => {
      state.status = "LOADED";
      state.seasons = action.payload;
    });
    builder.addCase(fetchSeasons.rejected, (state, action) => {
      state.status = "LOAD_FAILED";
      state.error = action.error.message;
    });
  },
});

export const { selectSeason, clearSelectedSeason } = seasonSlice.actions;

export default seasonSlice.reducer;
