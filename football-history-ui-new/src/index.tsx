import { createRoot } from "react-dom/client";
import { ColorModeContextProvider } from "./App/Contexts/ColorModeContext";
import { Layout } from "./App/Components";
import { App } from "./App";

const rootElement = document.getElementById("root");

if (!rootElement) {
  throw new Error("Failed to find the root element");
}

const root = createRoot(rootElement);

root.render(
  <ColorModeContextProvider>
    <Layout>
      <App />
    </Layout>
  </ColorModeContextProvider>
);
