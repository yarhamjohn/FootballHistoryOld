import { FC, ReactElement, useContext } from "react";
import { ResponsiveLine } from "@nivo/line";
import { SeasonsContext } from "../../../Contexts/SeasonsContext";
import { useHistoricalRecordGraph } from "./useHistoricalRecordGraph";
import { HistoricalSeason } from "../../../Domain/Types";
import { Box } from "@mui/material";

type HistoricalRecordGraphProps = {
  historicalSeasons: HistoricalSeason[];
  historicalRecordRange: number[];
};

const HistoricalRecordGraph: FC<HistoricalRecordGraphProps> = ({
  historicalSeasons,
  historicalRecordRange
}): ReactElement => {
  const { seasons, firstSeason, lastSeason } = useContext(SeasonsContext);
  const { series, colors, yValues } = useHistoricalRecordGraph(
    historicalSeasons,
    historicalRecordRange
  );

  return (
    <div style={{ height: "600px", position: "relative", width: "50%" }}>
      <div style={{ position: "absolute", top: 0, left: 0, height: "100%", width: "100%" }}>
        <ResponsiveLine
          data={series}
          colors={colors}
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
            return <p>here</p>; //<Tooltip points={slice.points} id={slice.id} seasons={seasons} />;
          }}
          axisBottom={{
            tickSize: 5,
            tickPadding: 5,
            tickRotation: 0
          }}
        />
      </div>{" "}
    </div>
  );
};

export { HistoricalRecordGraph };
