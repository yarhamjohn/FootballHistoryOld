import { useContext, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { SeasonsContext } from "../Contexts/SeasonsContext";
import { Season } from "../Domain/Types";

const useSeasons = () => {
  const navigate = useNavigate();
  const { seasons, activeSeason, setActiveSeason } = useContext(SeasonsContext);
  const { season } = useParams();

  useEffect(() => {
    if (season === undefined) {
      navigate(`/seasons/${activeSeason?.startYear}-${activeSeason?.endYear}`);
      return;
    }

    const startAndEnd = season.split("-");
    if (startAndEnd.length !== 2) {
      navigate("/not-found");
    }

    if (Number.isNaN(startAndEnd[0]) || Number.isNaN(startAndEnd[1])) {
      navigate("/not-found");
    }

    if (Number(startAndEnd[0]) + 1 !== Number(startAndEnd[1])) {
      navigate("/not-found");
    }

    const urlMatchesSeason = seasons.filter((s) => s.startYear === Number(startAndEnd[0]));

    if (urlMatchesSeason.length === 1) {
      setActiveSeason(urlMatchesSeason[0]);
    } else {
      navigate("/not-found");
    }
  }, [season]);

  const changeSeason = (newSeason: Season) => {
    navigate(`/seasons/${newSeason?.startYear}-${newSeason?.endYear}`);
    setActiveSeason(newSeason);
  };

  return { changeSeason };
};

export { useSeasons };
