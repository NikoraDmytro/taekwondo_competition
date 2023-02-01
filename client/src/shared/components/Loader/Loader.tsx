import classNames from "classnames";
import React from "react";

import styles from "./Loader.module.scss";

interface Props {
  small?: boolean;
}

export const Loader = ({ small }: Props) => {
  const className = classNames(styles.loader, {
    [styles.small]: small,
  });

  return <div className={className}></div>;
};
