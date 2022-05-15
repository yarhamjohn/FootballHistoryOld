import { Toolbar } from "@mui/material";
import AppBar from "@mui/material/AppBar";
import { DarkModeToggle } from "./DarkModeToggle";
import { HomeButton } from "./HomeButton";
import { TabBar } from "./TabBar";

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
