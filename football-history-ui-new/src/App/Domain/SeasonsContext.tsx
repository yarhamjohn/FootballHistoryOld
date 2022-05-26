import { createContext, FC, ReactElement, ReactNode } from "react";
import { Season } from "./Types";

const SeasonsContext = createContext<Season[]>([]);

type Props = { children?: ReactNode; seasons: Season[] };

const SeasonsContextProvider: FC<Props> = ({ children, seasons }): ReactElement => {
  return <SeasonsContext.Provider value={seasons}>{children}</SeasonsContext.Provider>;
};

export { SeasonsContext, SeasonsContextProvider };
