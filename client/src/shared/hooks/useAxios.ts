import { useCallback, useEffect, useState } from "react";

export const useAxios = <TReturn>(loadData: () => Promise<TReturn>) => {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState<TReturn | null>(null);
  const [error, setError] = useState<Error | null>(null);

  const fetchData = useCallback(async () => {
    try {
      setData(null);
      setLoading(true);

      const response = await loadData();

      setError(null);
      setData(response);
    } catch (err) {
      setError(err as Error);
    } finally {
      setLoading(false);
    }
  }, [loadData]);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  return { data, loading, error, refresh: fetchData };
};
