import React from "react";
import { Route, Routes } from "react-router-dom";

import { Clubs } from "pages/Clubs/Clubs";
import { Header } from "./shared/components/Header";
import { Calendar } from "./pages/Calendar/Calendar";
import { LandingPage } from "pages/LandingPage/LandingPage";
import { CompetitionForm } from "pages/Forms/CompetitionForm";
import { CompetitionInfo } from "./pages/CompetitionInfo/CompetitionInfo";
import { deleteCompetition } from "services/CompetitionsServices";
import { ConfirmModal } from "shared/components/ConfirmModal";
import { deleteClub } from "services/ClubsServices";
import { ClubForm } from "pages/Forms/ClubForm";

function App() {
  return (
    <>
      <Header />

      <main style={{ padding: "0 30px" }}>
        <Routes>
          <Route path="/" element={<LandingPage />} />

          <Route path="/clubs/*">
            <Route index element={<Clubs />} />
            <Route path="form" element={<ClubForm />} />
            <Route path="form/:clubId" element={<ClubForm />} />

            <Route
              path="confirm/:competitionId"
              element={
                <>
                  <Clubs />
                  <ConfirmModal
                    entityName="клуб"
                    deleteRequest={(id) => deleteClub(+id)}
                  />
                </>
              }
            />
          </Route>

          <Route path="/calendar/*">
            <Route index element={<Calendar />} />
            <Route path="form" element={<CompetitionForm />} />
            <Route path="form/:competitionId" element={<CompetitionForm />} />
            <Route path=":competitionId/*" element={<CompetitionInfo />} />

            <Route
              path="confirm/:competitionId"
              element={
                <>
                  <Calendar />
                  <ConfirmModal
                    entityName="змагання"
                    deleteRequest={(id) => deleteCompetition(+id)}
                  />
                </>
              }
            />
          </Route>
        </Routes>
      </main>
    </>
  );
}

export default App;
