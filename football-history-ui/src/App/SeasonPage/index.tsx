import React, { FunctionComponent } from "react";
import { useAppSelector } from "../../reduxHooks";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { Divider } from "semantic-ui-react";

const SeasonPage: FunctionComponent = () => {
  return (
    <>
      <SeasonFilter />
      <Divider />
      <h1>Records and comments</h1>
      <p>Here will go some stuff about the season</p>
      <Divider />
      <h1>Leagues</h1>
      <p>here will go some tabs for each competition</p>
    </>
  );
};

export { SeasonPage };
