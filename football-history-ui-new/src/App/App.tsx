import { FC, ReactElement, useState } from "react";
import { Layout } from "./Components/Layout/Layout";
import { Competitions } from "./Pages/Competitions/Competitions";
import { Home } from "./Pages/Home/Home";
import { Seasons } from "./Pages/Seasons/Seasons";
import { Teams } from "./Pages/Teams/Teams";

enum Pages {
  "Home" = 0,
  "Teams" = 1,
  "Seasons" = 2,
  "Competitions" = 3
}

const App: FC = (): ReactElement => {
  const [activeTab, setActiveTab] = useState<number>(Pages.Home);

  return (
    <Layout activeTab={activeTab} setActiveTab={setActiveTab}>
      {activeTab === Pages.Home ? (
        <Home />
      ) : activeTab === Pages.Teams ? (
        <Teams />
      ) : activeTab === Pages.Seasons ? (
        <Seasons />
      ) : (
        <Competitions />
      )}
    </Layout>
  );
};

export { App };
