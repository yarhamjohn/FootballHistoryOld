import { useQuery } from "react-query";
import { fetchData, getTeamsUrl } from "../Domain/Api";
import { Team } from "../Domain/Types";

const useFetchTeams = () => useQuery<Team[], Error>("teams", () => fetchData(getTeamsUrl()));

export { useFetchTeams };
