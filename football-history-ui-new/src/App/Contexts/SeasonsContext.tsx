import { createContext, FC, ReactElement, ReactNode, useState } from "react";
import { Season } from "../Domain/Types";

type SeasonContextType = {
  seasons: Season[];
  activeSeason: Season;
  setActiveSeason: (season: Season) => void;
};

const SeasonsContext = createContext<SeasonContextType>({} as SeasonContextType);

type Props = { children?: ReactNode; seasons: Season[] };

const SeasonsContextProvider: FC<Props> = ({ children, seasons }): ReactElement => {
  const [activeSeason, setActiveSeason] = useState<Season>(
    seasons.sort((a, b) => b.startYear - a.startYear)[0]
  );

  return (
    <SeasonsContext.Provider value={{ seasons, activeSeason, setActiveSeason }}>
      {children}
    </SeasonsContext.Provider>
  );
};

export { SeasonsContext, SeasonsContextProvider };
