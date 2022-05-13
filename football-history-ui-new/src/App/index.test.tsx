import { render, screen } from "@testing-library/react";
import { App } from ".";

describe("App", () => {
  it("renders header", () => {
    render(<App />);

    expect(screen.getByText("Football History."));
  });
});
