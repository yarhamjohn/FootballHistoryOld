import Tab from "@mui/material/Tab/Tab";
import Tabs from "@mui/material/Tabs/Tabs";
import { FC, ReactElement } from "react";

type Props = {
  activeTab: number;
  setActiveTab: (index: number) => void;
};

const TabBar: FC<Props> = ({ activeTab, setActiveTab }): ReactElement => {
  return (
    <Tabs value={activeTab} onChange={(_, i) => setActiveTab(i)}>
      <Tab label="Home" />
      <Tab label="Teams" />
      <Tab label="Seasons" />
      <Tab label="Competitions" />
    </Tabs>
  );
};

export { TabBar };
