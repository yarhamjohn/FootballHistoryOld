import Alert from "@mui/material/Alert/Alert";
import Autocomplete from "@mui/material/Autocomplete/Autocomplete";
import CircularProgress from "@mui/material/CircularProgress/CircularProgress";
import Divider from "@mui/material/Divider/Divider";
import TextField from "@mui/material/TextField/TextField";
import { FC, ReactElement, useState } from "react";
import { Team } from "../../Domain/Types";
import { useFetchTeams } from "../../Hooks/useFetchTeams";

const Teams: FC = (): ReactElement => {
  const { teamsState } = useFetchTeams();
  const [activeTeam, setActiveTeam] = useState<Team | null>(null);

  if (teamsState.status === "FETCH_NOT_STARTED") {
    return <></>;
  }

  if (teamsState.status === "FETCH_IN_PROGRESS") {
    return <CircularProgress />;
  }

  if (teamsState.status === "FETCH_ERROR") {
    return <Alert severity="error">{teamsState.error.message}</Alert>;
  }

  return (
    <div style={{ width: "100%", alignItems: "center", display: "flex", flexDirection: "column" }}>
      <Autocomplete
        value={activeTeam}
        onChange={(_, newValue: Team | null) => {
          setActiveTeam(newValue);
        }}
        id="team-select"
        options={teamsState.data}
        getOptionLabel={(option) => option.name}
        sx={{ width: 300, marginBottom: "2rem" }}
        renderInput={(params) => <TextField {...params} label="Team" />}
      />
      <Divider style={{ width: "100%" }} />
    </div>
  );
};

export { Teams };
