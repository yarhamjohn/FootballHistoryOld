import Autocomplete from "@mui/material/Autocomplete/Autocomplete";
import Divider from "@mui/material/Divider/Divider";
import TextField from "@mui/material/TextField/TextField";
import { FC, ReactElement } from "react";
import { HistoricalRecord } from "../../Components/HistoricalRecord/HistoricalRecord";
import { Team } from "../../Domain/Types";
import { useTeams } from "../../Hooks/useTeams";

const Teams: FC = (): ReactElement => {
  const { teams, activeTeam, setActiveTeam } = useTeams();

  return (
    <div style={{ width: "100%", alignItems: "center", display: "flex", flexDirection: "column" }}>
      <Autocomplete
        value={activeTeam}
        onChange={(_, newValue: Team | null) => {
          setActiveTeam(newValue);
        }}
        id="team-select"
        options={teams}
        getOptionLabel={(option) => option.name}
        sx={{ width: 300, marginBottom: "2rem" }}
        renderInput={(params) => <TextField {...params} label="Team" />}
      />
      <Divider style={{ width: "100%" }} />
      {activeTeam === null ? <></> : <HistoricalRecord teamId={activeTeam.id} />}
    </div>
  );
};

export { Teams };
