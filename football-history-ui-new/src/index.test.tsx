import { ReactElement } from "react";

describe("Application root", () => {
  let mockRender: jest.Mock;
  let mockCreateRoot: jest.Mock;
  const getElementByIdSpy = jest.spyOn(document, "getElementById");

  beforeEach(() => {
    jest.resetModules();

    mockRender = jest.fn();
    jest.mock("react-dom/client", () => ({
      createRoot: jest.fn().mockReturnValue({ render: (x: ReactElement) => mockRender(x) })
    }));

    /* eslint-disable-next-line @typescript-eslint/no-var-requires */
    mockCreateRoot = require("react-dom/client").createRoot;
  });

  it("should render without crashing", () => {
    const div = document.createElement("div");
    getElementByIdSpy.mockReturnValue(div);

    require("./index.tsx");

    expect(mockCreateRoot).toHaveBeenCalledWith(div);
    expect(mockRender).toHaveBeenCalled();
  });

  it("should throw given no matching document element", () => {
    getElementByIdSpy.mockReturnValue(null);

    expect(() => {
      require("./index.tsx");
    }).toThrow();

    expect(mockCreateRoot).not.toHaveBeenCalled();
    expect(mockRender).not.toHaveBeenCalled();
  });
});
