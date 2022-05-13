import { Switch, Box, IconButton, Tab, Tabs, Toolbar, FormControlLabel } from "@mui/material";
import AppBar from "@mui/material/AppBar";
import SvgIcon from "@mui/material/SvgIcon";
import { useContext, useState } from "react";
import { ColorModeContext } from "../../Contexts/ColorModeContext";

const DarkModeToggle = () => {
  const colorMode = useContext(ColorModeContext);

  return (
    <FormControlLabel
      control={<Switch defaultChecked onClick={() => colorMode.toggleColorMode()} />}
      label={"Dark mode"}
    />
  );
};

const HomeButton = () => {
  return (
    <IconButton color={"primary"}>
      <SvgIcon>
        <path d="M10 20v-6h4v6h5v-8h3L12 3 2 12h3v8z" />
      </SvgIcon>
    </IconButton>
  );
};

const TabBar = () => {
  const [activeTab, setActiveTab] = useState<number>(0);

  return (
    <Tabs value={activeTab} onChange={(e, n) => setActiveTab(n)}>
      <Tab label="Teams" />
      <Tab label="Seasons" />
      <Tab label="Competitions" />
    </Tabs>
  );
};

const AppHeader = () => {
  return (
    <>
      <AppBar color={"secondary"}>
        <Toolbar style={{ display: "flex", justifyContent: "space-between" }}>
          <HomeButton />
          <TabBar />
          <DarkModeToggle />
        </Toolbar>
      </AppBar>
      {/* Hack (as per docs: https://mui.com/#fixed-placement)
          to ensure content isn't rendered behind the app bar. */}
      <Toolbar />
    </>
  );
};

export { AppHeader };
