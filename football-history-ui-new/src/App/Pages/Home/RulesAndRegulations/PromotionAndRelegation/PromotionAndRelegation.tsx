import List from "@mui/material/List/List";
import ListItem from "@mui/material/ListItem/ListItem";
import ListItemText from "@mui/material/ListItemText/ListItemText";
import Paper from "@mui/material/Paper/Paper";
import Typography from "@mui/material/Typography/Typography";
import { FC, ReactElement } from "react";

const PromotionAndRelegation: FC = (): ReactElement => {
  return (
    <Paper elevation={10} style={{ padding: "1.25rem" }}>
      <Typography gutterBottom variant={"h6"}>
        Promotion and Relegation
      </Typography>
      <List>
        <ListItem disableGutters>
          <ListItemText
            primaryTypographyProps={{ color: "primary" }}
            primary={"Test Matches"}
            secondary={
              <ul>
                <li>
                  From 1892-93 to 1897-98, test matches were used to determine promotion and
                  relegation between the First and Second Divisions.
                </li>
                <li>
                  These test matches took the form of a mini-league with each team from the First
                  Division playing 2-legged ties against each team in the Second Division.
                </li>
                <li>
                  First Division teams winning the mini-league retained their status, whilst Second
                  Division teams losing the mini-league stayed in the Second Division.
                </li>
                <li>
                  Second Division teams winning the mini-league had to apply for entry to the First
                  Division whilst First Division teams losing the mini-league were offered a place
                  in the Second Division.
                </li>
              </ul>
            }
          />
        </ListItem>
        <ListItem disableGutters>
          <ListItemText
            primaryTypographyProps={{ color: "primary" }}
            primary={"Elections"}
            secondary={
              <ul>
                <li>
                  From 1888-89 to 1985-86, a system of election/re-election was used to determine if
                  teams finishing bottom of the lowest division should be replaced in the League
                  with non-League teams.
                </li>
                <li>
                  Teams in the bottom division of the League were required to reapply if they
                  finished in the:
                  <ul>
                    <li>bottom 4 (1888-89 to 1892-93)</li>
                    <li>bottom 3 (1893-94 to 1920-21)</li>
                    <li>bottom 2 of either Division Three North or South (1921-22 to 1957-58)</li>
                    <li>bottom 4 (1958-59 - 1985-86)</li>
                  </ul>
                </li>
                <li>
                  Since 1986-87, the election system has been replaced with automatic relegation.
                </li>
              </ul>
            }
          />
        </ListItem>
        <ListItem disableGutters>
          <ListItemText
            primaryTypographyProps={{ color: "primary" }}
            primary={"Play Offs"}
            secondary={
              <ul>
                <li>
                  Play-offs to determine which additional team would be promoted from each division
                  were introduced from the 1986-87 season.
                </li>
                <li>
                  Initially, these involved one team from the higher division and 3 from the lower
                  division.
                </li>
                <li>
                  From 1988-89 however, the play-offs were made up of 4 teams from the lower
                  division only.
                </li>
                <li>
                  From the 1989-90 season, the play-off final was changed from 2-legged to 1-legged.
                </li>
              </ul>
            }
          />
        </ListItem>
      </List>
    </Paper>
  );
};

export { PromotionAndRelegation };
