import { IconButton } from "@mui/material";
import SvgIcon from "@mui/material/SvgIcon";

const HomeButton = () => {
  return (
    <IconButton color={"primary"}>
      <SvgIcon>
        <path d="M10 20v-6h4v6h5v-8h3L12 3 2 12h3v8z" />
      </SvgIcon>
    </IconButton>
  );
};

export { HomeButton };
