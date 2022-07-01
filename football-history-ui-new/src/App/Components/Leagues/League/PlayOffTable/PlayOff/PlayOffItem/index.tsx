import TimelineConnector from "@mui/lab/TimelineConnector";
import TimelineContent from "@mui/lab/TimelineContent";
import TimelineItem from "@mui/lab/TimelineItem/TimelineItem";
import TimelineOppositeContent from "@mui/lab/TimelineOppositeContent/TimelineOppositeContent";
import TimelineSeparator from "@mui/lab/TimelineSeparator";
import TimelineDot from "@mui/lab/TimelineDot";
import { Card, CardContent, Typography } from "@mui/material";
import { FC, ReactElement } from "react";
import { Match } from "../../../../../../Domain/Types";

type Props = {
  match: Match;
  extraTime: boolean;
  penalties: boolean;
  color?:
    | "inherit"
    | "grey"
    | "primary"
    | "secondary"
    | "error"
    | "info"
    | "success"
    | "warning"
    | undefined;
};

const PlayOffItem: FC<Props> = ({ match, extraTime, penalties, color }): ReactElement => {
  return (
    <TimelineItem>
      <TimelineOppositeContent
        sx={{ m: "auto 0" }}
        align="right"
        variant="body2"
        color="text.secondary"
      >
        {new Date(match.matchDate).toDateString()}
      </TimelineOppositeContent>
      <TimelineSeparator>
        <TimelineConnector />
        <TimelineDot color={color} />
        <TimelineConnector />
      </TimelineSeparator>
      <TimelineContent sx={{ py: "12px", px: 2 }}>
        <Card>
          <CardContent>
            <Typography variant="body2">
              {`${match.homeTeam.name} ${match.homeTeam.goals} - ${match.awayTeam.goals} ${match.awayTeam.name}`}
            </Typography>
            {extraTime && (
              <Typography variant="body2">
                {`${match.homeTeam.goalsExtraTime} - ${match.awayTeam.goalsExtraTime} (A.E.T)`}
              </Typography>
            )}
            {penalties && (
              <Typography variant="body2">
                {`${match.homeTeam.penaltiesScored} (${match.homeTeam.penaltiesTaken}) - ${match.awayTeam.penaltiesScored} (${match.awayTeam.penaltiesTaken}) (Pens)`}
              </Typography>
            )}
          </CardContent>
        </Card>
      </TimelineContent>
    </TimelineItem>
  );
};

export { PlayOffItem };
