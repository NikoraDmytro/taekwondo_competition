import React, { useCallback } from "react";
import { Route, Routes, useParams } from "react-router-dom";
import { fetchSingleCompetition } from "services/CompetitionsServices";
import { AxiosWrapper } from "shared/components/AxiosWrapper";
import { InnerNavBar } from "shared/components/InnerNavBar";
import { Competitors } from "./components/Competitors";

import styles from "./CompetitionInfo.module.scss";
import { AddCompetitors } from "./components/AddCompetitors";

export const CompetitionInfo = () => {
  const { competitionId } = useParams();

  const fetchData = useCallback(
    () => fetchSingleCompetition(+competitionId!),
    [competitionId]
  );

  if (!competitionId) return null;

  return (
    <AxiosWrapper
      fetchData={fetchData}
      noDataText={`Змагання з ID = ${competitionId} не знайдено!`}
    >
      {({ data: competition }) => (
        <div>
          <div className={styles.infoHeader}>
            <h1 className={styles.title}>{competition.competitionName}</h1>

            <div className={styles.statusBox}>
              <p>
                Статус: {competition.currentStatus}
                <div className={styles.chevron}></div>
              </p>
            </div>
          </div>

          <InnerNavBar
            innerLinks={[
              { to: "competitors", title: "Учасники" },
              {
                to: "dayangs",
                title: "Даянги",
              },
              {
                to: "divisions",
                title: "Дивізіони",
              },
              {
                to: "results",
                title: "Результати",
              },
            ]}
          />

          <Routes>
            <Route
              path="competitors"
              element={
                <Competitors competitionId={competition.competitionId} />
              }
            />
            <Route
              path="competitors/form"
              element={
                <AddCompetitors competitionId={competition.competitionId} />
              }
            />
          </Routes>
        </div>
      )}
    </AxiosWrapper>
  );
};
