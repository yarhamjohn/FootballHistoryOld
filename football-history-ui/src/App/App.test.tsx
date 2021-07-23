import React from "react";
import { render } from "@testing-library/react";
import App from "./App";
import reduxStore from "../reduxStore";
import { Provider } from "react-redux";

test("renders learn react link", () => {
  const { getByText } = render(
    <Provider store={reduxStore}>
      <App />
    </Provider>
  );
  const linkElement = getByText("History of the English Football League");
  expect(linkElement).toBeInTheDocument();
});
