import TimelineItem from "@mui/lab/TimelineItem";
import TimelineSeparator from "@mui/lab/TimelineSeparator";
import TimelineConnector from "@mui/lab/TimelineConnector";
import TimelineContent from "@mui/lab/TimelineContent";
import TimelineOppositeContent from "@mui/lab/TimelineOppositeContent";
import Typography from "@mui/material/Typography/Typography";
import { FC, ReactElement } from "react";
import ArrowCircleUpIcon from "@mui/icons-material/ArrowCircleUp";
import AddCircleIcon from "@mui/icons-material/AddCircle";
import RemoveCircleIcon from "@mui/icons-material/RemoveCircle";
import ChangeCircleIcon from "@mui/icons-material/ChangeCircle";
import BuildCircleIcon from "@mui/icons-material/BuildCircle";
import { amber, blue, green, purple, red } from "@mui/material/colors";

type Props = {
  year: number;
  title: string;
  description: string;
  type: "add-league" | "add-team" | "reorganize" | "remove-team" | "rename";
};

const TimelineEntry: FC<Props> = ({ year, title, description, type }): ReactElement => {
  const color =
    type === "add-league"
      ? blue[500]
      : type === "add-team"
      ? green[500]
      : type === "remove-team"
      ? red[500]
      : type === "reorganize"
      ? amber[500]
      : type === "rename"
      ? purple[500]
      : undefined;

  const icon =
    type === "add-league" ? (
      <ArrowCircleUpIcon sx={{ color }} fontSize={"large"} />
    ) : type === "add-team" ? (
      <AddCircleIcon sx={{ color }} fontSize={"large"} />
    ) : type === "remove-team" ? (
      <RemoveCircleIcon sx={{ color }} fontSize={"large"} />
    ) : type === "reorganize" ? (
      <BuildCircleIcon sx={{ color }} fontSize={"large"} />
    ) : type === "rename" ? (
      <ChangeCircleIcon sx={{ color }} fontSize={"large"} />
    ) : null;

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

export { TimelineEntry };
