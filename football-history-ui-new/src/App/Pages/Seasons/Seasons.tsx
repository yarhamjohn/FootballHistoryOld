import Autocomplete from "@mui/material/Autocomplete/Autocomplete";
import Divider from "@mui/material/Divider/Divider";
import TextField from "@mui/material/TextField/TextField";
import { FC, ReactElement, useContext, useState } from "react";
import { SeasonsContext } from "../../Domain/SeasonsContext";
import { Season } from "../../Domain/Types";

const Seasons: FC = (): ReactElement => {
  const seasons = useContext(SeasonsContext);
  const [activeSeason, setActiveSeason] = useState<Season | null>(null);

  return (
    <div style={{ width: "100%", alignItems: "center", display: "flex", flexDirection: "column" }}>
      <Autocomplete
        value={activeSeason}
        onChange={(_, newValue: Season | null) => {
          setActiveSeason(newValue);
        }}
        id="season-select"
        options={seasons}
        getOptionLabel={(option) => `${option.startYear} - ${option.endYear}`}
        sx={{ width: 300, marginBottom: "2rem" }}
        renderInput={(params) => <TextField {...params} label="Season" />}
      />
      <Divider style={{ width: "100%" }} />
    </div>
  );
};

export { Seasons };
