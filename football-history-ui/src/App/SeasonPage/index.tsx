import { FunctionComponent, useState } from "react";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { Divider, Icon, Label, Message } from "semantic-ui-react";
import { CompetitionsInSeason } from "./CompetitionsInSeason";
import { useAppSelector } from "../../reduxHooks";
import { ErrorMessage } from "../components/ErrorMessage";
import { Competition, useGetAllCompetitionsQuery } from "../competitionsSlice";
import { useEffect } from "react";
import { CombinedStatistics } from "./CombinedStatistics";
import { useFetchPositions } from "../shared/useFetchPositions";

const SeasonPage: FunctionComponent = () => {
  const selectedSeason = useAppSelector((state) => state.selected.selectedSeason);
  const competitionsState = useAppSelector((state) => state.selected.selectedCompetition);
  const positions = useFetchPositions(selectedSeason?.id);

  const { data } = useGetAllCompetitionsQuery(undefined, {
    selectFromResult: ({ data }) => ({
      data: data === undefined ? [] : data.filter((x) => x.season.id === selectedSeason?.id),
    }),
  });

  const [competitionsInSeason, setCompetitionsInSeason] = useState<Competition[]>(data);

  useEffect(() => {
    setCompetitionsInSeason(data);
  }, [competitionsState, selectedSeason, data]);

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
        <SeasonFilter />
      </div>

      {competitionsInSeason.length === 0 ? (
        <Message
          header="There were no active competitions during this season."
          content={"This was due to World War II."}
        />
      ) : (
        <>
          <Divider />

          <div style={{ marginBottom: "2rem" }}>
            <h1>Information</h1>
            <p>
              There were {competitionsInSeason.length} active competition
              {competitionsInSeason.length === 1 ? "" : "s"} during the {selectedSeason.startYear} -{" "}
              {selectedSeason.endYear} season:
              <span style={{ fontWeight: "bold" }}>{getCompetitionNames()}</span>, involving a total
              of{" "}
              {competitionsInSeason
                .map((c) => c.rules.totalPlaces)
                .reduce((sum, current) => sum + current, 0)}{" "}
              different teams. There were {competitionsInSeason[0].rules.pointsForWin} points for a
              win.
            </p>
          </div>
          <div style={{ display: "flex", flexWrap: "wrap", gap: "1rem" }}>
            <div style={{ display: "flex", flexDirection: "column", flex: "1 1 0" }}>
              <CombinedStatistics seasonId={selectedSeason.id} />
            </div>
            <div style={{ display: "flex", flexDirection: "column", flex: "4 1 0" }}>
              <div style={{ display: "flex", flexDirection: "row" }}>
                <Label.Group color="yellow">
                  {positions.status === "LOAD_SUCCESSFUL" &&
                    positions.data
                      .filter((x) => x.leaguePosition === 1)
                      .map((p) => (
                        <Label key={p.teamId} as={"div"} size={"big"} style={{ cursor: "default" }}>
                          <Icon name="trophy" />
                          {p.teamName}
                          <Label.Detail>{p.competitionName}</Label.Detail>
                        </Label>
                      ))}
                </Label.Group>
              </div>
              <Divider />
              <CompetitionsInSeason />
            </div>
          </div>
        </>
      )}
    </>
  );
};

export { SeasonPage };
