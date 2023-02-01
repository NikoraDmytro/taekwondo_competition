import http from "./index";
import { Club } from "shared/types/Club";
import { ICreateClub } from "./../pages/Forms/ClubForm";
import { WithPageCount } from "shared/types/Paginated";
import { Sportsman } from "shared/types/Sportsman";

export const fetchClubs = async (
  params: Record<string, string>
): Promise<Club[]> => {
  return await http.get("/clubs", { params });
};

export const fetchSingleClub = async (id: number): Promise<Club> => {
  return await http.get(`/clubs/${id}`);
};

export const createClub = async (club: ICreateClub): Promise<Club> => {
  return await http.post(`/clubs`, club);
};

export const deleteClub = async (id: number): Promise<void> => {
  return await http.delete(`/clubs/${id}`);
};

export const editClub = async (
  id: number,
  club: ICreateClub
): Promise<Club> => {
  return await http.put(`/clubs/${id}`, club);
};

export const fetchClubSportsmans = async (
  clubId: number,
  params: Record<string, string>
): Promise<WithPageCount<{ sportsmans: Sportsman[] }>> => {
  return await http.get(`/clubs/${clubId}/sportsmans`, {
    params,
  });
};
