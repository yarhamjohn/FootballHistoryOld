type FetchAction<T> =
  | {
      type: "FETCH_STARTED";
    }
  | { type: "FETCH_SUCCEEDED"; data: T }
  | { type: "FETCH_FAILED"; error: Error };

type FetchState<T> =
  | {
      status: "FETCH_NOT_STARTED";
    }
  | { status: "FETCH_IN_PROGRESS" }
  | { status: "FETCH_SUCCESS"; data: T }
  | { status: "FETCH_ERROR"; error: Error };

const fetchReducer = <T>(state: FetchState<T>, action: FetchAction<T>): FetchState<T> => {
  switch (action.type) {
    case "FETCH_STARTED":
      return { status: "FETCH_IN_PROGRESS" };
    case "FETCH_SUCCEEDED":
      return { status: "FETCH_SUCCESS", data: action.data };
    case "FETCH_FAILED":
      return { status: "FETCH_ERROR", error: action.error };
  }
};

export { fetchReducer, FetchAction, FetchState };
