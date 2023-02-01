import React from "react";
import cl from "classnames";
import { NavLink } from "react-router-dom";

import logo from "shared/assets/logo.png";
import profileIcon from "shared/assets/profileIcon.png";

import styles from "./Header.module.scss";

type NavLinkProps = {
  href: string;
  text: string;
};

const NavBarLink = (props: NavLinkProps) => {
  return (
    <li className={styles.navBarLink}>
      <NavLink
        className={({ isActive }) => cl({ [styles.active]: isActive })}
        to={props.href}
      >
        {props.text}
      </NavLink>
    </li>
  );
};

export const Header = () => {
  const isRegistered = true;

  return (
    <header className={styles.header}>
      <div className={styles.logo}>
        <img src={logo} alt="" />
      </div>

      <ul className={styles.navBar}>
        <NavBarLink href="/clubs" text="Клуби" />
        <NavBarLink href="/calendar" text="Календар" />
      </ul>

      <div className={styles.profileTooltip}>
        <div className={styles.tooltipLabel}>
          <div className={styles.profileIcon}>
            <img src={profileIcon} alt="" />
          </div>
          <p>Кабінет</p>
        </div>

        <div className={styles.tooltipContent}>
          {isRegistered && (
            <ul className={styles.profileOptions}>
              <li>Налаштування</li>
              <li>Вийти</li>
            </ul>
          )}
        </div>
      </div>
    </header>
  );
};
