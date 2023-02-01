import React from "react";

import classNames from "classnames";

import styles from "./ErrorComponent.module.scss";
import { isAxiosError } from "axios";

interface Props {
  error: Error;
  short?: boolean;
  inline?: boolean;
}

export const ErrorComponent = ({ error, inline, short }: Props) => {
  if (!error) return null;

  if (short) {
    if (isAxiosError(error)) {
      return (
        <p className={styles.shortError}>
          {error.response?.data.message ?? error.message}
        </p>
      );
    }

    return <p className={styles.shortError}>{error.message}</p>;
  }

  const errorClass = classNames([styles.error], {
    [styles.errorInline]: inline,
  });

  if (isAxiosError(error)) {
    let errorName;

    switch (error.status) {
      case 400:
        errorName = "Поганий запит!";
        break;
      case 404:
        errorName = "Не знайдено!";
        break;
      case 422:
        errorName = "Не підлягає обробці";
        break;
      default:
        errorName = "Внутрішня помилка сервера!";
    }

    let errMsg;

    if (error.response) {
      const errorData = error.response.data as any;

      if (typeof errorData == "object" && "message" in errorData) {
        errMsg = errorData.message;
      } else {
        errMsg = JSON.stringify(errorData);
      }
    } else {
      errMsg = error.message;
    }

    return (
      <div className={errorClass}>
        <h1>
          {error.status ?? "500"} {errorName}
        </h1>
        <h2>{errMsg}</h2>
      </div>
    );
  }

  return (
    <div className={errorClass}>
      <h1>{error.name ?? "500 Внутрішня помилка сервера"}</h1>
      <h2>{error.message ?? ""}</h2>
    </div>
  );
};
