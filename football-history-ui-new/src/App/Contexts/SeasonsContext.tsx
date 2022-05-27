import { createContext, FC, ReactElement, ReactNode, useState } from "react";
import { Season } from "../Domain/Types";

type SeasonContextType = {
  seasons: Season[];
  activeSeason: Season;
  setActiveSeason: (season: Season) => void;
  firstSeason: Season;
  lastSeason: Season;
  seasonIds: number[];
};

const SeasonsContext = createContext<SeasonContextType>({} as SeasonContextType);

type Props = { children?: ReactNode; seasons: Season[] };

const SeasonsContextProvider: FC<Props> = ({ children, seasons }): ReactElement => {
  const [activeSeason, setActiveSeason] = useState<Season>(
    seasons.sort((a, b) => b.startYear - a.startYear)[0]
  );

  const oldestSeasonStartYear = Math.min(...seasons.map((x) => x.startYear));
  const newestSeasonStartYear = Math.max(...seasons.map((x) => x.startYear));

  const firstSeason = seasons.filter((s) => s.startYear === oldestSeasonStartYear)[0];
  const lastSeason = seasons.filter((s) => s.startYear === newestSeasonStartYear)[0];

  const seasonIds: number[] = seasons.map((s) => s.id);

  return (
    <SeasonsContext.Provider
      value={{ seasons, activeSeason, setActiveSeason, firstSeason, lastSeason, seasonIds }}
    >
      {children}
    </SeasonsContext.Provider>
  );
};

export { SeasonsContext, SeasonsContextProvider };
