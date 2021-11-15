import { FunctionComponent } from "react";
import { Card, Loader } from "semantic-ui-react";
import { ErrorMessage } from "../components/ErrorMessage";
import { useFetchStatistics } from "../shared/useFetchStatistics";

const CombinedStatistics: FunctionComponent<{ seasonId: number }> = ({ seasonId }) => {
  const statistics = useFetchStatistics(seasonId);

  if (statistics.status === "LOADING") {
    return <Loader />;
  }

  if (statistics.status === "LOAD_FAILED") {
    return (
      <ErrorMessage
        header="Failed to load statistics"
        content="Something went wrong attempting to load the season statistics"
      />
    );
  }

  if (statistics.status === "UNLOADED") {
    return null;
  }

  return (
    <>
      {statistics.data.map((x) => (
        <Card color="teal" key={x.category}>
          <Card.Content>
            <Card.Header>{x.category}</Card.Header>
          </Card.Content>
          {x.statistics.map((y, i) => (
            <Card.Content key={i}>
              <Card.Meta>{y.name}</Card.Meta>
              <Card.Description>
                {y.teamName} ({y.competitionName}): <strong>{Number(y.value.toFixed(2))}</strong>
              </Card.Description>
            </Card.Content>
          ))}
        </Card>
      ))}
    </>
  );
};

export { CombinedStatistics };
