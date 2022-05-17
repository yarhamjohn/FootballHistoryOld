import List from "@mui/material/List/List";
import ListItem from "@mui/material/ListItem/ListItem";
import ListItemText from "@mui/material/ListItemText/ListItemText";
import Paper from "@mui/material/Paper/Paper";
import Typography from "@mui/material/Typography/Typography";
import { FC, ReactElement } from "react";

const ScheduleChanges: FC = (): ReactElement => {
  return (
    <Paper elevation={10} style={{ padding: "1.25rem" }}>
      <Typography gutterBottom variant={"h6"}>
        Schedule changes
      </Typography>
      <List>
        <ListItem disableGutters>
          <ListItemText
            primaryTypographyProps={{ color: "primary" }}
            primary="World War One"
            secondary={"The leagues were abandoned in 1915/16, restarting in 1919/20."}
          />
        </ListItem>
        <ListItem disableGutters>
          <ListItemText
            primaryTypographyProps={{ color: "primary" }}
            primary="World War Two"
            secondary={"The leagues were abandoned in 1939/40, restarting in 1946/47."}
          />
        </ListItem>
        <ListItem disableGutters>
          <ListItemText
            primaryTypographyProps={{ color: "primary" }}
            primary="Covid-19"
            secondary={
              "The Premier League and Championship were temporarily suspended during 2019/20. League One and League Two were abandoned."
            }
          />
        </ListItem>
      </List>
    </Paper>
  );
};

export { ScheduleChanges };
