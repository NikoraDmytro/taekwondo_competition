import { useMemo } from "react";
import { useSearchParams } from "react-router-dom";

export const useObjectSearchParams = () => {
  const searchParams = useSearchParams()[0];

  const object = useMemo(() => {
    let object: Record<string, string> = {};

    searchParams.forEach((value, key) => {
      object[key] = value;
    });

    return object;
  }, [searchParams]);

  return object;
};
