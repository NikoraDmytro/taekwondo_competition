import React, { useCallback } from "react";

import { AxiosWrapper } from "shared/components/AxiosWrapper";
import { ControlPanel } from "shared/components/ControlPanel";
import { DataTable } from "shared/components/DataTable";
import { Link, useNavigate } from "react-router-dom";
import { cityOptions, beltOptions } from "shared/helpers/SelectOptions";
import { useObjectSearchParams } from "shared/hooks/useObjectSearchParams";
import { parseDate } from "utils/parseDate";
import {
  SelectFilter,
  SelectFilterAsync,
} from "shared/components/Inputs/SelectFilter";
import { TableButtons } from "shared/components/TableButtons/TableButtons";
import { fetchCompetitors } from "./../../../../services/CompetitorsServices";
import { fetchClubs } from "./../../../../services/ClubsServices";
import { Pagination } from "shared/components/Pagination";
import { NumberFilter } from "shared/components/Inputs/NumberFilter";

interface Props {
  competitionId: number;
}

export const Competitors = ({ competitionId }: Props) => {
  const navigate = useNavigate();
  const params = useObjectSearchParams();

  const fetchData = useCallback(
    () => fetchCompetitors(competitionId, params),
    [competitionId, params]
  );

  return (
    <>
      <ControlPanel
        searchPlaceholder="Введіть ПІБ спортсмена"
        filters={
          <>
            <SelectFilter name="belt" label="Пояс" options={beltOptions} />
            <SelectFilter
              name="sex"
              label="Стать"
              options={[
                {
                  label: "Ч",
                  value: "Ч",
                },
                { label: "Ж", value: "Ж" },
              ]}
            />
            <SelectFilterAsync
              name="clubId"
              label="Назва клубу"
              request={async (input) => {
                const clubs = await fetchClubs({ search: input });

                return clubs.map((club) => ({
                  label: club.clubName,
                  value: club.clubId.toString(),
                }));
              }}
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
            <NumberFilter name="minAge" label="Вік від" />
            <NumberFilter name="maxAge" label="Вік до" />
          </>
        }
        buttons={
          <>
            <Link to="form">
              <button>Додати</button>
            </Link>
            <button>Друк</button>
            <button disabled>Зважування</button>
          </>
        }
      />

      <AxiosWrapper
        checkLength={(data) => data.competitors.length !== 0}
        fetchData={fetchData}
        noDataText="Не знайдено жодного учасника!"
      >
        {({ data: { competitors, pageCount } }) => {
          return (
            <>
              <Pagination totalPagesCount={pageCount} />

              <DataTable
                tableColumns={[
                  { name: "id", label: "Номер заявки" },
                  {
                    name: "membershipCardNum",
                    label: "Номер членського квитка",
                  },
                  { name: "fullName", label: "ПІБ", sortable: true },
                  { name: "belt", label: "Пояс", sortable: true },
                  { name: "sex", label: "Стать" },
                  { name: "age", label: "Вік", sortable: true },
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
                  { name: "weightingResult", label: "Вага", sortable: true },
                  {
                    name: "divisionName",
                    label: "Назва дивізіону",
                    sortable: true,
                  },
                  { name: "buttons", label: "" },
                ]}
                tableRows={competitors.map((competitor) => ({
                  id: "№" + competitor.applicationNum,
                  membershipCardNum: competitor.membershipCardNum.toString(),
                  fullName: competitor.fullName,
                  belt: competitor.belt,
                  sex: competitor.sex,
                  age: competitor.age.toString(),
                  birthDate: parseDate(competitor.birthDate),
                  clubName: competitor.clubName,
                  coachFullName: competitor.coachFullName,
                  divisionName: competitor.divisionName,
                  weightingResult: competitor.weightingResult,
                  buttons: (
                    <TableButtons
                      onDelete={() =>
                        navigate(`confirm/${competitor.applicationNum}`)
                      }
                    />
                  ),
                }))}
              />
            </>
          );
        }}
      </AxiosWrapper>
    </>
  );
};
