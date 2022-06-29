import { Point } from "@nivo/line";

const useDrillDownTooltip = (points: Point[]): { date: string; position: string } => {
  const teamPoint = points[0];

  const date = new Date(teamPoint.data.xFormatted).toDateString();
  const position = `Position: ${teamPoint.data.yFormatted}`;

  return { date, position };
};

export { useDrillDownTooltip };
