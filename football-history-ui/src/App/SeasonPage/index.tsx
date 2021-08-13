import React, { FunctionComponent } from "react";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { Divider } from "semantic-ui-react";
import { CompetitionsInSeason } from "./CompetitionsInSeason";

const SeasonPage: FunctionComponent = () => {
  return (
    <>
      <SeasonFilter />
      <Divider />
      <h1>Records and comments</h1>
      <p>Here will go some stuff about the season</p>
      <Divider />
      <h1>Competitions</h1>
      <CompetitionsInSeason />
    </>
  );
};

export { SeasonPage };
