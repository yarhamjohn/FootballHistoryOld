import { createContext, FC, ReactElement, ReactNode, useEffect, useState } from "react";
import { Season } from "../Domain/Types";

type SeasonContextType = {
  seasons: Season[];
  activeSeason: Season;
  setActiveSeason: (season: Season) => void;
  firstSeason: Season;
  lastSeason: Season;
  seasonIds: number[];
  next: Season | undefined;
  previous: Season | undefined;
  moveNext: () => void;
  movePrevious: () => void;
  moveOldest: () => void;
  moveNewest: () => void;
};

const SeasonsContext = createContext<SeasonContextType>({} as SeasonContextType);

type Props = { children?: ReactNode; seasons: Season[] };

const SeasonsContextProvider: FC<Props> = ({ children, seasons }): ReactElement => {
  const [activeSeason, setActiveSeason] = useState<Season>(
    seasons.sort((a, b) => b.startYear - a.startYear)[0]
  );

  const [next, setNext] = useState<Season | undefined>(undefined);
  const [previous, setPrevious] = useState<Season | undefined>(undefined);

  useEffect(() => {
    const newNext = seasons.filter((s) => s.startYear === activeSeason.startYear - 1)[0];
    const newPrevious = seasons.filter((s) => s.startYear === activeSeason.startYear + 1)[0];

    setNext(newNext);
    setPrevious(newPrevious);
  }, [activeSeason, seasons]);

  const moveNext = () => {
    next && setActiveSeason(next);
  };

  const movePrevious = () => {
    previous && setActiveSeason(previous);
  };

  const moveOldest = () => {
    setActiveSeason(firstSeason);
  };

  const moveNewest = () => {
    setActiveSeason(lastSeason);
  };

  const oldestSeasonStartYear = Math.min(...seasons.map((x) => x.startYear));
  const newestSeasonStartYear = Math.max(...seasons.map((x) => x.startYear));

  const firstSeason = seasons.filter((s) => s.startYear === oldestSeasonStartYear)[0];
  const lastSeason = seasons.filter((s) => s.startYear === newestSeasonStartYear)[0];

  const seasonIds: number[] = seasons.map((s) => s.id);

  return (
    <SeasonsContext.Provider
      value={{
        seasons,
        activeSeason,
        setActiveSeason,
        firstSeason,
        lastSeason,
        seasonIds,
        next,
        previous,
        moveNext,
        movePrevious,
        moveOldest,
        moveNewest
      }}
    >
      {children}
    </SeasonsContext.Provider>
  );
};

export { SeasonsContext, SeasonsContextProvider };
