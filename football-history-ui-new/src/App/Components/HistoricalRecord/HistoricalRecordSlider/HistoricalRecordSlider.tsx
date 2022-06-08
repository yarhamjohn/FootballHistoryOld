import Box from "@mui/material/Box/Box";
import Slider from "@mui/material/Slider/Slider";
import { FC, ReactElement } from "react";
import { useHistoricalRecordSlider } from "../../../Hooks/useHistoricalRecordSlider";

type HistoricalRecordSliderProps = {
  selectedRange: number[];
  updateSelectedRange: (newRange: number[]) => void;
};

const HistoricalRecordSlider: FC<HistoricalRecordSliderProps> = ({
  selectedRange,
  updateSelectedRange
}): ReactElement => {
  const { firstSeason, lastSeason, range, handleOnChange, handleOnChangeCommitted } =
    useHistoricalRecordSlider(selectedRange, updateSelectedRange);

  return (
    <Box sx={{ width: "75%" }}>
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
