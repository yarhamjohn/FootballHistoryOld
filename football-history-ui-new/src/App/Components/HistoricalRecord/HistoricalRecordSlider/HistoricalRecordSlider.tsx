import Box from "@mui/material/Box/Box";
import Slider from "@mui/material/Slider/Slider";
import { FC, ReactElement, useContext, useState } from "react";
import { SeasonsContext } from "../../../Contexts/SeasonsContext";

type HistoricalRecordSliderProps = {
  historicalRecordRange: number[];
  updateHistoricalRecord: (newRange: number[]) => void;
};

const HistoricalRecordSlider: FC<HistoricalRecordSliderProps> = ({
  historicalRecordRange,
  updateHistoricalRecord
}): ReactElement => {
  const { firstSeason, lastSeason } = useContext(SeasonsContext);
  const [range, setRange] = useState<number[]>(historicalRecordRange);

  const handleOnChangeCommitted = (
    _: React.SyntheticEvent | Event,
    newValue: number | number[]
  ) => {
    updateHistoricalRecord(newValue as number[]);
  };

  const handleOnChange = (_: Event, newValue: number | number[]) => setRange(newValue as number[]);

  return (
    <Box sx={{ width: "50%" }}>
      <Slider
        min={firstSeason.startYear}
        max={lastSeason.startYear}
        value={range}
        onChange={handleOnChange}
        onChangeCommitted={handleOnChangeCommitted}
        valueLabelDisplay="on"
        valueLabelFormat={(x) => `${x} - ${x + 1}`}
      />
    </Box>
  );
};

export { HistoricalRecordSlider };
