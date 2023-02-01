import React from "react";
import classNames from "classnames";
import DatePicker from "react-datepicker";
import { useField, useFormikContext } from "formik";
import "react-datepicker/dist/react-datepicker.css";

import styles from "./InputField.module.scss";

interface Props {
  name: string;
  label: string;
}

export const DateField = ({ label, name }: Props) => {
  const { setFieldValue } = useFormikContext();
  const [field, meta] = useField(name);

  const className = classNames(styles.inputField);

  return (
    <div className={className}>
      <label htmlFor={name}>{label}</label>

      <DatePicker
        name={name}
        selected={field.value}
        dateFormat="dd/MM/yyyy"
        className="form-control"
        onChange={(date: any) => setFieldValue(name, date)}
      />

      {meta.touched && meta.error ? (
        <div className={styles.inputError}>{meta.error}</div>
      ) : null}
    </div>
  );
};
