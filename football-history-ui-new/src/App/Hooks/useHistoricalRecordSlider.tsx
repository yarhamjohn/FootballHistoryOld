import { useContext, useState } from "react";
import { SeasonsContext } from "../Contexts/SeasonsContext";

const useHistoricalRecordSlider = (
  selectedRange: number[],
  updateSelectedRange: (newValue: number[]) => void
) => {
  const { firstSeason, lastSeason } = useContext(SeasonsContext);
  const [range, setRange] = useState<number[]>(selectedRange);

  const handleOnChangeCommitted = (
    _: React.SyntheticEvent | Event,
    newValue: number | number[]
  ) => {
    updateSelectedRange(newValue as number[]);
  };

  const handleOnChange = (_: Event, newValue: number | number[]) => setRange(newValue as number[]);

  return { firstSeason, lastSeason, range, handleOnChangeCommitted, handleOnChange };
};

export { useHistoricalRecordSlider };
