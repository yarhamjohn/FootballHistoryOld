import Autocomplete from "@mui/material/Autocomplete/Autocomplete";
import Divider from "@mui/material/Divider/Divider";
import TextField from "@mui/material/TextField/TextField";
import { FC, ReactElement, useContext } from "react";
import { Team } from "../../Domain/Types";
import { TeamsContext } from "../../Contexts/TeamsContext";

const Teams: FC = (): ReactElement => {
  const { teams, activeTeam, setActiveTeam } = useContext(TeamsContext);

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
    </div>
  );
};

export { Teams };
