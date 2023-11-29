import { Dropdown, DropdownItemProps, Icon } from "semantic-ui-react";
import { FunctionComponent } from "react";
import { useAppDispatch, useAppSelector } from "../../../reduxHooks";
import { Season, useGetAllSeasonsQuery } from "../../seasonsSlice";
import { setSelectedSeason } from "../../selectionSlice";
import { useGetAllCompetitionsQuery } from "../../competitionsSlice";

const SeasonFilter: FunctionComponent = () => {
  const dispatch = useAppDispatch();
  const seasonState = useGetAllSeasonsQuery();
  const competitionState = useGetAllCompetitionsQuery();
  const selectedState = useAppSelector((state) => state.selected);

  function createDropdown(seasons: Season[]): DropdownItemProps[] {
    return seasons
      .slice()
      .sort((a, b) => b.startYear - a.startYear)
      .map((s) => {
        return {
          key: s.startYear,
          text: `${s.startYear} - ${s.endYear}`,
          value: s.startYear,
        };
      });
  }

  const changeSeason = (seasons: Season[], nextSeason: Season) => {
    if (seasons.some((s) => s.startYear === nextSeason.startYear)) {
      dispatch(setSelectedSeason({ season: nextSeason, competitions: competitionState.data! }));
    } else {
      return;
    }
  };

  const forwardOneSeason = (seasons: Season[]) => {
    if (selectedState.selectedSeason !== undefined) {
      const nextStartYear = selectedState.selectedSeason.startYear + 1;
      const nextSeason = seasons.filter((x) => x.startYear === nextStartYear);

      if (nextSeason.length === 1) {
        changeSeason(seasons, nextSeason[0]);
      }
    }
  };

  const backOneSeason = (seasons: Season[]) => {
    if (selectedState.selectedSeason !== undefined) {
      const previousStartYear = selectedState.selectedSeason.startYear - 1;
      const previousSeason = seasons.filter((x) => x.startYear === previousStartYear);

      if (previousSeason.length === 1) {
        changeSeason(seasons, previousSeason[0]);
      }
    }
  };

  function chooseSeason(seasons: Season[], startYear: number | undefined) {
    const season = seasons.filter((x) => x.startYear === startYear)[0];
    dispatch(setSelectedSeason({ season: season, competitions: competitionState.data! }));
  }

  const body = seasonState.isSuccess ? (
    <div style={{ display: "flex", alignItems: "center", color: "#00B5AD" }}>
      <Icon
        name="caret left"
        size="huge"
        onClick={() => backOneSeason(seasonState.data)}
        style={{ cursor: "pointer" }}
      />
      <Dropdown
        placeholder="Select Season"
        fluid
        selection
        options={createDropdown(seasonState.data)}
        onChange={(_, data) =>
          chooseSeason(seasonState.data, isNaN(Number(data.value)) ? undefined : Number(data.value))
        }
        value={selectedState.selectedSeason?.startYear}
        style={{ gridArea: "filter" }}
      />
      <Icon
        name="caret right"
        size="huge"
        onClick={() => forwardOneSeason(seasonState.data)}
        style={{ cursor: "pointer" }}
      />
    </div>
  ) : null;

  return body;
};

export { SeasonFilter };
