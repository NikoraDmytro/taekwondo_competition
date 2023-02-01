import ReactDOM from "react-dom";
import { useEffect, useState } from "react";
import cl from "classnames";

import styles from "./SnackBar.module.scss";

interface Props {
  show: boolean;
  message: string;
  close: () => void;
  type: "success" | "error" | "info";
}

export const SnackBar = ({ show, close, message, type }: Props) => {
  const [timeOut, setTimeOut] = useState<NodeJS.Timeout>();

  useEffect(() => {
    clearTimeout(timeOut);

    if (show) {
      const timeout = setTimeout(() => {
        close();
      }, 6000);

      setTimeOut(timeout);
    }

    return () => timeOut && clearTimeout(timeOut);
  }, [show]);

  return ReactDOM.createPortal(
    <div
      className={cl(styles.snackBar, {
        [styles.show]: show,
        [styles.error]: type === "error",
        [styles.success]: type === "success",
      })}
    >
      {message}
    </div>,
    document.body
  );
};
