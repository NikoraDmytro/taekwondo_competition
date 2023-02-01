import React from "react";

import styles from "./CheckBox.module.scss";

interface Props {
  id: string;
  checked: boolean;
  toggle: () => void;
}

export const CheckBox = ({ id, checked, toggle }: Props) => {
  return (
    <label htmlFor={id} className={styles.checkbox}>
      <input id={id} type="checkbox" onChange={toggle} checked={checked} />

      <span className={styles.checkmark} />
    </label>
  );
};
