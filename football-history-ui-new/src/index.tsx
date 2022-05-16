import { createRoot } from "react-dom/client";
import { ColorModeContextProvider } from "./App/Contexts/ColorModeContext";
import { App } from "./App/App";

const rootElement = document.getElementById("root");

if (!rootElement) {
  throw new Error("Failed to find the root element");
}

const root = createRoot(rootElement);

root.render(
  <ColorModeContextProvider>
    <App />
  </ColorModeContextProvider>
);
