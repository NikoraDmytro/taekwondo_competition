import React, { InputHTMLAttributes, useState } from "react";
import { useField } from "formik";

import styles from "./InputField.module.scss";

interface Props
  extends InputHTMLAttributes<HTMLInputElement | HTMLTextAreaElement> {
  name: string;
  label: string;
}

export const PasswordInput = ({ label, name, ...props }: Props) => {
  const [field, meta] = useField(name);
  const [hidden, setHidden] = useState(true);

  return (
    <div className={styles.inputField}>
      <label htmlFor={name}>{label}</label>

      <div className={styles.passwordInput}>
        <input type={hidden ? "password" : "text"} {...field} {...props} />

        <div
          onClick={(e) => {
            e.stopPropagation();
            setHidden(!hidden);
          }}
          className={styles.showBtn}
        >
          {hidden ? "Показати" : "Сховати"}
        </div>
      </div>

      {meta.touched && meta.error ? (
        <div className={styles.inputError}>{meta.error}</div>
      ) : null}
    </div>
  );
};
