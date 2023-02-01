import React from "react";
import { useNavigate } from "react-router-dom";
import { Form, Formik, FormikErrors, FormikHelpers } from "formik";
import * as yup from "yup";

import styles from "./AppForm.module.scss";
import { Loader } from "./../Loader/Loader";
import { useAxiosMutation } from "./../../hooks/useAxiosMutation";
import { ErrorComponent } from "../ErrorComponent";

export interface AppFormProps<T> {
  title?: string;
  initialValues: T;
  children: React.ReactNode;
  confirmMessage?: string;
  request: (data: T) => Promise<any>;
  validate?: (data: T) => void | object | Promise<FormikErrors<T>>;
  validationSchema?: yup.AnyObjectSchema;
}

export const AppForm = <T extends object>(props: AppFormProps<T>) => {
  const navigate = useNavigate();
  const { mutation, loading, error } = useAxiosMutation(props.request);

  const handleSubmit = async (
    values: T,
    { setSubmitting, resetForm }: FormikHelpers<T>
  ) => {
    if (props.confirmMessage) {
      const confirm = window.confirm(props.confirmMessage);

      if (!confirm) {
        setSubmitting(false);
        return;
      }
    }

    const success = await mutation(values);

    if (success) {
      resetForm();
      setSubmitting(false);
      navigate(-1);
    }
  };

  return (
    <>
      <h1 className={styles.formTitle}>{props.title}</h1>

      <Formik
        initialValues={props.initialValues}
        validate={props.validate}
        validationSchema={props.validationSchema}
        onSubmit={handleSubmit}
      >
        <Form className={styles.form}>
          {props.children}

          <div className={styles.lastRow}>
            {error && <ErrorComponent error={error} inline />}

            <button className={styles.submitBtn} type="submit">
              {!loading ? "Створити" : <Loader small />}
            </button>
          </div>
        </Form>
      </Formik>
    </>
  );
};
