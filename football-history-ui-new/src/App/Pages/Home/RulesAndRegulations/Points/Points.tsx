import List from "@mui/material/List/List";
import ListItem from "@mui/material/ListItem/ListItem";
import ListItemText from "@mui/material/ListItemText/ListItemText";
import Paper from "@mui/material/Paper/Paper";
import Typography from "@mui/material/Typography/Typography";
import { FC, ReactElement } from "react";

const Points: FC = (): ReactElement => {
  return (
    <Paper elevation={10} style={{ padding: "1.25rem", marginRight: "1.25rem" }}>
      <Typography gutterBottom variant={"h6"}>
        Points
      </Typography>
      <List>
        <ListItem disableGutters>
          <ListItemText
            primaryTypographyProps={{ color: "primary" }}
            primary="1888/89 - 1980/81"
            secondary={"A win was awarded 2 points and a draw 1 point."}
          />
        </ListItem>
        <ListItem disableGutters>
          <ListItemText
            primaryTypographyProps={{ color: "primary" }}
            primary="1981/82 - current"
            secondary={"A win was awarded 3 points and a draw 1 point."}
          />
        </ListItem>
      </List>
    </Paper>
  );
};

export { Points };
