import Tab from "@mui/material/Tab/Tab";
import Tabs from "@mui/material/Tabs/Tabs";
import { FC, ReactElement, useState } from "react";
import { useNavigate } from "react-router-dom";

const TabBar: FC = (): ReactElement => {
  const [activeTab, setActiveTab] = useState<number>(0);
  const navigate = useNavigate();

  return (
    <Tabs value={activeTab} onChange={(_, i) => setActiveTab(i)}>
      <Tab label="Home" onClick={() => navigate("home")} />
      <Tab label="Teams" onClick={() => navigate("teams")} />
      <Tab label="Seasons" onClick={() => navigate("seasons")} />
      <Tab label="Competitions" onClick={() => navigate("competitions")} />
    </Tabs>
  );
};

export { TabBar };
