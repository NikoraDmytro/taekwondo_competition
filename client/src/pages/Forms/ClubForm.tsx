import React from "react";
import { createClub, editClub, fetchSingleClub } from "services/ClubsServices";

import { AppFormWithEdit } from "shared/components/AppForm";
import { InputField } from "shared/components/Inputs/InputField";
import { SelectInput } from "shared/components/Inputs/SelectInput/SelectInput";
import { cityOptions } from "shared/helpers/SelectOptions";
import { Club } from "shared/types/Club";

export interface ICreateClub {
  city: string;
  clubName: string;
  gymAddr: string;
}

export const ClubForm = () => {
  const getInitialValues = (editedClub: Club | null): ICreateClub => {
    if (editedClub) {
      return {
        ...editedClub,
      };
    }

    return {
      city: "",
      clubName: "",
      gymAddr: "",
    };
  };

  return (
    <AppFormWithEdit
      idParamName="clubId"
      editRequest={(id: string, data: ICreateClub) => editClub(+id, data)}
      request={createClub}
      getData={(id: string) => fetchSingleClub(+id)}
      initialValues={getInitialValues}
      title="Додати клуб"
      editTitle="Редагувати клуб"
    >
      <InputField label="Назва клубу" name="clubName" type="text" />

      <SelectInput label="Місто" name="city" options={cityOptions} />

      <InputField label="Адреса клубу" name="gymAddr" type="text" />
    </AppFormWithEdit>
  );
};
