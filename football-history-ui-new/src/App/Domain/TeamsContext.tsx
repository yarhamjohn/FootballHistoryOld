import { createContext, FC, ReactElement, ReactNode } from "react";
import { Team } from "./Types";

const TeamsContext = createContext<Team[]>([]);

type Props = { children?: ReactNode; teams: Team[] };

const TeamsContextProvider: FC<Props> = ({ children, teams }): ReactElement => {
  return <TeamsContext.Provider value={teams}>{children}</TeamsContext.Provider>;
};

export { TeamsContext, TeamsContextProvider };
