import { FC, ReactElement } from "react";
import { Match } from "../../../../../Domain/Types";

type Props = { matches: Match[] };

const TestMatch: FC<Props> = ({ matches }): ReactElement => {
  return <div> Test Matches </div>;
};

export { TestMatch };
