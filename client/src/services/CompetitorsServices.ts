import http from "./index";
import { Competitor } from "../shared/types/Competitor";
import { WithPageCount } from "shared/types/Paginated";

export const fetchCompetitors = async (
  competitionId: number,
  params: Record<string, string>
): Promise<WithPageCount<{ competitors: Competitor[] }>> => {
  return await http.get(`/competitions/${competitionId}/competitors`, {
    params,
  });
};

export const createCompetitors = async (
  competitionId: number,
  membershipCardNums: string[]
) => {
  await http.post(`/competitors`, {
    membershipCardNums,
    competitionId,
  });
};
