import Card from "@mui/material/Card/Card";
import CardContent from "@mui/material/CardContent/CardContent";
import Typography from "@mui/material/Typography/Typography";
import { FC, ReactElement } from "react";

const NotFound: FC = (): ReactElement => {
  return (
    <Card variant="outlined">
      <CardContent>
        <Typography gutterBottom variant="h5">
          Page not found :(
        </Typography>
        <Typography variant="body1">
          The page you are looking for might have been removed, or the url is invalid.
        </Typography>
      </CardContent>
    </Card>
  );
};

export { NotFound };
