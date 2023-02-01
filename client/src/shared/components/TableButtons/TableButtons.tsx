import React from "react";

import styles from "./TableButtons.module.scss";

interface Props {
  extra?: {
    title: string;
    onClick: (e: React.MouseEvent<HTMLButtonElement>) => void;
  }[];
  onEdit?: () => void;
  onDelete?: () => void;
}

export const TableButtons = (props: Props) => {
  const { onEdit, onDelete, extra } = props;

  const editBtnClick = async (e: React.MouseEvent) => {
    e.stopPropagation();

    onEdit!();
  };

  const deleteBtnClick = async (e: React.MouseEvent) => {
    e.stopPropagation();

    onDelete!();
  };

  return (
    <div className={styles.tableButtons}>
      {extra?.map(({ title, onClick }) => (
        <button onClick={onClick}>{title}</button>
      ))}
      {onEdit && (
        <button onClick={editBtnClick} className={styles.edit}>
          Редагувати
        </button>
      )}
      {onDelete && (
        <button onClick={deleteBtnClick} className={styles.delete}>
          Видалити
        </button>
      )}
    </div>
  );
};
