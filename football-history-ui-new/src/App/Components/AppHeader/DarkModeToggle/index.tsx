import { Switch, FormControlLabel } from "@mui/material";
import { useContext } from "react";
import { ColorModeContext } from "../../../Contexts/ColorModeContext";

const DarkModeToggle = () => {
  const colorMode = useContext(ColorModeContext);

  return (
    <FormControlLabel
      control={<Switch defaultChecked onClick={() => colorMode.toggleColorMode()} />}
      label={"Dark mode"}
    />
  );
};

export { DarkModeToggle };
