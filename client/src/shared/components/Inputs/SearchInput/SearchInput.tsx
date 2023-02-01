import React, { ChangeEvent } from "react";
import { useSearchParams } from "react-router-dom";

import styles from "./SearchInput.module.scss";

interface Props {
  placeholder?: string;
}

export const SearchInput = (props: Props) => {
  const [searchParams, setSearchParams] = useSearchParams();

  const search = searchParams.get("search") ?? "";

  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;

    if (value === "") {
      searchParams.delete("search");
    } else {
      searchParams.set("search", value);
    }

    setSearchParams(searchParams);
  };

  return (
    <div className={styles.search}>
      <label>Пошук</label>

      <input
        type="search"
        value={search}
        placeholder={props.placeholder}
        onChange={handleChange}
      />
    </div>
  );
};
