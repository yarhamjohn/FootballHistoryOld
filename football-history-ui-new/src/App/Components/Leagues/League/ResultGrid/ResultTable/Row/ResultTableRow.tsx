import { green, red, blue } from "@mui/material/colors";
import TableCell from "@mui/material/TableCell/TableCell";
import TableRow from "@mui/material/TableRow/TableRow";
import { FC, ReactElement } from "react";
import { Match, Size } from "../../../../../../Domain/Types";

type Props = { matches: Match[]; abbreviations: string[]; teamAbbreviation: string; size: Size };

const ResultTableRow: FC<Props> = ({
  matches,
  abbreviations,
  teamAbbreviation,
  size
}): ReactElement => {
  const homeGames = matches.filter((m) => m.homeTeam.abbreviation === teamAbbreviation);
  const missingAbbreviations = abbreviations.filter(
    (x) => !homeGames.map((m) => m.awayTeam.abbreviation).includes(x)
  );

  // Add a fake game to ensure the layout is right
  for (const abb of missingAbbreviations) {
    homeGames.push({
      homeTeam: {
        abbreviation: abb,
        name: matches.filter((m) => m.homeTeam.abbreviation === abb)[0].homeTeam.name
      },
      awayTeam: {
        abbreviation: abb,
        name: matches.filter((m) => m.awayTeam.abbreviation === abb)[0].awayTeam.name
      }
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

  const getFullName = () => {
    return homeGames[0].homeTeam.name;
  };

  return (
    <TableRow key={`Row: ${teamAbbreviation}`} hover>
      <TableCell>{size === "small" ? teamAbbreviation : getFullName()}</TableCell>
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
