import { createRoot } from "react-dom/client";
import { ColorModeContextProvider } from "./App/Contexts/ColorModeContext";
import { App } from "./App/App";
import { BrowserRouter } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "react-query";

const rootElement = document.getElementById("root");

if (!rootElement) {
  throw new Error("Failed to find the root element");
}

const root = createRoot(rootElement);

const queryClient = new QueryClient();

root.render(
  <ColorModeContextProvider>
    <QueryClientProvider client={queryClient}>
      <BrowserRouter>
        <App />
      </BrowserRouter>
    </QueryClientProvider>
  </ColorModeContextProvider>
);
