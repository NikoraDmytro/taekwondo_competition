import React, { ChangeEvent } from "react";
import { useSearchParams } from "react-router-dom";

import styles from "./NumberFilter.module.scss";

interface Props {
  name: string;
  label: string;
  placeholder?: string;
}

const toTitleCase = (str: string) => {
  return str.charAt(0).toUpperCase() + str.substr(1);
};

export const NumberFilter = (props: Props) => {
  const [searchParams, setSearchParams] = useSearchParams();

  const nameInTitleCase = toTitleCase(props.name);
  const number = searchParams.get(nameInTitleCase) ?? "";

  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;

    if (value === "") {
      searchParams.delete(nameInTitleCase);
    } else {
      searchParams.set(nameInTitleCase, value);
    }

    setSearchParams(searchParams);
  };

  return (
    <div className={styles.search}>
      <label>{props.label}</label>

      <input
        type="number"
        value={number}
        placeholder={props.placeholder}
        onChange={handleChange}
      />
    </div>
  );
};
