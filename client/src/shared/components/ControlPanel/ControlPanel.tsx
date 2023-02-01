import React from "react";
import cl from "classnames";

import { SearchInput } from "../Inputs/SearchInput";

import styles from "./ControlPanel.module.scss";

interface Props {
  searchPlaceholder?: string;
  filters?: React.ReactNode;
  buttons?: React.ReactNode;
}

export const ControlPanel = (props: Props) => {
  return (
    <div className={styles.controlPanel}>
      <div className={styles.controlPanelBox}>
        <SearchInput placeholder={props.searchPlaceholder} />

        <div className={styles.filtersBlock}>{props.filters}</div>
      </div>

      <div className={cl(styles.buttonsBlock, styles.controlPanelBox)}>
        {props.buttons}
      </div>
    </div>
  );
};
