import { FC, ReactElement } from "react";
import { ResponsiveLine } from "@nivo/line";
import { HistoricalSeason } from "../../../Domain/Types";
import Card from "@mui/material/Card/Card";
import CardContent from "@mui/material/CardContent/CardContent";
import { useHistoricalRecordGraph } from "../../../Hooks/useHistoricalRecordGraph";
import { Tooltip } from "./Tooltip";

type HistoricalRecordGraphProps = {
  historicalSeasons: HistoricalSeason[];
  selectedRange: number[];
};

const HistoricalRecordGraph: FC<HistoricalRecordGraphProps> = ({
  historicalSeasons,
  selectedRange
}): ReactElement => {
  const { series, yValues, xValues, theme, customLine, customPoint } = useHistoricalRecordGraph(
    historicalSeasons,
    selectedRange
  );

  return (
    <Card style={{ height: "40rem", position: "relative", width: "75%" }}>
      <CardContent style={{ position: "absolute", top: 0, left: 0, height: "100%", width: "100%" }}>
        <ResponsiveLine
          data={series}
          colors={series.map((s) => s.color)}
          theme={theme}
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
            min: Math.min(...selectedRange) - 1,
            max: Math.max(...selectedRange) + 1
          }}
          gridXValues={xValues}
          enableSlices="x"
          sliceTooltip={({ slice }) => {
            return <Tooltip points={slice.points} seasons={historicalSeasons} />;
          }}
          axisBottom={{
            tickSize: 5,
            tickPadding: 5,
            tickRotation: 0
          }}
          layers={["grid", "axes", customLine, customPoint, "crosshair", "slices"]}
        />
      </CardContent>
    </Card>
  );
};

export { HistoricalRecordGraph };
