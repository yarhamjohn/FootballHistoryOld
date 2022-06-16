import { Tabs, Tab, SxProps, Theme } from "@mui/material";
import { FC, ReactElement } from "react";
import { Competition } from "../../../Domain/Types";

type Props = {
  activeTab: number;
  setActiveTab: (activeTab: number) => void;
  competitions: Competition[];
  size: "small" | "large";
};

const LeagueTabs: FC<Props> = ({ activeTab, setActiveTab, competitions, size }): ReactElement => {
  const sx: SxProps<Theme> =
    size === "large"
      ? {
          marginRight: "20%",
          position: "sticky",
          top: "5rem",
          borderRight: 1,
          borderColor: "divider"
        }
      : {
          marginBottom: "2rem",
          borderColor: "divider"
        };

  return (
    <Tabs value={activeTab} orientation={size === "large" ? "vertical" : "horizontal"} sx={sx}>
      {competitions.map((x, i) => (
        <Tab key={x.id} value={i} label={x.name} onClick={() => setActiveTab(i)} wrapped={false} />
      ))}
    </Tabs>
  );
};

export { LeagueTabs };
