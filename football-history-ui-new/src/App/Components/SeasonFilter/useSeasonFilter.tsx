import { useContext, useEffect, useState } from "react";
import { Season } from "../../Domain/Types";
import { useSeasons } from "../../Hooks/useSeasons";
import { SeasonsContext } from "../../Contexts/SeasonsContext";

const useSeasonFilter = () => {
  const { changeSeason } = useSeasons();
  const { seasons, activeSeason, firstSeason, lastSeason } = useContext(SeasonsContext);

  const [next, setNext] = useState<Season | undefined>(undefined);
  const [previous, setPrevious] = useState<Season | undefined>(undefined);

  useEffect(() => {
    const next = seasons.filter((s) => s.startYear === activeSeason.startYear - 1);
    const previous = seasons.filter((s) => s.startYear === activeSeason.startYear + 1);

    setNext(next[0]);
    setPrevious(previous[0]);
  }, [activeSeason, seasons]);

  const moveNext = () => {
    if (next !== undefined) {
      changeSeason(next);
    }
  };

  const movePrevious = () => {
    if (previous !== undefined) {
      changeSeason(previous);
    }
  };

  const moveOldest = () => {
    changeSeason(firstSeason);
  };

  const moveNewest = () => {
    changeSeason(lastSeason);
  };

  return { next, previous, moveNext, movePrevious, moveOldest, moveNewest };
};

export { useSeasonFilter };
