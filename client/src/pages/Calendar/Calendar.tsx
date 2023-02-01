import React, { useMemo } from "react";

import { fetchCompetitions } from "services/CompetitionsServices";
import { AxiosWrapper } from "shared/components/AxiosWrapper";
import { ControlPanel } from "shared/components/ControlPanel";
import { DataTable } from "shared/components/DataTable";
import { TableButtons } from "./../../shared/components/TableButtons/TableButtons";
import { SelectFilter } from "./../../shared/components/Inputs/SelectFilter/SelectFilter";
import { Link, useNavigate } from "react-router-dom";
import {
  cityOptions,
  competitionLevelOptions,
  competitionStatusOptions,
} from "shared/helpers/SelectOptions";
import { DateFilter } from "shared/components/Inputs/DateFilter";
import { useObjectSearchParams } from "shared/hooks/useObjectSearchParams";
import { parseDate } from "utils/parseDate";

export const Calendar = () => {
  const navigate = useNavigate();
  const params = useObjectSearchParams();

  return (
    <>
      <ControlPanel
        searchPlaceholder="Введіть назву змагання"
        filters={
          <>
            <SelectFilter
              name="status"
              label="Поточний статус"
              options={competitionStatusOptions}
            />
            <SelectFilter
              name="level"
              label="Рівень змагання"
              options={competitionLevelOptions}
            />
            <SelectFilter
              name="city"
              label="Місто проведення"
              options={cityOptions}
            />
            <DateFilter name="startDateFrom" label="Початок від" />
            <DateFilter name="startDateTo" label="Початок до" />
          </>
        }
        buttons={
          <Link to="form">
            <button>Додати</button>
          </Link>
        }
      />

      <AxiosWrapper
        fetchData={() => fetchCompetitions(params)}
        noDataText="Не знайдено жодного змагання!"
      >
        {({ data: competitions }) => {
          return (
            <>
              <DataTable
                tableColumns={[
                  { name: "id", label: "Id" },
                  {
                    name: "competitionName",
                    label: "Назва змагання",
                    sortable: true,
                  },
                  { name: "city", label: "Місто" },
                  { name: "status", label: "Поточний статус" },
                  {
                    name: "weightingDate",
                    label: "Дата зважування",
                    sortable: true,
                  },
                  { name: "startDate", label: "Дата початку", sortable: true },
                  { name: "endDate", label: "Дата закінчення", sortable: true },
                  { name: "level", label: "Рівень змагання" },
                  { name: "buttons", label: "" },
                ]}
                tableRows={competitions.map((competition) => ({
                  id: competition.competitionId.toString(),
                  competitionName: (
                    <p
                      style={{
                        width: 250,
                        textOverflow: "ellipsis",
                        overflow: "hidden",
                      }}
                    >
                      {competition.competitionName}
                    </p>
                  ),
                  city: competition.city,
                  status: competition.currentStatus,
                  weightingDate: parseDate(competition.weightingDate),
                  startDate: parseDate(competition.startDate),
                  endDate: parseDate(competition.endDate),
                  level: competition.competitionLevel,
                  buttons: (
                    <TableButtons
                      onEdit={() =>
                        navigate(`form/${competition.competitionId}`)
                      }
                      onDelete={() =>
                        navigate(`confirm/${competition.competitionId}`)
                      }
                    />
                  ),
                }))}
                onRowClick={(row) => navigate(row.id + "/competitors")}
              />
            </>
          );
        }}
      </AxiosWrapper>
    </>
  );
};
