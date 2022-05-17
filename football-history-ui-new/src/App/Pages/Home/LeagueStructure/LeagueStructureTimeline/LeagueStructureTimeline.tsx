import Timeline from "@mui/lab/Timeline/Timeline";
import { FC, ReactElement } from "react";
import { TimelineEntry } from "./TimelineEntry/TimelineEntry";

const LeagueStructureTimeline: FC = (): ReactElement => {
  return (
    <Timeline position={"alternate"}>
      <TimelineEntry
        year={1888}
        title={"The football league was formed."}
        description={
          "There was a single division, the First Division, which consisted of 12 teams."
        }
        type={"add-league"}
      />
      <TimelineEntry
        year={1891}
        title={"Two teams were added."}
        description={"The First Division expanded to a total of 14 teams."}
        type={"add-team"}
      />
      <TimelineEntry
        year={1892}
        title={"A second tier was added."}
        description={
          "The First Division expanded to a total of 16 teams and a Second Division was added with 12 teams."
        }
        type={"add-league"}
      />
      <TimelineEntry
        year={1893}
        title={"Three teams were added."}
        description={"The Second Division expanded to a total of 15 teams."}
        type={"add-team"}
      />
      <TimelineEntry
        year={1894}
        title={"One team was added."}
        description={"The Second Division expanded to a total of 16 teams."}
        type={"add-team"}
      />
      <TimelineEntry
        year={1898}
        title={"Four teams were added."}
        description={
          "Both the First and Second Divisions were expanded to a total of 18 teams each."
        }
        type={"add-team"}
      />
      <TimelineEntry
        year={1905}
        title={"Four teams were added."}
        description={
          "Both the First and Second Divisions were expanded to a total of 20 teams each."
        }
        type={"add-team"}
      />
      <TimelineEntry
        year={1919}
        title={"Four teams were added."}
        description={
          "Both the First and Second Divisions were expanded to a total of 22 teams each."
        }
        type={"add-team"}
      />
      <TimelineEntry
        year={1920}
        title={"A third tier was added."}
        description={"A Third Division was added with 22 teams."}
        type={"add-league"}
      />
      <TimelineEntry
        year={1921}
        title={"The third tier was split into two divisions."}
        description={
          "20 teams were added and the Third Division was split into a Third Division North with 20 teams and a Third Division South with 22 teams."
        }
        type={"add-league"}
      />
      <TimelineEntry
        year={1923}
        title={"Two teams were added."}
        description={"The Third Division North was expanded to a total of 22 teams."}
        type={"add-team"}
      />
      <TimelineEntry
        year={1950}
        title={"Four teams were added."}
        description={
          "Both the Third Division North and the Third Division South were expanded to a total of 24 teams."
        }
        type={"add-team"}
      />
      <TimelineEntry
        year={1958}
        title={"The third tier was restructured."}
        description={
          "The Third Division North and Third Division South were restructured into a Third Division and a Fourth Division."
        }
        type={"add-league"}
      />
      <TimelineEntry
        year={1987}
        title={"The first and second tiers were resized."}
        description={
          "The First Division shrank to 21 teams and the Second Division grew to 23 teams."
        }
        type={"reorganize"}
      />
      <TimelineEntry
        year={1988}
        title={"The first and second tiers were resized."}
        description={
          "The First Division shrank to 20 teams and the Second Division grew to 24 teams."
        }
        type={"reorganize"}
      />
      <TimelineEntry
        year={1991}
        title={"The tiers were resized."}
        description={
          "One team was added, the First Division grew to 22 teams and the Fourth Division shrank to 23 teams."
        }
        type={"reorganize"}
      />
      <TimelineEntry
        year={1992}
        title={"Creation of the Premier League."}
        description={
          "The First Division was renamed the Premier League, whilst the First, Second and Third Divisions became Divisions One, Two and Three."
        }
        type={"rename"}
      />
      <TimelineEntry
        year={1993}
        title={"One team was removed."}
        description={"The Third Division shrank to 22 teams."}
        type={"remove-team"}
      />
      <TimelineEntry
        year={1995}
        title={"The tiers were resized."}
        description={
          "The Premier League shrank to 20 teams and the Third Division grew to 24 teams."
        }
        type={"reorganize"}
      />
      <TimelineEntry
        year={2004}
        title={"The leagues were renamed."}
        description={
          "Divisions One, Two and Three became The Championship, League One and League Two."
        }
        type={"rename"}
      />
    </Timeline>
  );
};

export { LeagueStructureTimeline };
