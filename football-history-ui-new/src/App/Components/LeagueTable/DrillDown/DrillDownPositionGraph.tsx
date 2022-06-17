import { FC, ReactElement } from "react";
import { ResponsiveLine } from "@nivo/line";
import { CompetitionRules, LeaguePosition } from "../../../Domain/Types";
import { useDrillDownPositionGraph } from "../../../Hooks/useDrillDownPositionGraph";
import { DrillDownTooltip } from "./DrillDownTooltip";

type Props = { positions: LeaguePosition[]; rules: CompetitionRules };

const DrillDownPositionGraph: FC<Props> = ({ positions, rules }): ReactElement => {
  const { data, colors, gridYValues, gridXValues, theme } = useDrillDownPositionGraph(
    positions,
    rules,
    rules.totalPlaces
  );

  return (
    <div style={{ position: "absolute", top: 0, left: 0, height: "100%", width: "100%" }}>
      <ResponsiveLine
        data={data}
        theme={theme}
        colors={colors}
        margin={{ left: 25, bottom: 10, top: 10 }}
        yScale={{ type: "linear", min: 1, max: rules.totalPlaces, reverse: true }}
        enablePoints={false}
        gridYValues={gridYValues}
        gridXValues={gridXValues}
        axisBottom={null}
        enableSlices="x"
        sliceTooltip={({ slice }) => <DrillDownTooltip points={slice.points} />}
      />
    </div>
  );
};

export { DrillDownPositionGraph };
