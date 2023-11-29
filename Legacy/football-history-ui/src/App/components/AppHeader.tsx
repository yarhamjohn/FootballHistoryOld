import React, { FunctionComponent } from "react";
import { Menu } from "semantic-ui-react";
import { AppPage } from "../App";

const AppHeader: FunctionComponent<{
  activePage: AppPage;
  setActivePage: (activePage: AppPage) => void;
  style: React.CSSProperties;
}> = ({ activePage, setActivePage, style }) => {
  return (
    <Menu
      color={"teal"}
      pointing
      secondary
      style={{ ...style, fontSize: "1.5rem", alignSelf: "center", margin: 0 }}
    >
      <Menu.Item name="Home" active={activePage === "Home"} onClick={() => setActivePage("Home")}>
        Home
      </Menu.Item>
      <Menu.Item name="Team" active={activePage === "Team"} onClick={() => setActivePage("Team")}>
        Team
      </Menu.Item>
      <Menu.Item
        name="League"
        active={activePage === "League"}
        onClick={() => setActivePage("League")}
      >
        League
      </Menu.Item>
      <Menu.Item
        name="Season"
        active={activePage === "Season"}
        onClick={() => setActivePage("Season")}
      >
        Season
      </Menu.Item>
    </Menu>
  );
};

export { AppHeader };
