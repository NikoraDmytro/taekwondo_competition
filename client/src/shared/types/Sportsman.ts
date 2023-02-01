export interface Sportsman {
  membershipCardNum: number;
  sex: Sex;
  belt: Belt;
  photo: string;
  birthDate: Date;
  clubName: string;
  fullName: string;
  coachFullName: string;
  sportsCategory: string;
}

export type Role = "Admin" | "Regular";

export type Sex = "Ч" | "Ж";

export type Belt =
  | "10"
  | "9"
  | "8"
  | "7"
  | "6"
  | "5"
  | "4"
  | "3"
  | "2"
  | "1"
  | "I"
  | "II"
  | "III"
  | "IV"
  | "V"
  | "VI"
  | "VII"
  | "VIII"
  | "IX";
