import { Dropdown, DropdownItemProps, Icon } from "semantic-ui-react";
import React, { FunctionComponent, useEffect } from "react";
import { Season, useSeasons } from "./useSeasons";
import { isNumber } from "../shared/functions";

const SeasonFilter: FunctionComponent<{
  seasons: Season[];
  selectedSeason: number | undefined;
  setSelectedSeason: (startYear: number) => void;
  style: React.CSSProperties;
}> = ({ seasons, selectedSeason, setSelectedSeason, style }) => {
  function GetDropdownSeasons(seasons: Season[]): DropdownItemProps[] {
    return seasons.map((s) => {
      return {
        key: s.startYear,
        text: `${s.startYear} - ${s.endYear}`,
        value: s.startYear,
      };
    });
  }

  const selectSeasonStartYear = (selection: any) => {
    if (isNumber(selection)) {
      setSelectedSeason(selection);
    } else {
      throw new Error("An unexpected error occurred. The selection could not be processed.");
    }
  };

  return (
    <div style={{ ...style, display: "flex", alignItems: "center", color: "#00B5AD" }}>
      <Icon name="caret left" size="huge" />
      <Dropdown
        placeholder="Select Season"
        fluid
        selection
        options={GetDropdownSeasons(seasons)}
        onChange={(_, data) => selectSeasonStartYear(data.value)}
        value={selectedSeason}
        style={{ gridArea: "filter" }}
      />
      <Icon name="caret right" size="huge" />
    </div>
  );
};

export { SeasonFilter };
