import FormControlLabel from "@mui/material/FormControlLabel/FormControlLabel";
import Switch from "@mui/material/Switch/Switch";
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
