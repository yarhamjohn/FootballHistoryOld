import { useQuery } from "react-query";
import { fetchData, getCompetitionsUrl } from "../Domain/Api";
import { Competition } from "../Domain/Types";

const useFetchCompetitions = () =>
  useQuery<Competition[], Error>("competitions", () => fetchData(getCompetitionsUrl()));

export { useFetchCompetitions };
