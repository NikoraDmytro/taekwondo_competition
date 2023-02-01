import React, { useCallback, useState } from "react";
import { useNavigate } from "react-router-dom";
import { ControlPanel } from "shared/components/ControlPanel";
import { DataTable } from "shared/components/DataTable";
import { SelectFilter } from "shared/components/Inputs/SelectFilter/SelectFilter";
import {
  cityOptions,
  competitionLevelOptions,
  competitionStatusOptions,
} from "shared/helpers/SelectOptions";
import { DateFilter } from "shared/components/Inputs/DateFilter";
import { fetchAvailableSportsmans } from "services/CompetitionsServices";
import { AxiosWrapper } from "shared/components/AxiosWrapper";
import { useObjectSearchParams } from "shared/hooks/useObjectSearchParams";
import { parseDate } from "utils/parseDate";
import { Pagination } from "shared/components/Pagination";

import styles from "./AddCompetitors.module.scss";
import { useAxiosMutation } from "shared/hooks/useAxiosMutation";
import { createCompetitors } from "services/CompetitorsServices";
import { Loader } from "shared/components/Loader";
import { SnackBar } from "shared/components/SnackBar";

interface Props {
  competitionId: number;
}

export const AddCompetitors = ({ competitionId }: Props) => {
  const navigate = useNavigate();
  const params = useObjectSearchParams();
  const [selected, setSelected] = useState<string[]>([]);
  const [showSnack, setShowSnack] = useState(false);
  const { mutation, loading, error } = useAxiosMutation(createCompetitors);

  const handleClick = async () => {
    const success = await mutation(competitionId, selected);

    if (success) {
      navigate(-1);
    } else {
      setShowSnack(true);
    }
  };

  const fetchData = useCallback(
    () => fetchAvailableSportsmans(competitionId, params),
    [competitionId, params]
  );

  return (
    <>
      <h1 className={styles.title}>Додати учасників</h1>

      <ControlPanel
        searchPlaceholder="Введіть ПІБ спортсмена"
        filters={
          <>
            <SelectFilter
              name="belt"
              label="Пояс"
              options={competitionStatusOptions}
            />
            <SelectFilter
              name="sex"
              label="Стать"
              options={competitionLevelOptions}
            />
            <SelectFilter
              name="clubId"
              label="Назва клубу"
              options={cityOptions}
            />
            <SelectFilter
              name="coachId"
              label="ПІБ тренера"
              options={cityOptions}
            />
            <SelectFilter
              name="divisionName"
              label="Дивізіон"
              options={cityOptions}
            />
            <DateFilter name="ageMin" label="Вік від" />
            <DateFilter name="ageMax" label="Вік до" />
          </>
        }
        buttons={
          <button onClick={handleClick}>
            {!loading ? "Зареєструвати" : <Loader small />}
          </button>
        }
      />
      <AxiosWrapper
        checkLength={(data) => data.sportsmans.length !== 0}
        fetchData={fetchData}
        noDataText="Не знайдено жодного учасника!"
      >
        {({ data: { sportsmans, pageCount } }) => {
          return (
            <>
              <Pagination totalPagesCount={pageCount} />

              <DataTable
                selectable={true}
                selected={selected}
                onSelect={(id) =>
                  selected.includes(id)
                    ? setSelected(selected.filter((val) => val !== id))
                    : setSelected([...selected, id])
                }
                onSelectAll={() => true}
                tableColumns={[
                  {
                    name: "id",
                    label: "Номер членського квитка",
                  },
                  { name: "fullName", label: "ПІБ", sortable: true },
                  { name: "belt", label: "Пояс", sortable: true },
                  { name: "sex", label: "Стать" },
                  {
                    name: "birthDate",
                    label: "Дата народження",
                  },
                  { name: "clubName", label: "Назва клубу", sortable: true },
                  {
                    name: "coachFullName",
                    label: "ПІБ тренера",
                    sortable: true,
                  },
                ]}
                tableRows={sportsmans.map((sportsman) => ({
                  id: sportsman.membershipCardNum.toString(),
                  fullName: sportsman.fullName,
                  belt: sportsman.belt,
                  sex: sportsman.sex,
                  clubName: sportsman.clubName,
                  coachFullName: sportsman.coachFullName,
                  birthDate: parseDate(sportsman.birthDate),
                }))}
              />
            </>
          );
        }}
      </AxiosWrapper>
      <SnackBar
        show={showSnack}
        close={() => setShowSnack(false)}
        message={
          error
            ? (error as any).response?.data?.message || (error as Error).message
            : ""
        }
        type="error"
      />
    </>
  );
};
