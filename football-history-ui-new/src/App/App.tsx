import { Alert, CircularProgress, CssBaseline } from "@mui/material";
import { FC, ReactElement } from "react";
import { Navigate, Route, Routes } from "react-router-dom";
import { ActiveTab } from "./Components/Layout/AppHeader/TabBar/TabBar";
import { Layout } from "./Components/Layout/Layout";
import { useFetchTeams } from "./Hooks/useFetchTeams";
import { Competitions } from "./Pages/Competitions/Competitions";
import { Home } from "./Pages/Home/Home";
import { NotFound } from "./Pages/NotFound/NotFound";
import { Seasons } from "./Pages/Seasons/Seasons";
import { Teams } from "./Pages/Teams/Teams";
import { TeamsContextProvider } from "./Contexts/TeamsContext";
import { useFetchSeasons } from "./Hooks/useFetchSeasons";
import { SeasonsContextProvider } from "./Contexts/SeasonsContext";
import { CompetitionsContextProvider } from "./Contexts/CompetitionsContext";
import { useFetchCompetitions } from "./Hooks/useFetchCompetitions";

const App: FC = (): ReactElement => {
  const { teamsState } = useFetchTeams();
  const { seasonsState } = useFetchSeasons();
  const { competitionsState } = useFetchCompetitions();

  if (
    teamsState.status === "FETCH_NOT_STARTED" ||
    seasonsState.status === "FETCH_NOT_STARTED" ||
    competitionsState.status === "FETCH_NOT_STARTED"
  ) {
    return <></>;
  }

  if (
    teamsState.status === "FETCH_IN_PROGRESS" ||
    seasonsState.status === "FETCH_IN_PROGRESS" ||
    competitionsState.status === "FETCH_IN_PROGRESS"
  ) {
    return <CircularProgress />;
  }

  if (teamsState.status === "FETCH_ERROR") {
    return <Alert severity="error">{teamsState.error.message}</Alert>;
  }

  if (seasonsState.status === "FETCH_ERROR") {
    return <Alert severity="error">{seasonsState.error.message}</Alert>;
  }

  if (competitionsState.status === "FETCH_ERROR") {
    return <Alert severity="error">{competitionsState.error.message}</Alert>;
  }

  return (
    <TeamsContextProvider teams={teamsState.data}>
      <SeasonsContextProvider seasons={seasonsState.data}>
        <CompetitionsContextProvider competitions={competitionsState.data}>
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
