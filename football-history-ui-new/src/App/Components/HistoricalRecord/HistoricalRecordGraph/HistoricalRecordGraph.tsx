import { FC, ReactElement, useContext } from "react";
import { Datum, ResponsiveLine } from "@nivo/line";
import { SeasonsContext } from "../../../Contexts/SeasonsContext";
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
  const { seasons, firstSeason, lastSeason } = useContext(SeasonsContext);
  const { series, colors, yValues, getTheme } = useHistoricalRecordGraph(
    historicalSeasons,
    selectedRange
  );

  const DashedLine = ({
    series,
    lineGenerator,
    xScale,
    yScale
  }: {
    series: any;
    lineGenerator: any;
    xScale: any;
    yScale: any;
  }) => {
    return series.map(({ id, data, color }: { id: any; data: Datum[]; color: any }) => (
      <>
        <path
          key={`point-${id}`}
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
      </>
    ));
  };

  const ScatterCircle = ({ series, xScale, yScale }: { series: any; xScale: any; yScale: any }) => {
    return (
      <>
        {series.map(({ id, data, color }: { id: any; data: Datum[]; color: any }) => {
          return id === "positions"
            ? data.map((s: Datum) => {
                return s.data.y === null ? (
                  <></>
                ) : (
                  <circle
                    key={`circle-${s.data.x}-${s.data.y}-${id}`}
                    cx={xScale(s.data.x) ?? null}
                    cy={yScale(s.data.y) ?? null}
                    r={4}
                    fill={getLeagueStatusColor(s.data.status) ?? color}
                    stroke={getLeagueStatusColor(s.data.status) ?? color}
                    style={{ pointerEvents: "none" }}
                  />
                );
              })
            : [];
        })}
      </>
    );
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
          layers={["grid", "axes", DashedLine, ScatterCircle, "crosshair", "slices"]}
        />
      </CardContent>
    </Card>
  );
};

export { HistoricalRecordGraph };
