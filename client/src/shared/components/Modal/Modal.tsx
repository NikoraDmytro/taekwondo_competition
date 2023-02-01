import React, { InputHTMLAttributes, useEffect, useState } from "react";
import ReactDOM from "react-dom";
import classNames from "classnames";

import styles from "./Modal.module.scss";

interface Props extends InputHTMLAttributes<HTMLDivElement> {
  visible: boolean;
  close: () => void;
}

export const Modal = ({ visible, close, className, ...props }: Props) => {
  const [show, setShow] = useState(false);

  useEffect(() => {
    if (visible) {
      setShow(true);
    }
  }, [visible]);

  if (!show) {
    return null;
  }

  const wrapperClassName = classNames(styles.modalWrapper, {
    [styles.visible]: visible,
  });

  const isWrapper = (target: EventTarget) => {
    const element = target as HTMLElement;

    return element.classList.contains(styles.modalWrapper);
  };

  const handleOutsideClick = (e: React.MouseEvent) => {
    if (isWrapper(e.target)) {
      close();
    }
  };

  const handleAnimationEnd = (e: React.AnimationEvent) => {
    if (!visible && isWrapper(e.target)) {
      setShow(false);
    }
  };

  return ReactDOM.createPortal(
    <div
      className={wrapperClassName}
      onMouseDown={handleOutsideClick}
      onAnimationEnd={handleAnimationEnd}
    >
      <div className={classNames(styles.modal, className)} {...props}>
        {props.children}

        <button className={styles.closeModalBtn} onClick={close}></button>
      </div>
    </div>,
    document.body
  );
};
