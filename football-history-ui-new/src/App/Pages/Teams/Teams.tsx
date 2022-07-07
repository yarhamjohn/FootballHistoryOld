import { Alert, CircularProgress, useMediaQuery } from "@mui/material";
import Autocomplete from "@mui/material/Autocomplete/Autocomplete";
import Divider from "@mui/material/Divider/Divider";
import TextField from "@mui/material/TextField/TextField";
import { FC, ReactElement, useContext } from "react";
import { HistoricalRecord } from "../../Components/HistoricalRecord/HistoricalRecord";
import { SeasonFilter } from "../../Components/SeasonFilter/SeasonFilter";
import { SeasonsContext } from "../../Contexts/SeasonsContext";
import { Team } from "../../Domain/Types";
import { useTeams } from "../../Hooks/useTeams";
import { LeagueTable } from "../../Components/Leagues/League/LeagueTable/LeagueTable";
import Box from "@mui/system/Box/Box";
import { PointDeduction } from "../../Components/Leagues/League/PointDeduction/PointDeduction";
import { useFetchLeagueBySeasonAndTeam } from "../../Hooks/useFetchLeague";

type Props = { teamId: number };

const TeamLeague: FC<Props> = ({ teamId }): ReactElement => {
  const { activeSeason } = useContext(SeasonsContext);
  const league = useFetchLeagueBySeasonAndTeam(activeSeason.id, teamId);

  const largeScreen = useMediaQuery("(min-width:1400px)");

  let body;

  if (league.isError) {
    body = <Alert severity="error">{league.error.message}</Alert>;
  } else if (league.isSuccess) {
    body = (
      <>
        <LeagueTable
          league={league.data}
          size={largeScreen ? "large" : "small"}
          openActiveTeamRow={true}
        />
        <Box sx={{ marginTop: "1rem", width: "100%" }}>
          <PointDeduction leagueTableRows={league.data.table} />
        </Box>
      </>
    );
  } else {
    body = <CircularProgress />;
  }

  return (
    <div style={{ width: "100%", alignItems: "center", display: "flex", flexDirection: "column" }}>
      <SeasonFilter />
      {body}
    </div>
  );
};

const Teams: FC = (): ReactElement => {
  const { teams, activeTeam, changeTeam } = useTeams();

  return (
    <div style={{ width: "100%", alignItems: "center", display: "flex", flexDirection: "column" }}>
      <Autocomplete
        value={activeTeam}
        onChange={(_, newTeam: Team | null) => changeTeam(newTeam)}
        id="team-select"
        options={teams}
        getOptionLabel={(option) => option.name}
        sx={{ width: 300, marginBottom: "2rem" }}
        renderInput={(params) => <TextField {...params} label="Team" />}
      />
      <Divider style={{ width: "100%", marginBottom: "2rem" }} />
      {activeTeam === null ? (
        <></>
      ) : (
        <Box sx={{ width: "80%" }}>
          <HistoricalRecord teamId={activeTeam.id} />
          <Divider style={{ width: "100%", marginTop: "2rem", marginBottom: "2rem" }} />
          <TeamLeague teamId={activeTeam.id} />
        </Box>
      )}
    </div>
  );
};

export { Teams };
