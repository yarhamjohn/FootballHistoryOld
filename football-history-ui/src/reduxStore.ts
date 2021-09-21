import { configureStore } from "@reduxjs/toolkit";
import seasonReducer from "./App/seasonsSlice";
import competitionReducer from "./App/competitionsSlice";
import teamReducer from "./App/teamsSlice";

const reduxStore = configureStore({
  reducer: {
    season: seasonReducer,
    competition: competitionReducer,
    team: teamReducer,
  },
});

export type RootState = ReturnType<typeof reduxStore.getState>;
export type AppDispatch = typeof reduxStore.dispatch;

export default reduxStore;
