import { useQuery } from "react-query";
import { fetchData, getSeasonsUrl } from "../Domain/Api";
import { Season } from "../Domain/Types";

const useFetchSeasons = () =>
  useQuery<Season[], Error>("seasons", () => fetchData(getSeasonsUrl()));

export { useFetchSeasons };
