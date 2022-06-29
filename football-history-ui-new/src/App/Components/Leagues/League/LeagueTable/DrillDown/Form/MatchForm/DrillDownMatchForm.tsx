import { FC, ReactElement } from "react";
import { Match } from "../../../../../../../Domain/Types";
import { useDrillDownMatchForm } from "./useDrillDownMatchForm";

type Props = { matches: Match[]; teamId: number };

const DrillDownMatchForm: FC<Props> = ({ matches, teamId }): ReactElement => {
  const { form } = useDrillDownMatchForm(matches, teamId);

  return (
    <div style={{ display: "flex", justifyContent: "space-evenly" }}>
      {form.map((result, i) => (
        <span
          title={result.title}
          key={i}
          style={{ fontWeight: "bold", cursor: "default", color: result.color }}
        >
          {result.outcome}
        </span>
      ))}
    </div>
  );
};

export { DrillDownMatchForm };
