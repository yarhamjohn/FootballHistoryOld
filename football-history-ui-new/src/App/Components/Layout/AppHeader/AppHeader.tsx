import { DarkModeToggle } from "./DarkModeToggle/DarkModeToggle";
import SportsSoccerIcon from "@mui/icons-material/SportsSoccer";
import { ActiveTab, TabBar } from "./TabBar/TabBar";
import Toolbar from "@mui/material/Toolbar/Toolbar";
import AppBar from "@mui/material/AppBar/AppBar";
import { FC, ReactElement } from "react";

type Props = {
  activeTab: ActiveTab;
};

const AppHeader: FC<Props> = ({ activeTab }): ReactElement => {
  return (
    <>
      <AppBar color={"secondary"}>
        <Toolbar style={{ display: "flex", justifyContent: "space-between" }}>
          <div style={{ width: "10rem", display: "flex", justifyContent: "left" }}>
            <SportsSoccerIcon color={"primary"} fontSize={"large"} />
          </div>
          <TabBar activeTab={activeTab} />
          <div style={{ width: "10rem", display: "flex", justifyContent: "right" }}>
            <DarkModeToggle />
          </div>
        </Toolbar>
      </AppBar>
      {/* Hack (as per docs: https://mui.com/#fixed-placement)
          to ensure content isn't rendered behind the app bar. */}
      <Toolbar />
    </>
  );
};

export { AppHeader };
