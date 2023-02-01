import React from "react";
import { Link, useNavigate } from "react-router-dom";

import { fetchClubs } from "services/ClubsServices";
import { AxiosWrapper } from "shared/components/AxiosWrapper";
import { ControlPanel } from "shared/components/ControlPanel";
import { DataTable } from "shared/components/DataTable";
import { SelectFilter } from "shared/components/Inputs/SelectFilter";
import { cityOptions } from "shared/helpers/SelectOptions";
import { useObjectSearchParams } from "shared/hooks/useObjectSearchParams";

import styles from "./Clubs.module.scss";
import { TableButtons } from "shared/components/TableButtons/TableButtons";

export const Clubs = () => {
  const navigate = useNavigate();
  const params = useObjectSearchParams();

  return (
    <>
      <ControlPanel
        searchPlaceholder="Введіть назву клубу"
        filters={
          <SelectFilter name="city" label="Місто" options={cityOptions} />
        }
        buttons={
          <Link to="form">
            <button>Додати</button>
          </Link>
        }
      />

      <AxiosWrapper
        fetchData={() => fetchClubs(params)}
        noDataText="Не знайдено жодного клубу!"
      >
        {({ data: clubs }) => {
          return (
            <>
              <DataTable
                tableColumns={[
                  { name: "id", label: "Id" },
                  {
                    name: "clubName",
                    label: "Назва клубу",
                    sortable: true,
                  },
                  {
                    name: "gymAddr",
                    label: "Адреса",
                    sortable: true,
                  },
                  {
                    name: "city",
                    label: "Місто",
                    sortable: true,
                  },
                  { name: "buttons", label: "" },
                ]}
                tableRows={clubs.map((club) => ({
                  id: club.clubId.toString(),
                  clubName: club.clubName,
                  city: club.city,
                  gymAddr: club.gymAddr,
                  buttons: (
                    <TableButtons
                      onEdit={() => navigate(`form/${club.clubId}`)}
                      onDelete={() => navigate(`confirm/${club.clubId}`)}
                    />
                  ),
                }))}
                onRowClick={(row) => navigate(row.id + "/sportsmans")}
              />
            </>
          );
        }}
      </AxiosWrapper>
    </>
  );
};
