import List from "@mui/material/List/List";
import ListItem from "@mui/material/ListItem/ListItem";
import ListItemText from "@mui/material/ListItemText/ListItemText";
import Paper from "@mui/material/Paper/Paper";
import Typography from "@mui/material/Typography/Typography";
import { FC, ReactElement } from "react";

const Classification: FC = (): ReactElement => {
  return (
    <Paper elevation={10} style={{ padding: "1.25rem", marginRight: "1.25rem" }}>
      <Typography gutterBottom variant={"h6"}>
        Classification rules
      </Typography>
      <List>
        <ListItem disableGutters>
          <ListItemText
            primaryTypographyProps={{ color: "primary" }}
            primary="1888/89 - 1975/76"
            secondary={
              "Rankings were determined by 'Goal Average' (actually Goal Ratio - Goals For / Goals Against)."
            }
          />
        </ListItem>
        <ListItem disableGutters>
          <ListItemText
            primaryTypographyProps={{ color: "primary" }}
            primary="1976/77 - 1991/92"
            secondary={"Rankings were determined by Goal Difference then Goals For."}
          />
        </ListItem>
        <ListItem disableGutters>
          <ListItemText
            primaryTypographyProps={{ color: "primary" }}
            primary="1992/93 - 1999/00"
            secondary={
              "The Premier League switched to ranking by Goal Difference then Goals For, whilst the Football League continued to use Goals For then Goal Difference."
            }
          />
        </ListItem>
        <ListItem disableGutters>
          <ListItemText
            primaryTypographyProps={{ color: "primary" }}
            primary="2000/01 - current"
            secondary={"Rankings were determined by Goal Difference then Goals For."}
          />
        </ListItem>
        <ListItem disableGutters>
          <ListItemText
            primaryTypographyProps={{ color: "primary" }}
            primary="2019/20"
            secondary={
              "Rankings in Leagues One and Two were determined by Points Per Game due to Covid-19."
            }
          />
        </ListItem>
      </List>
    </Paper>
  );
};

export { Classification };
