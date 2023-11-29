import { useEffect, useState } from "react";

export type FetchResult<T> =
  | {
      status: "UNLOADED";
    }
  | { status: "LOADING" }
  | { status: "LOAD_SUCCESSFUL"; data: T }
  | { status: "LOAD_FAILED"; error: string };

async function callApi<T>(url: string, signal: AbortSignal): Promise<T> {
  return fetch(url, { signal })
    .then((response) => {
      if (!response.ok) {
        throw new Error(response.statusText);
      }
      return response.json() as Promise<T>;
    })
    .then((result) => {
      return result;
    });
}

function useFetch<T>(url: string): FetchResult<T> {
  const [result, setResult] = useState<FetchResult<T>>({ status: "UNLOADED" });

  useEffect(() => {
    if (url === "") {
      return;
    }

    const abortController = new AbortController();

    setResult({ status: "LOADING" });

    callApi<T>(url, abortController.signal)
      .then((data: T) => setResult({ status: "LOAD_SUCCESSFUL", data }))
      .catch((error: Error) => {
        if (!abortController.signal.aborted) {
          setResult({ status: "LOAD_FAILED", error: error.message });
        }
      });

    return () => {
      abortController.abort();
    };
  }, [url]);

  return result;
}

export { useFetch, callApi };
