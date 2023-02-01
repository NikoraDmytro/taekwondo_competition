import React from "react";
import {
  fetchSingleCompetition,
  editCompetition,
  createCompetition,
} from "services/CompetitionsServices";

import { AppFormWithEdit } from "shared/components/AppForm";
import { DateField, InputField } from "shared/components/Inputs/InputField";
import { SelectInput } from "shared/components/Inputs/SelectInput/SelectInput";
import {
  competitionLevelOptions,
  cityOptions,
} from "shared/helpers/SelectOptions";
import { Competition } from "shared/types/Competition";
import { validate, validationSchema } from "./utils/competitionValidation";

export interface ICreateCompetition {
  city: string;
  endDate: Date;
  startDate: Date;
  weightingDate: Date;
  competitionName: string;
  competitionLevel: string;
}

export const CompetitionForm = () => {
  const getInitialValues = (
    editedCompetition: Competition | null
  ): ICreateCompetition => {
    if (editedCompetition) {
      return {
        ...editedCompetition,
        endDate: new Date(editedCompetition.endDate),
        startDate: new Date(editedCompetition.startDate),
        weightingDate: new Date(editedCompetition.weightingDate),
      };
    }

    return {
      city: "",
      endDate: new Date(),
      startDate: new Date(),
      weightingDate: new Date(),
      competitionName: "",
      competitionLevel: "",
    };
  };

  return (
    <AppFormWithEdit
      idParamName="competitionId"
      editRequest={(id: string, data: ICreateCompetition) =>
        editCompetition(+id, data)
      }
      validate={validate}
      validationSchema={validationSchema}
      request={createCompetition}
      getData={(id: string) => fetchSingleCompetition(+id)}
      initialValues={getInitialValues}
      title="Додати змагання"
      editTitle="Редагувати змагання"
    >
      <InputField label="Назва змагання" name="competitionName" type="text" />

      <SelectInput label="Місто проведення" name="city" options={cityOptions} />
      <SelectInput
        label="Рівень змагання"
        name="competitionLevel"
        options={competitionLevelOptions}
      />

      <DateField label="Дата зважування" name="weightingDate" />
      <DateField label="Дата початку" name="startDate" />
      <DateField label="Дата закінчення" name="endDate" />
    </AppFormWithEdit>
  );
};
