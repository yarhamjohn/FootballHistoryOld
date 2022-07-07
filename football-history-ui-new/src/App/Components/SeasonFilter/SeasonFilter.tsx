import Autocomplete from "@mui/material/Autocomplete/Autocomplete";
import TextField from "@mui/material/TextField/TextField";
import { FC, ReactElement, useContext } from "react";
import { Season } from "../../Domain/Types";
import Button from "@mui/material/Button/Button";
import { SeasonsContext } from "../../Contexts/SeasonsContext";

const SeasonFilter: FC = (): ReactElement => {
  const {
    seasons,
    activeSeason,
    setActiveSeason,
    moveNext,
    movePrevious,
    moveOldest,
    moveNewest,
    next,
    previous
  } = useContext(SeasonsContext);

  return (
    <div style={{ display: "flex", columnGap: "1rem", marginBottom: "2rem" }}>
      <Button variant="outlined" size="small" onClick={moveOldest} disabled={next === undefined}>
        &lt;&lt;
      </Button>
      <Button variant="outlined" size="small" onClick={moveNext} disabled={next === undefined}>
        &lt;
      </Button>
      <Autocomplete
        value={activeSeason}
        onChange={(_, newSeason: Season | null) => newSeason !== null && setActiveSeason(newSeason)}
        id="season-select"
        disableClearable
        options={seasons}
        getOptionLabel={(option) => `${option.startYear} - ${option.endYear}`}
        sx={{ width: 300 }}
        renderInput={(params) => <TextField {...params} label="Season" />}
      />
      <Button
        variant="outlined"
        size="small"
        onClick={movePrevious}
        disabled={previous === undefined}
      >
        &gt;
      </Button>
      <Button
        variant="outlined"
        size="small"
        onClick={moveNewest}
        disabled={previous === undefined}
      >
        &gt;&gt;
      </Button>
    </div>
  );
};

export { SeasonFilter };
