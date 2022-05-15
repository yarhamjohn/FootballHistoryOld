import { Tab, Tabs } from "@mui/material";
import { useState } from "react";

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

export { TabBar };
