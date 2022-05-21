import { Reducer, useReducer } from "react";
import { FetchAction, fetchReducer, FetchState } from "./fetchReducer";

const useFetch = <T,>() => {
  const [state, dispatch] = useReducer<Reducer<FetchState<T>, FetchAction<T>>>(fetchReducer, {
    status: "FETCH_NOT_STARTED"
  });

  const callApi = (url: string) => {
    dispatch({ type: "FETCH_STARTED" });
    fetch(url)
      .then((response) => {
        if (!response.ok) {
          throw new Error(`Something went wrong. ${response.statusText}`);
        }

        return response.json();
      })
      .then((json) => dispatch({ type: "FETCH_SUCCEEDED", data: json }))
      .catch((error) => dispatch({ type: "FETCH_FAILED", error }));
  };

  return { state, callApi };
};

export { useFetch };
