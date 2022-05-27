import { createContext, FC, ReactElement, ReactNode, useState } from "react";
import { Team } from "../Domain/Types";

type TeamContextType = {
  teams: Team[];
  activeTeam: Team | null;
  setActiveTeam: (team: Team | null) => void;
};

const TeamsContext = createContext<TeamContextType>({} as TeamContextType);

type Props = { children?: ReactNode; teams: Team[] };

const TeamsContextProvider: FC<Props> = ({ children, teams }): ReactElement => {
  const [activeTeam, setActiveTeam] = useState<Team | null>(null);

  return (
    <TeamsContext.Provider value={{ teams, activeTeam, setActiveTeam }}>
      {children}
    </TeamsContext.Provider>
  );
};

export { TeamsContext, TeamsContextProvider };
