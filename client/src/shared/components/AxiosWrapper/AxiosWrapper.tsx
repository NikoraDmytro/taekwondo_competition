import React from "react";
import { useAxios } from "shared/hooks/useAxios";
import { ErrorComponent } from "../ErrorComponent";
import { Loader } from "../Loader";

import styles from "./AxiosWrapper.module.scss";

interface Props<T> {
  noDataText: string;
  checkLength?: (data: T) => boolean;
  fetchData: () => Promise<T>;
  children: (props: {
    data: T;
    refresh: () => Promise<void>;
  }) => React.ReactNode;
}

export const AxiosWrapper = <T extends object>(props: Props<T>) => {
  const { loading, data, error, refresh } = useAxios(props.fetchData);

  const hasData =
    !!data &&
    (props.checkLength ? props.checkLength(data) : (data as any).length !== 0);

  return (
    <div className={styles.container}>
      {loading && <Loader small={hasData} />}
      {error && <ErrorComponent error={error} />}
      {!loading && !error && !hasData && (
        <h1 className={styles.noData}>{props.noDataText}</h1>
      )}

      {hasData && props.children({ data, refresh })}
    </div>
  );
};
