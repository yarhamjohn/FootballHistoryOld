import Tab from "@mui/material/Tab/Tab";
import Tabs from "@mui/material/Tabs/Tabs";
import { FC, ReactElement, useState } from "react";
import { useNavigate } from "react-router-dom";

enum ActiveTab {
  home = 0,
  teams = 1,
  seasons = 2,
  competitions = 3,
  unknown = 4
}

type Props = {
  activeTab: ActiveTab;
};

const TabBar: FC<Props> = ({ activeTab }): ReactElement => {
  const [currentTab, setCurrentTab] = useState<number>(activeTab);
  const navigate = useNavigate();

  return (
    <Tabs
      value={currentTab === ActiveTab.unknown ? false : currentTab}
      onChange={(_, i) => setCurrentTab(i)}
    >
      <Tab
        value={ActiveTab.home}
        label="Home"
        onClick={() => navigate(`/${ActiveTab[ActiveTab.home]}`)}
      />
      <Tab
        value={ActiveTab.teams}
        label="Teams"
        onClick={() => navigate(`/${ActiveTab[ActiveTab.teams]}`)}
      />
      <Tab
        value={ActiveTab.seasons}
        label="Seasons"
        onClick={() => navigate(`/${ActiveTab[ActiveTab.seasons]}`)}
      />
      <Tab
        value={ActiveTab.competitions}
        label="Competitions"
        onClick={() => navigate(`/${ActiveTab[ActiveTab.competitions]}`)}
      />
    </Tabs>
  );
};

export { TabBar, ActiveTab };
