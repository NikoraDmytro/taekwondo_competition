import { Belt, Sex } from "./Sportsman";

export interface Competitor {
  applicationNum: number;
  membershipCardNum: number;
  fullName: string;
  belt: Belt;
  sex: Sex;
  age: number;
  birthDate: Date;
  clubName: string;
  coachFullName: string;
  divisionName: string;
  weightingResult: number;
  sportsCategory: string;
}
