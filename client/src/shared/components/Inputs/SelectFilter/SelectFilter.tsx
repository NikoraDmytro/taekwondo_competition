import React, { ChangeEvent, useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";

import { DropDown } from "../DropDown";
import { Option } from "./../../../types/Option";

import styles from "./SelectFilter.module.scss";

interface Props {
  name: string;
  label: string;
  options: Option[];
}

const toTitleCase = (str: string) => {
  return str.charAt(0).toUpperCase() + str.substr(1);
};

export const SelectFilter = (props: Props) => {
  const [value, setValue] = useState("");
  const [searchParams, setSearchParams] = useSearchParams();

  const nameInTitleCase = toTitleCase(props.name);

  useEffect(() => {
    const search = searchParams.get(nameInTitleCase) ?? "";

    const option = props.options.find((opt) => opt.value === search);

    setValue(option?.label ?? "");
  }, [searchParams, props.options, setValue, nameInTitleCase]);

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
    <div className={styles.selectFilter}>
      <label>{props.label}</label>

      <DropDown value={value} onChange={handleChange} onBlur={handleBlur}>
        {props.options
          .filter((opt) => opt.label.includes(value))
          .map(renderOption)}
      </DropDown>
    </div>
  );
};
