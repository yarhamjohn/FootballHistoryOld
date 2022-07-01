import Timeline from "@mui/lab/Timeline/Timeline";
import { FC, ReactElement } from "react";
import { Match } from "../../../../../Domain/Types";
import { PlayOffItem } from "./PlayOffItem";

type Props = { matches: Match[] };

const PlayOff: FC<Props> = ({ matches }): ReactElement => {
  const playOffMatches = matches.filter((m) => m.rules.type === "PlayOff");

  const semifinals = playOffMatches.filter((m) => m.rules.stage === "Semi-Final");
  const final = playOffMatches.filter((m) => m.rules.stage === "Final");

  const setOne = semifinals
    .filter((m) => m.homeTeam.id === final[0].homeTeam.id || m.awayTeam.id === final[0].homeTeam.id)
    .sort((a, b) => new Date(a.matchDate).valueOf() - new Date(b.matchDate).valueOf());

  const setTwo = semifinals
    .filter((m) => m.homeTeam.id === final[0].awayTeam.id || m.awayTeam.id === final[0].awayTeam.id)
    .sort((a, b) => new Date(a.matchDate).valueOf() - new Date(b.matchDate).valueOf());

  return (
    <>
      <Timeline position="alternate">
        <PlayOffItem match={setOne[0]} extraTime={false} penalties={false} color={"info"} />
        <PlayOffItem match={setTwo[0]} extraTime={false} penalties={false} color={"warning"} />
        <PlayOffItem
          match={setOne[1]}
          extraTime={
            setOne[0].homeTeam.goals + setOne[1].awayTeam.goals ===
            setOne[1].homeTeam.goals + setOne[0].awayTeam.goals
          }
          penalties={
            setOne[0].homeTeam.goals + setOne[1].awayTeam.goals ===
              setOne[1].homeTeam.goals + setOne[0].awayTeam.goals &&
            setOne[0].homeTeam.goalsExtraTime + setOne[1].awayTeam.goalsExtraTime ===
              setOne[1].homeTeam.goalsExtraTime + setOne[0].awayTeam.goalsExtraTime
          }
          color={"info"}
        />
        <PlayOffItem
          match={setTwo[1]}
          extraTime={
            setTwo[0].homeTeam.goals + setTwo[1].awayTeam.goals ===
            setTwo[1].homeTeam.goals + setTwo[0].awayTeam.goals
          }
          penalties={
            setTwo[0].homeTeam.goals + setTwo[1].awayTeam.goals ===
              setTwo[1].homeTeam.goals + setTwo[0].awayTeam.goals &&
            setTwo[0].homeTeam.goalsExtraTime + setTwo[1].awayTeam.goalsExtraTime ===
              setTwo[1].homeTeam.goalsExtraTime + setTwo[0].awayTeam.goalsExtraTime
          }
          color={"warning"}
        />
        <PlayOffItem
          match={final[0]}
          extraTime={final.length === 1 && final[0].homeTeam.goals === final[0].awayTeam.goals}
          penalties={
            (final.length === 1 &&
              final[0].homeTeam.goals === final[0].awayTeam.goals &&
              final[0].homeTeam.goalsExtraTime) === final[0].awayTeam.goalsExtraTime
          }
          color={"success"}
        />
        {final.length === 2 ? (
          <PlayOffItem
            match={final[1]}
            extraTime={
              final[0].homeTeam.goals + final[1].awayTeam.goals ===
              final[1].homeTeam.goals + final[0].awayTeam.goals
            }
            penalties={
              final[0].homeTeam.goals + final[1].awayTeam.goals ===
                final[1].homeTeam.goals + final[0].awayTeam.goals &&
              final[0].homeTeam.goalsExtraTime + final[1].awayTeam.goalsExtraTime ===
                final[1].homeTeam.goalsExtraTime + final[0].awayTeam.goalsExtraTime
            }
            color={"success"}
          />
        ) : null}
        {final.length === 3 ? (
          <>
            <PlayOffItem match={final[1]} extraTime={false} penalties={false} color={"success"} />
            <PlayOffItem
              match={final[2]}
              extraTime={final[2].homeTeam.goals === final[2].awayTeam.goals}
              penalties={
                final[2].homeTeam.goals === final[2].awayTeam.goals &&
                final[2].homeTeam.goalsExtraTime === final[2].awayTeam.goalsExtraTime
              }
              color={"success"}
            />
          </>
        ) : null}
      </Timeline>
    </>
  );
};

export { PlayOff };
