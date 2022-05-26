import { useEffect } from "react";
import { getSeasonsUrl } from "../Domain/Api";
import { Season } from "../Domain/Types";
import { useFetch } from "./useFetch";

const useFetchSeasons = () => {
  const { state, callApi } = useFetch<Season[]>();

  useEffect(() => {
    callApi(getSeasonsUrl());
  }, []);

  return { seasonsState: state };
};

export { useFetchSeasons };
