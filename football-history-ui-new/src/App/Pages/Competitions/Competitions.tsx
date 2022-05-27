import Autocomplete from "@mui/material/Autocomplete/Autocomplete";
import Divider from "@mui/material/Divider/Divider";
import TextField from "@mui/material/TextField/TextField";
import { FC, ReactElement, useContext } from "react";
import { CompetitionsContext } from "../../Contexts/CompetitionsContext";
import { SeasonsContext } from "../../Contexts/SeasonsContext";
import { Competition } from "../../Domain/Types";

const Competitions: FC = (): ReactElement => {
  const { competitions, activeCompetition, setActiveCompetition } = useContext(CompetitionsContext);
  const { activeSeason } = useContext(SeasonsContext);

  const getCompetitionsToDisplay = (): Competition[] => {
    return competitions.filter((c) => c.season.id === activeSeason.id);
  };

  return (
    <div style={{ width: "100%", alignItems: "center", display: "flex", flexDirection: "column" }}>
      <Autocomplete
        value={activeCompetition}
        onChange={(_, newValue: Competition | null) => {
          setActiveCompetition(newValue);
        }}
        id="competition-select"
        options={getCompetitionsToDisplay()}
        getOptionLabel={(option) => option.name}
        sx={{ width: 300, marginBottom: "2rem" }}
        renderInput={(params) => <TextField {...params} label="Competition" />}
      />
      <Divider style={{ width: "100%" }} />
    </div>
  );
};

export { Competitions };
