import { FC, ReactElement } from "react";
import { Navigate, Route, Routes } from "react-router-dom";
import { ActiveTab } from "./Components/AppHeader/TabBar/TabBar";
import { Layout } from "./Components/Layout/Layout";
import { Competitions } from "./Pages/Competitions/Competitions";
import { Home } from "./Pages/Home/Home";
import { NotFound } from "./Pages/NotFound/NotFound";
import { Seasons } from "./Pages/Seasons/Seasons";
import { Teams } from "./Pages/Teams/Teams";

const App: FC = (): ReactElement => {
  return (
    <Routes>
      <Route path="/" element={<Navigate replace to={ActiveTab[ActiveTab.home]} />} />
      <Route
        path="home"
        element={
          <Layout activeTab={ActiveTab.home}>
            <Home />
          </Layout>
        }
      />
      <Route
        path="teams"
        element={
          <Layout activeTab={ActiveTab.teams}>
            <Teams />
          </Layout>
        }
      />
      <Route
        path="seasons"
        element={
          <Layout activeTab={ActiveTab.seasons}>
            <Seasons />
          </Layout>
        }
      />
      <Route
        path="competitions"
        element={
          <Layout activeTab={ActiveTab.competitions}>
            <Competitions />
          </Layout>
        }
      />
      <Route
        path="*"
        element={
          <Layout activeTab={ActiveTab.unknown}>
            <NotFound />
          </Layout>
        }
      />
    </Routes>
  );
};

export { App };
