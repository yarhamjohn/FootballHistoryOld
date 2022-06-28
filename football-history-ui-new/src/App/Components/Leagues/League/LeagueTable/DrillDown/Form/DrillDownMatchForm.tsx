import { blue, green, red } from "@mui/material/colors";
import { FC, ReactElement } from "react";
import { Match } from "../../../../../../Domain/Types";
import { useDrillDownMatchForm } from "../../../../../../Hooks/useDrillDownMatchForm";

type Props = { matches: Match[]; teamId: number };

const DrillDownMatchForm: FC<Props> = ({ matches, teamId }): ReactElement => {
  const { form } = useDrillDownMatchForm(matches, teamId);

  return (
    <div style={{ display: "flex", justifyContent: "space-evenly" }}>
      {form.map((result, i) => (
        <span
          key={i}
          style={{
            fontWeight: "bold",
            color: result === "W" ? green[500] : result === "L" ? red[500] : blue[500]
          }}
        >
          {result}
        </span>
      ))}
    </div>
  );
};

export { DrillDownMatchForm };
