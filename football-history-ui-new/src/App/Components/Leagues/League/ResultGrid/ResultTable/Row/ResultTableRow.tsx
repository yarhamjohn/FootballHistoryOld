import { green, red, blue, grey } from "@mui/material/colors";
import TableCell from "@mui/material/TableCell/TableCell";
import TableRow from "@mui/material/TableRow/TableRow";
import { FC, ReactElement } from "react";
import { Match } from "../../../../../../Domain/Types";

type Props = { matches: Match[]; abbreviations: string[]; teamAbbreviation: string };

const ResultTableRow: FC<Props> = ({ matches, abbreviations, teamAbbreviation }): ReactElement => {
  const homeGames = matches.filter((m) => m.homeTeam.abbreviation === teamAbbreviation);
  const missingAbbreviations = abbreviations.filter(
    (x) => !homeGames.map((m) => m.awayTeam.abbreviation).includes(x)
  );

  // Add a fake game to ensure the layout is right
  for (let abb of missingAbbreviations) {
    homeGames.push({
      homeTeam: { abbreviation: abb },
      awayTeam: { abbreviation: abb }
    } as Match);
  }

  homeGames.sort((a, b) => {
    if (a.awayTeam.abbreviation > b.awayTeam.abbreviation) {
      return 1;
    }
    if (a.awayTeam.abbreviation < b.awayTeam.abbreviation) {
      return -1;
    }
    return 0;
  });

  const GetColor = (homeGoals: number, awayGoals: number) => {
    if (homeGoals > awayGoals) {
      return green[500];
    }

    if (homeGoals < awayGoals) {
      return red[500];
    }

    return blue[500];
  };

  return (
    <TableRow key={`Row: ${teamAbbreviation}`}>
      <TableCell>{teamAbbreviation}</TableCell>
      {homeGames.map((match) => {
        if (missingAbbreviations.includes(match.awayTeam.abbreviation)) {
          return (
            <TableCell
              key={`Cell: ${match.homeTeam.abbreviation}-${match.awayTeam.abbreviation}`}
              padding={"none"}
              align={"center"}
            />
          );
        } else {
          return (
            <TableCell
              key={`Cell: ${match.homeTeam.abbreviation}-${match.awayTeam.abbreviation}`}
              style={{
                cursor: "context-menu",
                backgroundColor: GetColor(match.homeTeam.goals, match.awayTeam.goals)
              }}
              padding={"none"}
              align={"center"}
              title={`${new Date(match.matchDate).toDateString()}: ${match.homeTeam.name} ${
                match.homeTeam.goals
              } - ${match.awayTeam.goals} ${match.awayTeam.name}`}
            >
              {`${match.homeTeam.goals}-${match.awayTeam.goals}`}
            </TableCell>
          );
        }
      })}
    </TableRow>
  );
};

export { ResultTableRow };
