import { FunctionComponent } from "react";
import { Dropdown, DropdownItemProps } from "semantic-ui-react";
import { useAppDispatch, useAppSelector } from "../../../reduxHooks";
import { setSelectedTeam } from "../../selectionSlice";
import { Team, useGetAllTeamsQuery } from "../../teamsSlice";

const TeamFilter: FunctionComponent = () => {
  const dispatch = useAppDispatch();
  const teamState = useGetAllTeamsQuery();
  const selectedState = useAppSelector((state) => state.selected);

  function createDropdown(teams: Team[]): DropdownItemProps[] {
    return teams
      .map((c) => {
        return {
          key: c.id,
          text: c.name,
          value: c.id,
        };
      })
      .sort((a, b) => (a.text > b.text ? 1 : a.text < b.text ? -1 : 0));
  }

  function chooseTeam(teams: Team[], id: number | undefined) {
    const team = teams.filter((x) => x.id === id);

    if (team.length === 0) {
      return;
    }

    dispatch(setSelectedTeam(team[0]));
  }

  const body = teamState.isSuccess ? (
    <div
      style={{
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
      }}
    >
      {selectedState.selectedTeam === undefined ? (
        <p style={{ margin: "0 50px 0 0" }}>
          Select a team from the dropdown. The list contains all clubs to have featured in the
          Football League or Premier League since 1992.
        </p>
      ) : (
        <h1 style={{ margin: 0 }}>{selectedState.selectedTeam.name}</h1>
      )}
      <Dropdown
        placeholder={"Select Team"}
        text={selectedState.selectedTeam?.name ?? ""}
        clearable
        search
        selection
        options={createDropdown(teamState.data)}
        onChange={(_, data) =>
          chooseTeam(teamState.data, isNaN(Number(data.value)) ? undefined : Number(data.value))
        }
        style={{ maxHeight: "25px" }}
      />
    </div>
  ) : null;

  return body;
};

export { TeamFilter };
