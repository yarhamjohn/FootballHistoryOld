import Autocomplete from "@mui/material/Autocomplete/Autocomplete";
import Divider from "@mui/material/Divider/Divider";
import TextField from "@mui/material/TextField/TextField";
import { FC, ReactElement, useContext, useState } from "react";
import { Team } from "../../Domain/Types";
import { TeamsContext } from "../../Domain/TeamsContext";

const Teams: FC = (): ReactElement => {
  const teams = useContext(TeamsContext);
  const [activeTeam, setActiveTeam] = useState<Team | null>(null);

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
