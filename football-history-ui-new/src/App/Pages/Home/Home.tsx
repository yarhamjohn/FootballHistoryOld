import Card from "@mui/material/Card/Card";
import CardContent from "@mui/material/CardContent/CardContent";
import Chip from "@mui/material/Chip/Chip";
import Timeline from "@mui/lab/Timeline";
import TimelineItem from "@mui/lab/TimelineItem";
import TimelineSeparator from "@mui/lab/TimelineSeparator";
import TimelineConnector from "@mui/lab/TimelineConnector";
import TimelineContent from "@mui/lab/TimelineContent";
import TimelineOppositeContent from "@mui/lab/TimelineOppositeContent";
import TimelineDot from "@mui/lab/TimelineDot";
import Typography from "@mui/material/Typography/Typography";
import { FC, ReactElement } from "react";
import ArrowCircleUpIcon from "@mui/icons-material/ArrowCircleUp";
import AddCircleIcon from "@mui/icons-material/AddCircle";
import { ResponsiveStyleValue } from "@mui/system";
import { Property } from "@babel/types";

type Props = {
  year: number;
  title: string;
  description: string;
  type: "new-league" | "new-teams";
};

const TimelineEntry: FC<Props> = ({ year, title, description, type }): ReactElement => {
  const color = type === "new-league" ? "primary" : "secondary";

  const icon =
    type === "new-league" ? <ArrowCircleUpIcon color={color} /> : <AddCircleIcon color={color} />;

  return (
    <TimelineItem>
      <TimelineOppositeContent sx={{ m: "auto 0" }}>
        <Typography color={color} variant={"h5"}>
          {year}
        </Typography>
      </TimelineOppositeContent>
      <TimelineSeparator>
        <TimelineConnector />
        {icon}
        <TimelineConnector />
      </TimelineSeparator>
      <TimelineContent>
        <Typography variant={"h6"}>{title}</Typography>
        <Typography variant={"body2"}>{description}</Typography>
      </TimelineContent>
    </TimelineItem>
  );
};

const Home: FC = (): ReactElement => {
  return (
    <div>
      <Typography gutterBottom variant={"h2"}>
        History of the English Football League
      </Typography>
      <Typography gutterBottom variant={"body1"}>
        This website provides a variety of metrics including league tables, match results and
        historical performance data for each of the teams to have featured in the top 4 divisions of
        the English Football League since its founding in 1888.
      </Typography>

      <Card>
        <CardContent>
          <Typography gutterBottom variant={"h4"}>
            League structure
          </Typography>
          <Timeline position={"alternate"}>
            <TimelineEntry
              year={1888}
              title={"The football league was formed."}
              description={
                "There was a single division, the First Division, which consisted of 12 teams."
              }
              type={"new-league"}
            />
            <TimelineEntry
              year={1891}
              title={"Two teams were added."}
              description={"The First Division expanded to a total of 14 teams."}
              type={"new-teams"}
            />
            <TimelineEntry
              year={1892}
              title={"A second tier was added."}
              description={
                "The First Division expanded to a total of 16 teams and a Second Division was added with 12 teams."
              }
              type={"new-league"}
            />
            <TimelineEntry
              year={1893}
              title={"Three teams were added."}
              description={"The Second Division expanded to a total of 15 teams."}
              type={"new-teams"}
            />
            <TimelineEntry
              year={1894}
              title={"One team was added."}
              description={"The Second Division expanded to a total of 16 teams."}
              type={"new-teams"}
            />
            <TimelineEntry
              year={1898}
              title={"Four teams were added."}
              description={
                "Both the First and Second Divisions were expanded to a total of 18 teams each."
              }
              type={"new-teams"}
            />
            <TimelineEntry
              year={1905}
              title={"Four teams were added."}
              description={
                "Both the First and Second Divisions were expanded to a total of 20 teams each."
              }
              type={"new-teams"}
            />
            <TimelineEntry
              year={1919}
              title={"Four teams were added."}
              description={
                "Both the First and Second Divisions were expanded to a total of 22 teams each."
              }
              type={"new-teams"}
            />
            <TimelineEntry
              year={1920}
              title={"A third tier was added."}
              description={"A Third Division was added with 22 teams."}
              type={"new-league"}
            />
            <TimelineEntry
              year={1921}
              title={"The third tier was split into two divisions."}
              description={
                "20 teams were added and the Third Division was split into a Third Division North with 20 teams and a Third Division South with 22 teams."
              }
              type={"new-league"}
            />
            <TimelineEntry
              year={1923}
              title={"Two teams were added."}
              description={"The Third Division North was expanded to a total of 22 teams."}
              type={"new-teams"}
            />
            <TimelineEntry
              year={1950}
              title={"Four teams were added."}
              description={
                "Both the Third Division North and the Third Division South were expanded to a total of 24 teams."
              }
              type={"new-teams"}
            />
            <TimelineEntry
              year={1958}
              title={"The third tier was restructured."}
              description={
                "The Third Division North and Third Division South were restructured into a Third Division and a Fourth Division."
              }
              type={"new-league"}
            />
            <TimelineEntry
              year={1987}
              title={"The first and second tiers were resized."}
              description={
                "The First Division shrank to 21 teams and the Second Division grew to 23 teams."
              }
              type={"new-teams"}
            />
            <TimelineEntry
              year={1988}
              title={"The first and second tiers were resized."}
              description={
                "The First Division shrank to 20 teams and the Second Division grew to 24 teams."
              }
              type={"new-teams"}
            />
            <TimelineEntry
              year={1991}
              title={"The tiers were resized."}
              description={
                "One team was added, the First Division grew to 22 teams and the Fourth Division shrank to 23 teams."
              }
              type={"new-teams"}
            />
            <TimelineEntry
              year={1992}
              title={"Creation of the Premier League."}
              description={
                "The First Division was renamed the Premier League, whilst the First, Second and Third Divisions became Divisions One, Two and Three."
              }
              type={"new-league"}
            />
            <TimelineEntry
              year={1993}
              title={"One team was removed."}
              description={"The Third Division shrank to 22 teams."}
              type={"new-teams"}
            />
            <TimelineEntry
              year={1995}
              title={"The tiers were resized."}
              description={
                "The Premier League shrank to 20 teams and the Third Division grew to 24 teams."
              }
              type={"new-teams"}
            />
            <TimelineEntry
              year={2004}
              title={"The leagues were renamed."}
              description={
                "Divisions One, Two and Three became The Championship, League One and League Two."
              }
              type={"new-league"}
            />
          </Timeline>
        </CardContent>
      </Card>
      <h3>Rules and regulations</h3>
      <h4>
        <em>Points</em>
      </h4>
      <ul>
        <li>
          From the inception of the league in 1888, 2 points were awarded for a win and 1 for a
          draw.
        </li>
        <li>
          This was changed to 3 points for a win and 1 point for a draw from the 1981-82 season.
        </li>
      </ul>
      <h4>
        <em>Rankings</em>
      </h4>
      <ul>
        <li>
          Initially, 'Goal Average' (actually goal ratio - goals for / goals against) was used to
          separate teams level on points.
        </li>
        <li>For the 1976-77 season, this was replaced with Goal Difference.</li>
        <li>
          After the formation of the Premier League in 1992-93, the Football League used Goals For
          before Goal difference up to the 1998-99 season before changing back from 1999-00
        </li>
      </ul>
      <h4>
        <em>Promotion and Relegation</em>
      </h4>
      <ul>
        <li>
          From 1892-93 to 1897-98, test matches were used to determine promotion and relegation
          between the First and Second Divisions.
        </li>
        <li>
          These test matches took the form of a mini-league with each team from the First Division
          playing 2-legged ties against each team in the Second Division.
        </li>
        <li>
          First Division teams winning the mini-league retained their status, whilst Second Division
          teams losing the mini-league stayed in the Second Division.
        </li>
        <li>
          Second Division teams winning the mini-league had to apply for entry to the First Division
          whilst First Division teams losting the mini-league were offered a place in the Second
          Division.
        </li>
      </ul>
      <ul>
        <li>
          From 1888-89 to 1985-86, a system of election/re-election was used to determine if teams
          finishing bottom of the lowest division should be replaced in the League with non-League
          teams.
        </li>
        <li>
          Teams in the bottom division of the League were required to reapply if they finished in
          the:
          <ul>
            <li>bottom 4 (1888-89 to 1892-93)</li>
            <li>bottom 3 (1893-94 to 1920-21)</li>
            <li>bottom 2 of either Division Three North or South (1921-22 to 1957-58)</li>
            <li>bottom 4 (1958-59 - 1985-86)</li>
          </ul>
        </li>
        <li>Since 1986-87, the election system has been replaced with automatic relegation.</li>
      </ul>
      <ul>
        <li>
          Play-offs to determine which additional team would be promoted from each division were
          introduced from the 1986-87 season.
        </li>
        <li>
          Initially, these involved one team from the higher division and 3 from the lower division.
        </li>
        <li>
          From 1988-89 however, the play-offs were made up of 4 teams from the lower division only.
        </li>
        <li>From the 1989-90 season, the play-off final was changed from 2-legged to 1-legged.</li>
      </ul>
    </div>
  );
};

export { Home };
