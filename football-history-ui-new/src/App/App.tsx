import { Alert, CircularProgress, CssBaseline } from "@mui/material";
import { FC, ReactElement } from "react";
import { Navigate, Route, Routes } from "react-router-dom";
import { ActiveTab } from "./Components/Layout/AppHeader/TabBar/TabBar";
import { Layout } from "./Components/Layout/Layout";
import { Competitions } from "./Pages/Competitions/Competitions";
import { Home } from "./Pages/Home/Home";
import { NotFound } from "./Pages/NotFound/NotFound";
import { Seasons } from "./Pages/Seasons/Seasons";
import { Teams } from "./Pages/Teams/Teams";
import { TeamsContextProvider } from "./Contexts/TeamsContext";
import { SeasonsContextProvider } from "./Contexts/SeasonsContext";
import { CompetitionsContextProvider } from "./Contexts/CompetitionsContext";
import { useApp } from "./useApp";

const App: FC = (): ReactElement => {
  const state = useApp();

  if (state.status === "LOADING") {
    return <CircularProgress />;
  }

  if (state.status === "LOAD_FAILED") {
    return <Alert severity="error">{state.error.message}</Alert>;
  }

  return (
    <TeamsContextProvider teams={state.data.teams}>
      <SeasonsContextProvider seasons={state.data.seasons}>
        <CompetitionsContextProvider competitions={state.data.competitions}>
          <CssBaseline>
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
          </CssBaseline>
        </CompetitionsContextProvider>
      </SeasonsContextProvider>
    </TeamsContextProvider>
  );
};

export { App };
