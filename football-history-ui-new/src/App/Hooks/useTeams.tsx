import { useContext, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { TeamsContext } from "../Contexts/TeamsContext";

const useTeams = () => {
  const navigate = useNavigate();
  const { teams, activeTeam, setActiveTeam } = useContext(TeamsContext);
  const { teamName } = useParams();

  useEffect(() => {
    if (teamName === undefined) return;

    const urlMatchesTeam = teams.filter(
      (t) => t.name.toLowerCase() === teamName.replace("-", " ").toLowerCase()
    );

    if (urlMatchesTeam.length === 1) {
      setActiveTeam(urlMatchesTeam[0]);
    } else {
      navigate("not-found");
    }
  }, [teamName]);

  useEffect(() => {
    navigate(`/teams/${activeTeam?.name.replace(" ", "-").toLowerCase() ?? ""}`);
  }, [activeTeam]);

  return { teams, activeTeam, setActiveTeam };
};

export { useTeams };
