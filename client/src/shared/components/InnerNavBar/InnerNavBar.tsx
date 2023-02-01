import React from "react";
import classNames from "classnames";
import { NavLink } from "react-router-dom";

import styles from "./InnerNavBar.module.scss";

interface Props {
  innerLinks: {
    to: string;
    title: string;
  }[];
}

export const InnerNavBar = ({ innerLinks }: Props) => (
  <ul className={styles.navigationList}>
    {innerLinks.map((innerLink) => (
      <li key={innerLink.title} className={styles.listItem}>
        <NavLink
          className={({ isActive }) =>
            classNames({
              [styles.activeLink]: isActive,
            })
          }
          to={innerLink.to}
        >
          {innerLink.title}
        </NavLink>
      </li>
    ))}
  </ul>
);
