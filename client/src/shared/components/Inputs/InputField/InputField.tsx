import React, { InputHTMLAttributes } from "react";
import { useField } from "formik";

import styles from "./InputField.module.scss";

interface Props
  extends InputHTMLAttributes<HTMLInputElement | HTMLTextAreaElement> {
  name: string;
  label: string;
}

export const InputField = ({ label, name, ...props }: Props) => {
  const [field, meta] = useField(name);

  return (
    <div className={styles.inputField}>
      <label htmlFor={name}>{label}</label>

      <input {...field} {...props} />

      {meta.touched && meta.error ? (
        <div className={styles.inputError}>{meta.error}</div>
      ) : null}
    </div>
  );
};
