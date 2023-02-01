export type WithPageCount<T extends object> = {
  pageCount: number;
} & T;
