import Autocomplete from "@mui/material/Autocomplete/Autocomplete";
import Divider from "@mui/material/Divider/Divider";
import TextField from "@mui/material/TextField/TextField";
import { FC, ReactElement, useContext } from "react";
import { LeagueTable } from "../../Components/LeagueTable/LeagueTable";
import { SeasonsContext } from "../../Contexts/SeasonsContext";
import { Season } from "../../Domain/Types";
import { TabbedLeagues } from "./TabbedLeagues/TabbedLeagues";

const Seasons: FC = (): ReactElement => {
  const { seasons, activeSeason, setActiveSeason } = useContext(SeasonsContext);

  return (
    <div style={{ width: "100%", alignItems: "center", display: "flex", flexDirection: "column" }}>
      <Autocomplete
        value={activeSeason}
        onChange={(_, newValue: Season | null) => {
          if (newValue !== null) {
            setActiveSeason(newValue);
          }
        }}
        id="season-select"
        disableClearable
        options={seasons}
        getOptionLabel={(option) => `${option.startYear} - ${option.endYear}`}
        sx={{ width: 300, marginBottom: "2rem" }}
        renderInput={(params) => <TextField {...params} label="Season" />}
      />
      <Divider style={{ width: "100%", marginBottom: "2rem" }} />
      <div style={{ width: "80%" }}>
        <TabbedLeagues />
      </div>
    </div>
  );
};

export { Seasons };
