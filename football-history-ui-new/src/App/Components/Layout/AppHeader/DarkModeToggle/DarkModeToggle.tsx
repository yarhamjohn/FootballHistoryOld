import Switch from "@mui/material/Switch/Switch";
import { useContext } from "react";
import { ColorModeContext } from "../../../../Contexts/ColorModeContext";
import LightModeIcon from "@mui/icons-material/LightMode";
import DarkModeIcon from "@mui/icons-material/DarkMode";
import Stack from "@mui/material/Stack/Stack";

const DarkModeToggle = () => {
  const colorMode = useContext(ColorModeContext);

  return (
    <Stack direction="row" spacing={1} alignItems="center">
      <LightModeIcon />
      <Switch defaultChecked onClick={() => colorMode.toggleColorMode()} />
      <DarkModeIcon />
    </Stack>
  );
};

export { DarkModeToggle };
