import React from "react";
import { useState } from "react";
import { useEffect } from "react";
import ReactPaginate from "react-paginate";
import { useSearchParams } from "react-router-dom";

import styles from "./Pagination.module.scss";

interface Props {
  totalPagesCount: number;
}

export const Pagination = (props: Props) => {
  const [currentPage, setCurrentPage] = useState(1);
  const [searchParams, setSearchParams] = useSearchParams();

  useEffect(() => {
    const pageNumber = searchParams.get("PageNumber") ?? 1;

    setCurrentPage(+pageNumber);
  }, [searchParams]);

  const onPageChange = ({ selected }: { selected: number }) => {
    searchParams.set("PageNumber", String(selected + 1));
    setSearchParams(searchParams);
  };

  return props.totalPagesCount > 1 ? (
    <ReactPaginate
      breakLabel="..."
      nextLabel=">"
      previousLabel="<"
      forcePage={currentPage - 1}
      activeClassName={styles.active}
      disabledClassName={styles.disabled}
      pageClassName={styles.page}
      nextClassName={styles.nextLabel}
      previousClassName={styles.prevLabel}
      breakClassName={styles.breakLabel}
      containerClassName={styles.container}
      onPageChange={onPageChange}
      pageRangeDisplayed={5}
      pageCount={props.totalPagesCount}
    />
  ) : null;
};
