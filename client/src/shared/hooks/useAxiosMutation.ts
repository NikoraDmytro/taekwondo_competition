import { useState } from "react";

export const useAxiosMutation = <TReturn>(
  request: (...values: any[]) => Promise<TReturn>
) => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<Error | null>(null);

  const mutation = async (...values: any[]): Promise<boolean> => {
    try {
      setLoading(true);

      await request(...values);

      setError(null);
      return true;
    } catch (err) {
      setError(err as Error);
      return false;
    } finally {
      setLoading(false);
    }
  };

  return { mutation, loading, error };
};
