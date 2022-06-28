import Autocomplete from "@mui/material/Autocomplete/Autocomplete";
import Divider from "@mui/material/Divider/Divider";
import TextField from "@mui/material/TextField/TextField";
import { FC, ReactElement } from "react";
import { Season } from "../../Domain/Types";
import { useSeasons } from "../../Hooks/useSeasons";
import { Leagues } from "../../Components/Leagues/Leagues";

const Seasons: FC = (): ReactElement => {
  const { seasons, activeSeason, changeSeason } = useSeasons();

  return (
    <div style={{ width: "100%", alignItems: "center", display: "flex", flexDirection: "column" }}>
      <Autocomplete
        value={activeSeason}
        onChange={(_, newSeason: Season | null) => newSeason !== null && changeSeason(newSeason)}
        id="season-select"
        disableClearable
        options={seasons}
        getOptionLabel={(option) => `${option.startYear} - ${option.endYear}`}
        sx={{ width: 300, marginBottom: "2rem" }}
        renderInput={(params) => <TextField {...params} label="Season" />}
      />
      <Divider style={{ width: "100%", marginBottom: "2rem" }} />
      <div style={{ width: "80%" }}>
        <Leagues />
      </div>
    </div>
  );
};

export { Seasons };
