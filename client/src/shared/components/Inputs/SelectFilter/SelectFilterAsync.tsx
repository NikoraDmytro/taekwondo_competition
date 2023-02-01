import React, { ChangeEvent, useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";

import { Loader } from "shared/components/Loader";
import { useAxios } from "shared/hooks/useAxios";

import styles from "./SelectFilter.module.scss";
import { DropDown } from "../DropDown";
import { Option } from "shared/types/Option";
import { ErrorComponent } from "shared/components/ErrorComponent";

interface Props {
  name: string;
  label: string;
  request: (input: string) => Promise<Option[]>;
}

const toTitleCase = (str: string) => {
  return str.charAt(0).toUpperCase() + str.substr(1);
};

export const SelectFilterAsync = (props: Props) => {
  const [value, setValue] = useState("");
  const [searchParams, setSearchParams] = useSearchParams();

  const nameInTitleCase = toTitleCase(props.name);

  useEffect(() => {
    const search = searchParams.get(nameInTitleCase) ?? "";

    setValue(search);
  }, [searchParams, setValue, nameInTitleCase]);

  const { data, loading, error } = useAxios<Option[]>(() =>
    props.request(value)
  );

  const selectOption = (option: string) => () => {
    searchParams.set(nameInTitleCase, option);
    setSearchParams(searchParams);
  };

  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    setValue(e.target.value);
  };

  const handleBlur = () => {
    if (value === "") {
      searchParams.delete(nameInTitleCase);
    } else {
      searchParams.set(nameInTitleCase, value);
    }

    setSearchParams(searchParams);
  };

  const renderOption = (option: Option) => (
    <li key={option.label} onClick={selectOption(option.value)}>
      {option.label}
    </li>
  );

  return (
    <div className={styles.filterSelect}>
      <label>{props.label}</label>

      <DropDown value={value} onChange={handleChange} onBlur={handleBlur}>
        {!loading && data && data.length ? (
          data.map(renderOption)
        ) : (
          <li>
            {loading && <Loader small />}
            {error && <ErrorComponent error={error} short />}
            {data && !data.length && <p>Нічого не знайдено!</p>}
          </li>
        )}
      </DropDown>
    </div>
  );
};
