import React from "react";

import poster from "shared/assets/poster.jpg";

import styles from "./LandingPage.module.scss";

export const LandingPage = () => {
  return (
    <div>
      <img className={styles.poster} src={poster} alt="" />
    </div>
  );
};
