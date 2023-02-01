import { isAfter } from "date-fns";
import { FormikErrors } from "formik";
import * as yup from "yup";
import { ICreateCompetition } from "../CompetitionForm";

export const validationSchema = new yup.ObjectSchema({
  city: yup.string().required("Обов'язкове поле"),
  competitionName: yup.string().required("Обов'язкове поле"),
  competitionLevel: yup.string().required("Обов'язкове поле"),
  weightingDate: yup.date().required("Обов'язкове поле"),
  startDate: yup.date().required("Обов'язкове поле"),
  endDate: yup.date().required("Обов'язкове поле"),
});

export const validate = (data: ICreateCompetition) => {
  let errors: FormikErrors<ICreateCompetition> = {};

  if (isAfter(data.startDate, data.endDate)) {
    errors.endDate = "Не може бути раніше за дату початку!";
  }
  if (isAfter(data.weightingDate, data.startDate)) {
    errors.startDate = "Не може бути раніше за дату зважування!";
  }

  return errors;
};
