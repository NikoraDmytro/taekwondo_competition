import React, { useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Loader } from "shared/components/Loader";
import { Modal } from "../Modal/Modal";
import { useAxiosMutation } from "../../hooks/useAxiosMutation";
import { ErrorComponent } from "shared/components/ErrorComponent";
import { Formik, Form } from "formik";
import { PasswordInput } from "../Inputs/InputField/PasswordInput";

import styles from "./ConfirmModal.module.scss";

interface Props {
  entityName: string;
  deleteRequest: (id: string) => Promise<void>;
}

export const ConfirmModal = ({ entityName, deleteRequest }: Props) => {
  const navigate = useNavigate();
  const { competitionId } = useParams();
  const [isDeleted, setIsDeleted] = useState(false);
  const { loading, error, mutation } = useAxiosMutation(deleteRequest);

  if (!competitionId) return null;

  const handleSubmit = async () => {
    const isSuccess = await mutation(+competitionId);

    if (isSuccess) {
      setIsDeleted(true);
    }
  };

  const close = () => navigate(-1);

  return (
    <Modal visible={true} close={close}>
      {error && <ErrorComponent error={error} />}
      {isDeleted && (
        <div className={styles.modalBox}>
          <h1 className={styles.modalTitle}>
            {entityName[0].toUpperCase() + entityName.slice(1)} було видалено!
          </h1>

          <div className={styles.controlsBox}>
            <button className={styles.confirmBtn} onClick={close}>
              Продовжити
            </button>
          </div>
        </div>
      )}
      {!isDeleted && (
        <div className={styles.modalBox}>
          <h1 className={styles.modalTitle}>Видалити {entityName}?</h1>

          <Formik
            initialValues={{ password: "" }}
            validate={(data) => {
              if (!data.password) {
                return {
                  password: "Не вірний пароль!",
                };
              }
            }}
            onSubmit={() => handleSubmit()}
          >
            <Form>
              <PasswordInput
                name="password"
                className={styles.passwordInput}
                label="Для підтвердження введіть свій пароль"
              />

              <div className={styles.controlsBox}>
                <button className={styles.cancelBtn} onClick={close}>
                  Назад
                </button>

                <button className={styles.confirmBtn} type="submit">
                  {!loading ? "Підтвердити" : <Loader small />}
                </button>
              </div>
            </Form>
          </Formik>
        </div>
      )}
    </Modal>
  );
};
