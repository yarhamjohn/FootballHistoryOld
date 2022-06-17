import { useContext, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { TeamsContext } from "../Contexts/TeamsContext";
import { Team } from "../Domain/Types";

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

  const changeTeam = (newTeam: Team | null) => {
    navigate(`/teams/${newTeam?.name.replace(" ", "-").toLowerCase() ?? ""}`);
    setActiveTeam(newTeam);
  };

  return { teams, activeTeam, changeTeam };
};

export { useTeams };
