import React, { FunctionComponent, useEffect } from "react";
import { Dropdown, DropdownItemProps } from "semantic-ui-react";
import { useAppDispatch, useAppSelector } from "../../../reduxHooks";
import { selectCompetitionsBySeasonId, setSelectedCompetition } from "../../competitionsSlice";

const CompetitionFilter: FunctionComponent = () => {
  const dispatch = useAppDispatch();
  const seasonState = useAppSelector((state) => state.season);
  const competitionState = useAppSelector((state) => state.competition);

  useEffect(() => {
    if (seasonState.selectedSeason !== undefined) {
      const competitions = selectCompetitionsBySeasonId(
        competitionState,
        seasonState.selectedSeason.id
      );
      dispatch(setSelectedCompetition(competitions[0]));
    }
  }, [seasonState.selectedSeason, dispatch]);

  if (competitionState.status !== "LOADED") {
    return null;
  }

  function createDropdown(): DropdownItemProps[] {
    if (competitionState.status !== "LOADED") {
      return [];
    }

    return selectCompetitionsBySeasonId(competitionState, seasonState.selectedSeason?.id).map(
      (c) => {
        return {
          key: c.id,
          text: `${c.name} (${c.level})`,
          value: c.id,
        };
      }
    );
  }

  function chooseCompetition(id: number | undefined) {
    if (competitionState.status !== "LOADED") {
      return [];
    }

    const competition = competitionState.competitions.filter((x) => x.id === id)[0];
    dispatch(setSelectedCompetition(competition));
  }

  return (
    <div
      style={{
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
      }}
    >
      {competitionState.selectedCompetition === undefined ? (
        <p style={{ margin: "0 50px 0 0" }}>Select a competition from the dropdown.</p>
      ) : (
        <h1 style={{ margin: 0 }}>{competitionState.selectedCompetition.name}</h1>
      )}
      <Dropdown
        placeholder="Select Division"
        clearable
        search
        selection
        options={createDropdown()}
        onChange={(_, data) =>
          chooseCompetition(isNaN(Number(data.value)) ? undefined : Number(data.value))
        }
        style={{ maxHeight: "25px" }}
      />
    </div>
  );
};

export { CompetitionFilter };
