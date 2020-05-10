import React, { FunctionComponent } from "react";
import { Dropdown, DropdownItemProps } from "semantic-ui-react";
import { Club, useClubs } from "../../hooks/useClubs";
import { isString } from "../../shared/functions";

const ClubFilter: FunctionComponent<{
  selectedClub: Club | undefined;
  setSelectedClub: (selectedClub: Club | undefined) => void;
}> = ({ selectedClub, setSelectedClub }) => {
  const { clubsState } = useClubs();

  function GetDropdownClubs(clubs: Club[]): DropdownItemProps[] {
    return clubs.map((c) => {
      return {
        key: c.name,
        text: c.name,
        value: c.name,
      };
    });
  }

  const selectClub = (selection: any) => {
    if (clubsState.type !== "CLUBS_LOADED") {
      return;
    }

    if (isString(selection)) {
      if (selection === "") {
        setSelectedClub(undefined);
        return;
      }

      const club = clubsState.clubs.filter((c) => c.name === selection);
      if (club.length !== 1) {
        throw new Error(
          `Clubs should be unique but there were ${
            club.length
          } clubs that matches the selection ${club.join(",")}.`
        );
      }
      setSelectedClub(club[0]);
    } else {
      throw new Error(
        `An unexpected error occurred. The selection (${selection.toString()}) could not be processed.`
      );
    }
  };

  if (clubsState.type !== "CLUBS_LOADED") {
    return null;
  }

  return (
    <div
      style={{
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
      }}
    >
      {selectedClub === undefined ? (
        <p style={{ margin: "0 50px 0 0" }}>
          Select a club from the dropdown. The list contains all clubs to have featured in the
          Football League or Premier League since 1992.
        </p>
      ) : (
        <h1 style={{ margin: 0 }}>{selectedClub.name}</h1>
      )}
      <Dropdown
        placeholder="Select Club"
        clearable
        search
        selection
        options={GetDropdownClubs(clubsState.clubs)}
        onChange={(_, data) => selectClub(data.value)}
        style={{ maxHeight: "25px" }}
      />
    </div>
  );
};

export { ClubFilter };
