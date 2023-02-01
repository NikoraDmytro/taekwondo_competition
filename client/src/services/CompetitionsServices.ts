import http from "./index";
import { Competition } from "../shared/types/Competition";
import { ICreateCompetition } from "pages/Forms/CompetitionForm";
import { Sportsman } from "shared/types/Sportsman";
import { WithPageCount } from "shared/types/Paginated";

export const fetchCompetitions = async (
  params: Record<string, string>
): Promise<Competition[]> => {
  return await http.get("/competitions", { params });
};

export const fetchSingleCompetition = async (
  id: number
): Promise<Competition> => {
  return await http.get(`/competitions/${id}`);
};

export const createCompetition = async (
  competition: ICreateCompetition
): Promise<Competition> => {
  return await http.post(`/competitions`, competition);
};

export const deleteCompetition = async (id: number): Promise<void> => {
  return await http.delete(`/competitions/${id}`);
};

export const editCompetition = async (
  id: number,
  competition: ICreateCompetition
): Promise<Competition> => {
  return await http.put(`/competitions/${id}`, competition);
};

export const fetchAvailableSportsmans = async (
  competitionId: number,
  params: Record<string, string>
): Promise<WithPageCount<{ sportsmans: Sportsman[] }>> => {
  return await http.get(`/competitions/${competitionId}/sportsmans`, {
    params,
  });
};
