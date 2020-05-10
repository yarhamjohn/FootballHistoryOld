import { ApplicationAction } from "../../index";

export type LeagueTableState = {
  type: "NO_LEAGUE_TABLE_LOADED";
} |{
  type: "LOADING_LEAGUE_TABLE";
} | {
  type: "LEAGUE_TABLE_LOADED";
};

export type LeagueTableAction = {
  type: "LEAGUE_TABLE_LOAD_STARTED";
} | {
  type: "LEAGUE_TABLE_LOAD_COMPLETED";
};

const leagueTableReducer = (
  prevState: LeagueTableState | undefined,
  action: ApplicationAction
): LeagueTableState => {
  if (!prevState) {
    return { type: "NO_LEAGUE_TABLE_LOADED" };
  }

  switch (action.type) {
    case "LEAGUE_TABLE_LOAD_STARTED":
      return { type: "LOADING_LEAGUE_TABLE"};
    case "LEAGUE_TABLE_LOAD_COMPLETED":
      return { type: "LEAGUE_TABLE_LOADED"}
  }
};

export { leagueTableReducer};
