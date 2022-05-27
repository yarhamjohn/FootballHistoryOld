import { createContext, FC, ReactElement, ReactNode, useContext, useEffect, useState } from "react";
import { Competition } from "../Domain/Types";
import { SeasonsContext } from "./SeasonsContext";

type CompetitionContextType = {
  competitions: Competition[];
  activeCompetition: Competition | null;
  setActiveCompetition: (competition: Competition | null) => void;
};
const CompetitionsContext = createContext<CompetitionContextType>({} as CompetitionContextType);

type Props = { children?: ReactNode; competitions: Competition[] };

const CompetitionsContextProvider: FC<Props> = ({ children, competitions }): ReactElement => {
  const { activeSeason } = useContext(SeasonsContext);
  const [activeCompetition, setActiveCompetition] = useState<Competition | null>(null);

  useEffect(() => {
    const seasonCompetitions = competitions.filter((c) => c.season.id === activeSeason.id);
    const nextCompetition = seasonCompetitions.filter((c) => c.level === activeCompetition?.level);

    if (nextCompetition.length === 0) {
      setActiveCompetition(null);
    } else {
      setActiveCompetition(nextCompetition[0]);
    }
  }, [activeSeason]);

  return (
    <CompetitionsContext.Provider value={{ competitions, activeCompetition, setActiveCompetition }}>
      {children}
    </CompetitionsContext.Provider>
  );
};

export { CompetitionsContext, CompetitionsContextProvider };
