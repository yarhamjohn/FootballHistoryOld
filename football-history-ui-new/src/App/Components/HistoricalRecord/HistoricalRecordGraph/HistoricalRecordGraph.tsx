import { FC, ReactElement, useContext } from "react";
import { ResponsiveLine } from "@nivo/line";
import { SeasonsContext } from "../../../Contexts/SeasonsContext";
import { HistoricalSeason } from "../../../Domain/Types";
import Card from "@mui/material/Card/Card";
import CardContent from "@mui/material/CardContent/CardContent";
import { useHistoricalRecordGraph } from "../../../Hooks/useHistoricalRecordGraph";
import { Tooltip } from "./Tooltip";
import { ColorModeContext } from "../../../Contexts/ColorModeContext";

type HistoricalRecordGraphProps = {
  historicalSeasons: HistoricalSeason[];
  selectedRange: number[];
};

const HistoricalRecordGraph: FC<HistoricalRecordGraphProps> = ({
  historicalSeasons,
  selectedRange
}): ReactElement => {
  const { seasons, firstSeason, lastSeason } = useContext(SeasonsContext);
  const { series, colors, yValues } = useHistoricalRecordGraph(historicalSeasons, selectedRange);
  const { mode } = useContext(ColorModeContext);

  const getTheme = () => {
    if (mode === "light") return;
    return {
      textColor: "#999",
      grid: {
        line: {
          stroke: "#333"
        }
      },
      crosshair: { line: { stroke: "#999" } }
    };
  };

  return (
    <Card style={{ height: "40rem", position: "relative", width: "75%" }}>
      <CardContent style={{ position: "absolute", top: 0, left: 0, height: "100%", width: "100%" }}>
        <ResponsiveLine
          data={series}
          colors={colors}
          theme={getTheme()}
          curve={"monotoneX"}
          margin={{ left: 25, bottom: 25, top: 10 }}
          yScale={{
            type: "linear",
            min: Math.min(...yValues),
            max: Math.max(...yValues),
            reverse: true
          }}
          gridYValues={yValues}
          xScale={{
            type: "linear",
            min: firstSeason.startYear - 1,
            max: lastSeason.startYear + 1
          }}
          gridXValues={seasons.map((x) => x.startYear)}
          enableSlices="x"
          sliceTooltip={({ slice }) => {
            return <Tooltip points={slice.points} seasons={historicalSeasons} />;
          }}
          axisBottom={{
            tickSize: 5,
            tickPadding: 5,
            tickRotation: 0
          }}
        />
      </CardContent>
    </Card>
  );
};

export { HistoricalRecordGraph };
