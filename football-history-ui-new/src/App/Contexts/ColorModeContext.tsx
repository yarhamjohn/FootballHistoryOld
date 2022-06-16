import { FC, ReactNode, ReactElement, createContext, useState, useMemo } from "react";
import { ThemeProvider, createTheme } from "@mui/material/styles";
import { useMediaQuery } from "@mui/material";
import { amber } from "@mui/material/colors";

// eslint-disable-next-line @typescript-eslint/no-empty-function
const ColorModeContext = createContext({ colorMode: { toggleColorMode: () => {} }, mode: "light" });

type Props = {
  children?: ReactNode;
};

const ColorModeContextProvider: FC<Props> = ({ children }): ReactElement => {
  const [mode, setMode] = useState<"light" | "dark">(
    useMediaQuery("(prefers-color-scheme: dark)") ? "dark" : "light"
  );

  const colorMode = useMemo(
    () => ({
      toggleColorMode: () => {
        setMode((prevMode) => (prevMode === "light" ? "dark" : "light"));
      }
    }),
    []
  );

  const theme = useMemo(
    () =>
      createTheme({
        palette: { mode, secondary: { main: amber[500] } }
      }),
    [mode]
  );

  return (
    <ColorModeContext.Provider value={{ colorMode, mode }}>
      <ThemeProvider theme={theme}>{children}</ThemeProvider>
    </ColorModeContext.Provider>
  );
};

export { ColorModeContext, ColorModeContextProvider };
