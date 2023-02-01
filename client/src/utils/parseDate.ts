import { format } from "date-fns";

export const parseDate = (date: Date) => {
  return format(new Date(date), "dd.MM.yyyy");
};
