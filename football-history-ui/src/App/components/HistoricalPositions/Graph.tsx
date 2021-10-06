import { FunctionComponent } from "react";
import { ResponsiveLine } from "@nivo/line";
import { Tooltip } from "./Tooltip";
import { Loader } from "semantic-ui-react";
import { useHistoricalPositionsGraph } from "./useHistoricalPositionsGraph";
import { SeasonDateRange } from "./index";
import { HistoricalSeason } from "../../shared/useFetchHistoricalRecord";

const HistoricalPositionsGraph: FunctionComponent<{
  isLoading: boolean;
  seasons: HistoricalSeason[];
  range: SeasonDateRange;
}> = ({ isLoading, seasons: seasons, range }) => {
  const { series, colors, yValues } = useHistoricalPositionsGraph(seasons, range);

  return (
    <div style={{ height: "600px", position: "relative" }}>
      {isLoading && <Loader active style={{ position: "absolute", top: "50%", left: "50%" }} />}
      <div style={{ position: "absolute", top: 0, left: 0, height: "100%", width: "100%" }}>
        <ResponsiveLine
          data={series}
          colors={colors}
          margin={{ left: 25, bottom: 25, top: 10 }}
          yScale={{
            type: "linear",
            min: Math.min(...yValues),
            max: Math.max(...yValues),
            reverse: true,
          }}
          gridYValues={yValues}
          enableSlices="x"
          sliceTooltip={({ slice }) => {
            return <Tooltip points={slice.points} id={slice.id} seasons={seasons} />;
          }}
          axisBottom={{
            tickSize: 5,
            tickPadding: 5,
            tickRotation: 0,
          }}
        />
      </div>
    </div>
  );
};

export { HistoricalPositionsGraph };
