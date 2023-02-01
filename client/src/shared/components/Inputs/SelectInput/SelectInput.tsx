import { useField } from "formik";
import React, { InputHTMLAttributes } from "react";

import { Option } from "shared/types/Option";
import { DropDown } from "../DropDown";

import styles from "./SelectInput.module.scss";

interface Props extends InputHTMLAttributes<HTMLInputElement> {
  name: string;
  label: string;
  options: Option[];
}

export const SelectInput = ({ label, options, name, ...props }: Props) => {
  const [field, meta, helpers] = useField(name);

  const handleSelect = (option: Option) => {
    helpers.setValue(option.value);
  };

  return (
    <div className={styles.selectInput}>
      <label>{label}</label>

      <DropDown type="text" {...field} {...props}>
        {options
          .filter((opt) => opt.label.includes(field.value))
          .map((option) => (
            <li key={option.label} onClick={() => handleSelect(option)}>
              {option.label}
            </li>
          ))}
      </DropDown>

      {meta.touched && meta.error ? (
        <div className={styles.inputError}>{meta.error}</div>
      ) : null}
    </div>
  );
};
