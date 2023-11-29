import { configureStore } from "@reduxjs/toolkit";
import { competitionsApi } from "./App/competitionsSlice";
import { seasonsApi } from "./App/seasonsSlice";
import { teamsApi } from "./App/teamsSlice";
import selectionReducer from "./App/selectionSlice";

const reduxStore = configureStore({
  reducer: {
    selected: selectionReducer,
    [seasonsApi.reducerPath]: seasonsApi.reducer,
    [teamsApi.reducerPath]: teamsApi.reducer,
    [competitionsApi.reducerPath]: competitionsApi.reducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware()
      .concat(seasonsApi.middleware)
      .concat(teamsApi.middleware)
      .concat(competitionsApi.middleware),
});

export type RootState = ReturnType<typeof reduxStore.getState>;
export type AppDispatch = typeof reduxStore.dispatch;

export default reduxStore;
