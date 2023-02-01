export interface Competition {
  competitionId: number;
  competitionName: string;
  weightingDate: Date;
  startDate: Date;
  endDate: Date;
  city: string;
  competitionLevel: string;
  currentStatus: CompetitionStatus;
}

export type CompetitionStatus =
  | "очікується"
  | "прийом заявок"
  | "прийом заявок закінчено"
  | "в процесі"
  | "закінчено";
