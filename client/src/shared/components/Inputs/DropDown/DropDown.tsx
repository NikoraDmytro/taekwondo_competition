import React, { InputHTMLAttributes, useState } from "react";
import classNames from "classnames";

import styles from "./DropDown.module.scss";

interface Props extends InputHTMLAttributes<HTMLInputElement> {
  children: React.ReactNode;
}

export const DropDown = ({ children, ...props }: Props) => {
  const [active, setActive] = useState(false);

  const handleFocus = () => {
    setActive(true);
  };

  const handleBlur = () => {
    setActive(false);
  };

  const className = classNames({
    [styles.dropDownMenu]: true,
    [styles.active]: active,
  });

  return (
    <div
      tabIndex={5}
      onBlur={handleBlur}
      onFocus={handleFocus}
      className={styles.dropDown}
    >
      <input autoComplete="off" {...props} />

      <ul onClick={handleBlur} className={className}>
        {children}
      </ul>
    </div>
  );
};
