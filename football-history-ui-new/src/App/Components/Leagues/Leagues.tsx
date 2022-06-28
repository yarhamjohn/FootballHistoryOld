import { Alert, CircularProgress, Grid, Box } from "@mui/material";
import { FC, ReactElement, useContext, useState } from "react";
import { SeasonsContext } from "../../Contexts/SeasonsContext";
import { useFetchCompetitionsInSeason } from "../../Hooks/useFetchCompetitionsInSeason";
import useMediaQuery from "@mui/material/useMediaQuery";
import { LeaguesMenu } from "./Menu/LeaguesMenu";
import { League } from "./League/League";

const Leagues: FC = (): ReactElement => {
  const { activeSeason } = useContext(SeasonsContext);
  const [activeTab, setActiveTab] = useState<number>(0);
  const largeScreen = useMediaQuery("(min-width:1400px)");

  const competitionsInSeason = useFetchCompetitionsInSeason(activeSeason.id);

  if (competitionsInSeason.isError) {
    return <Alert severity="error">{competitionsInSeason.error.message}</Alert>;
  }

  if (competitionsInSeason.isSuccess) {
    if (largeScreen) {
      return (
        <Grid container justifyContent="center">
          <Grid item xs={2}>
            <LeaguesMenu
              activeTab={activeTab}
              setActiveTab={setActiveTab}
              competitions={competitionsInSeason.data}
              size="large"
            />
          </Grid>
          <Grid item xs={10}>
            <League competition={competitionsInSeason.data[activeTab]} size="large" />
          </Grid>
        </Grid>
      );
    } else {
      return (
        <Box
          sx={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center"
          }}
        >
          <LeaguesMenu
            activeTab={activeTab}
            setActiveTab={setActiveTab}
            competitions={competitionsInSeason.data}
            size="small"
          />
          <League competition={competitionsInSeason.data[activeTab]} size="small" />
        </Box>
      );
    }
  }

  return <CircularProgress />;
};

export { Leagues };
