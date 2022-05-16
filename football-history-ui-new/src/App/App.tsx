import { FC, ReactElement } from "react";
import { Navigate, Route, Routes } from "react-router-dom";
import { Layout } from "./Components/Layout/Layout";
import { Competitions } from "./Pages/Competitions/Competitions";
import { Home } from "./Pages/Home/Home";
import { NotFound } from "./Pages/NotFound/NotFound";
import { Seasons } from "./Pages/Seasons/Seasons";
import { Teams } from "./Pages/Teams/Teams";

const App: FC = (): ReactElement => {
  return (
    <Layout>
      <Routes>
        <Route path="/" element={<Navigate replace to="home" />} />
        <Route path="/home" element={<Home />} />
        <Route path="/teams" element={<Teams />} />
        <Route path="/seasons" element={<Seasons />} />
        <Route path="/competitions" element={<Competitions />} />
        <Route path="*" element={<NotFound />} />
      </Routes>
    </Layout>
  );
};

export { App };
