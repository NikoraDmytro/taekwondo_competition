import React from "react";
import { useParams } from "react-router-dom";
import { AxiosWrapper } from "../AxiosWrapper";
import { AppForm, AppFormProps } from "./AppForm";

interface Props<TForm, TGet>
  extends Omit<AppFormProps<TForm>, "initialValues"> {
  idParamName: string;
  editTitle?: string;
  editConfirmMessage?: string;
  getData: (id: string) => Promise<TGet>;
  editRequest: (id: string, data: TForm) => Promise<any>;
  initialValues: (data: TGet | null) => TForm;
}

export const AppFormWithEdit = <TForm extends object, TGet extends object>(
  props: Props<TForm, TGet>
) => {
  const { [props.idParamName]: id } = useParams();

  if (!id) {
    return (
      <AppForm
        {...props}
        title={props.title}
        request={props.request}
        confirmMessage={props.confirmMessage}
        initialValues={props.initialValues(null)}
      >
        {props.children}
      </AppForm>
    );
  }

  return (
    <AxiosWrapper
      fetchData={() => props.getData(id!)}
      noDataText={`За ID = ${id} нічого не знайдено!`}
    >
      {({ data }) => (
        <AppForm
          {...props}
          title={props.editTitle}
          request={(data) => props.editRequest(id!, data)}
          confirmMessage={props.editConfirmMessage}
          initialValues={props.initialValues(data)}
        >
          {props.children}
        </AppForm>
      )}
    </AxiosWrapper>
  );
};
