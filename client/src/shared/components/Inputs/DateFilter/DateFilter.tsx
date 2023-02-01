import React, { useEffect, useState } from "react";
import DatePicker from "react-datepicker";
import { format, parse } from "date-fns";
import { useSearchParams } from "react-router-dom";
import "react-datepicker/dist/react-datepicker.css";

import styles from "./DateFilter.module.scss";

interface Props {
  name: string;
  label: string;
}

const toTitleCase = (str: string) => {
  return str.charAt(0).toUpperCase() + str.substr(1);
};

export const DateFilter = (props: Props) => {
  const [value, setValue] = useState<Date | null>();
  const [searchParams, setSearchParams] = useSearchParams();

  const nameInTitleCase = toTitleCase(props.name);

  const handleChange = (date: Date | null) => {
    if (!date) {
      searchParams.delete(nameInTitleCase);
    } else {
      searchParams.set(nameInTitleCase, format(date, "yyyy-MM-dd"));
    }

    setSearchParams(searchParams);
  };

  useEffect(() => {
    const dateString = searchParams.get(nameInTitleCase);
    const date = dateString
      ? parse(dateString, "yyyy-MM-dd", new Date())
      : null;

    setValue(date);
  }, [searchParams, setValue, nameInTitleCase]);

  return (
    <div className={styles.dateFilter}>
      <label>{props.label}</label>

      <DatePicker
        selected={value}
        dateFormat="dd/MM/yyyy"
        className="form-control"
        onChange={handleChange}
      />
    </div>
  );
};
