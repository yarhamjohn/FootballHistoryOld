import { FunctionComponent } from "react";
import { Dropdown, DropdownItemProps } from "semantic-ui-react";
import { useAppDispatch, useAppSelector } from "../../../reduxHooks";
import { Competition, useGetAllCompetitionsQuery } from "../../competitionsSlice";
import { setSelectedCompetition } from "../../selectionSlice";

const CompetitionFilter: FunctionComponent = () => {
  const dispatch = useAppDispatch();
  const { selectedSeason, selectedCompetition } = useAppSelector((state) => state.selected);
  const competitionState = useGetAllCompetitionsQuery();

  function createDropdown(competitions: Competition[]): DropdownItemProps[] {
    return competitions
      .filter((x) => x.season.id === selectedSeason?.id)
      .map((c) => {
        return {
          key: c.id,
          text: `${c.name} (${c.level})`,
          value: c.id,
        };
      });
  }

  function chooseCompetition(competitions: Competition[], id: number | undefined) {
    const competition = competitions.filter((x) => x.id === id);

    if (competition.length === 0) {
      return;
    }

    dispatch(setSelectedCompetition(competition[0]));
  }

  const body = competitionState.isSuccess ? (
    <div
      style={{
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
      }}
    >
      {selectedCompetition === undefined ? (
        <p style={{ margin: "0 50px 0 0" }}>Select a competition from the dropdown.</p>
      ) : (
        <h1 style={{ margin: 0 }}>{selectedCompetition.name}</h1>
      )}
      <Dropdown
        placeholder="Select Division"
        text={selectedCompetition?.name ?? ""}
        clearable
        search
        selection
        options={createDropdown(competitionState.data)}
        onChange={(_, data) =>
          chooseCompetition(
            competitionState.data,
            isNaN(Number(data.value)) ? undefined : Number(data.value)
          )
        }
        style={{ maxHeight: "25px" }}
      />
    </div>
  ) : null;

  return body;
};

export { CompetitionFilter };
