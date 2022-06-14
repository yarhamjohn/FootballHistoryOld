import { Tabs, Tab, Alert, CircularProgress, Grid } from "@mui/material";
import { FC, ReactElement, useContext, useState } from "react";
import { LeagueTable } from "../../../Components/LeagueTable/LeagueTable";
import { SeasonsContext } from "../../../Contexts/SeasonsContext";
import { useFetchCompetitionsInSeason } from "../../../Hooks/useFetchCompetitionsInSeason";

const TabbedLeagues: FC = (): ReactElement => {
  const { activeSeason } = useContext(SeasonsContext);
  const [activeTab, setActiveTab] = useState<number>(0);

  const competitionsInSeason = useFetchCompetitionsInSeason(activeSeason.id);

  if (competitionsInSeason.isError) {
    return <Alert severity="error">{competitionsInSeason.error.message}</Alert>;
  }

  if (competitionsInSeason.isSuccess) {
    return (
      <Grid container justifyContent="center">
        <Grid item xs={2}>
          <Tabs
            value={activeTab}
            orientation="vertical"
            sx={{
              marginRight: "20%",
              position: "sticky",
              top: "5rem",
              borderRight: 1,
              borderColor: "divider"
            }}
          >
            {competitionsInSeason.data.map((x, i) => (
              <Tab
                key={x.id}
                value={i}
                label={x.name}
                onClick={() => setActiveTab(i)}
                wrapped={false}
              />
            ))}
          </Tabs>
        </Grid>
        <Grid item xs={6}>
          <LeagueTable competition={competitionsInSeason.data[activeTab]} />
        </Grid>
      </Grid>
    );
  }

  return <CircularProgress />;
};

export { TabbedLeagues };
