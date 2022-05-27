import { useEffect } from "react";
import { getCompetitionsUrl } from "../Domain/Api";
import { Competition } from "../Domain/Types";
import { useFetch } from "./useFetch";

const useFetchCompetitions = () => {
  const { state, callApi } = useFetch<Competition[]>();

  useEffect(() => {
    callApi(getCompetitionsUrl());
  }, []);

  return { competitionsState: state };
};

export { useFetchCompetitions };
