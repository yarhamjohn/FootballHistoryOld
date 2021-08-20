import React, { FunctionComponent, useState } from "react";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { Divider } from "semantic-ui-react";
import { CompetitionsInSeason } from "./CompetitionsInSeason";
import { useAppSelector } from "../../reduxHooks";
import { ErrorMessage } from "../components/ErrorMessage";
import { Competition, selectCompetitionsBySeasonId } from "../competitionsSlice";
import { useEffect } from "react";

const SeasonPage: FunctionComponent = () => {
  const selectedSeason = useAppSelector((state) => state.season.selectedSeason);
  const competitionsState = useAppSelector((state) => state.competition);
  const [competitionsInSeason, setCompetitionsInSeason] = useState<Competition[]>(
    selectCompetitionsBySeasonId(competitionsState, selectedSeason?.id)
  );

  useEffect(() => {
    setCompetitionsInSeason(selectCompetitionsBySeasonId(competitionsState, selectedSeason?.id));
  }, [competitionsState, selectedSeason]);

  if (selectedSeason === undefined) {
    return (
      <>
        <ErrorMessage
          header="Something went wrong"
          content="No season was selected. Please select a season."
        />
        <SeasonFilter />
      </>
    );
  }

  const getCompetitionNames = () => {
    switch (competitionsInSeason.length) {
      case 0:
        return "";
      case 1:
        return " ".concat(competitionsInSeason.map((c) => c.name).join(", "));
      default:
        const names = competitionsInSeason.map((c) => c.name);
        let result = " ";
        for (let i = 0; i < names.length; i++) {
          if (i === names.length - 1) {
            result += " and";
          } else if (i !== 0) {
            result += ",";
          }

          result += ` ${names[i]}`;
        }

        return result;
    }
  };

  return (
    <>
      <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
        <h1 style={{ margin: "0" }}>
          {selectedSeason.startYear} - {selectedSeason.endYear}
        </h1>
        )
        <SeasonFilter />
      </div>
      <Divider />
      <h1>Information</h1>
      <p>
        There were {competitionsInSeason.length} active competition
        {competitionsInSeason.length === 1 ? "" : "s"} during the {selectedSeason.startYear} -{" "}
        {selectedSeason.endYear} season:
        <span style={{ fontWeight: "bold" }}>{getCompetitionNames()}</span>, involving a total of{" "}
        {competitionsInSeason
          .map((c) => c.rules.totalPlaces)
          .reduce((sum, current) => sum + current, 0)}{" "}
        different teams. There were {competitionsInSeason[0].rules.pointsForWin} points for a win.
      </p>
      {/* assumes the same number of points for a win for each competition */}
      <Divider />
      <h1>Competitions</h1>
      <CompetitionsInSeason />
    </>
  );
};

export { SeasonPage };
