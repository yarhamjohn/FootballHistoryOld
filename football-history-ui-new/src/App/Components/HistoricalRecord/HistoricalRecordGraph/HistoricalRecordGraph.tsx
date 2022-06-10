import { FC, ReactElement } from "react";
import { CustomLayerProps, Datum, ResponsiveLine } from "@nivo/line";
import { HistoricalSeason } from "../../../Domain/Types";
import Card from "@mui/material/Card/Card";
import CardContent from "@mui/material/CardContent/CardContent";
import { useHistoricalRecordGraph } from "../../../Hooks/useHistoricalRecordGraph";
import { Tooltip } from "./Tooltip";
import { getLeagueStatusColor } from "./TooltipContent";

type HistoricalRecordGraphProps = {
  historicalSeasons: HistoricalSeason[];
  selectedRange: number[];
};

const HistoricalRecordGraph: FC<HistoricalRecordGraphProps> = ({
  historicalSeasons,
  selectedRange
}): ReactElement => {
  const { series, yValues, xValues, getTheme } = useHistoricalRecordGraph(
    historicalSeasons,
    selectedRange
  );

  const CustomLine = ({ series, lineGenerator, xScale, yScale }: CustomLayerProps) =>
    series.map(({ id, data, color }) => (
      <path
        key={`line-${id}`}
        d={lineGenerator(
          data.map((d: Datum) => {
            return {
              x: xScale(d.data.x) ?? null,
              y: yScale(d.data.y) ?? null
            };
          })
        )}
        fill="none"
        stroke={color}
        style={{ strokeWidth: 1 }}
      />
    ));

  const CustomPoint = ({ series, xScale, yScale }: CustomLayerProps) =>
    series.map(({ id, data, color }) =>
      id === "positions"
        ? data.map((d: Datum) =>
            d.data.y === null ? null : (
              <circle
                key={`point-${d.data.x}-${d.data.y}-${id}`}
                cx={Number(xScale(d.data.x))}
                cy={Number(yScale(d.data.y))}
                r={4}
                fill={getLeagueStatusColor(d.data.status) ?? color}
                stroke={getLeagueStatusColor(d.data.status) ?? color}
                style={{ pointerEvents: "none" }}
              />
            )
          )
        : []
    );

  return (
    <Card style={{ height: "40rem", position: "relative", width: "75%" }}>
      <CardContent style={{ position: "absolute", top: 0, left: 0, height: "100%", width: "100%" }}>
        <ResponsiveLine
          data={series}
          colors={series.map((s) => s.color)}
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
          layers={["grid", "axes", CustomLine, CustomPoint, "crosshair", "slices"]}
        />
      </CardContent>
    </Card>
  );
};

export { HistoricalRecordGraph };
