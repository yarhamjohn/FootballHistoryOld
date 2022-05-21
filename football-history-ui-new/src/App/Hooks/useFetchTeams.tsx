import { useEffect } from "react";
import { getTeamsUrl } from "../Domain/Api";
import { Team } from "../Domain/Types";
import { useFetch } from "./useFetch";

const useFetchTeams = () => {
  const { state, callApi } = useFetch<Team[]>();

  useEffect(() => {
    callApi(getTeamsUrl());
  }, []);

  return { teamsState: state };
};

export { useFetchTeams };
