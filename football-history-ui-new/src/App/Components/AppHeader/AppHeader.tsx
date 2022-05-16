import { DarkModeToggle } from "./DarkModeToggle/DarkModeToggle";
import SportsSoccerIcon from "@mui/icons-material/SportsSoccer";
import { TabBar } from "./TabBar/TabBar";
import Toolbar from "@mui/material/Toolbar/Toolbar";
import AppBar from "@mui/material/AppBar/AppBar";
import { FC, ReactElement } from "react";

type Props = {
  activeTab: number;
  setActiveTab: (index: number) => void;
};

const AppHeader: FC<Props> = ({ activeTab, setActiveTab }): ReactElement => {
  return (
    <>
      <AppBar color={"secondary"}>
        <Toolbar style={{ display: "flex", justifyContent: "space-between" }}>
          <SportsSoccerIcon color={"primary"} fontSize={"large"} />
          <TabBar activeTab={activeTab} setActiveTab={setActiveTab} />
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
