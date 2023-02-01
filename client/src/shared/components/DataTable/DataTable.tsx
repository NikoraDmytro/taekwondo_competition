import { useSearchParams } from "react-router-dom";
import { CheckBox } from "../Checkbox/CheckBox";
import cl from "classnames";

import sortIcon from "../../assets/sort.png";
import sortIconAsc from "../../assets/sortAsc.png";
import sortIconDesc from "../../assets/sortDesc.png";

import styles from "./DataTable.module.scss";

interface WithId {
  id: string;
  [key: string]: string | React.ReactNode;
}

export interface TableColumn<T extends WithId> {
  name: keyof T;
  label: string;
  sortable?: boolean;
}

interface TableProps<T extends WithId> {
  tableRows: T[];
  selected?: string[];
  selectable?: boolean;
  onSelectAll?: () => void;
  onRowClick?: (row: T) => void;
  tableColumns: TableColumn<T>[];
  onSelect?: (id: string) => void;
}

export const DataTable = <T extends WithId>(props: TableProps<T>) => {
  const {
    tableColumns,
    tableRows,
    onRowClick,
    selected,
    onSelect,
    onSelectAll,
  } = props;

  const [searchParams, setSearchParams] = useSearchParams();

  const orderBy = searchParams.get("orderBy");
  const orderField = orderBy?.split(" ")[0] ?? "";
  const orderDirection = orderBy?.split(" ")[1] ?? "ASC";

  const handleSort = (field: keyof T) => {
    const direction = orderDirection === "ASC" ? "DESC" : "ASC";

    searchParams.set("orderBy", `${field as string} ${direction}`);
    setSearchParams(searchParams);
  };

  const isSelected = (id: string) => selected?.indexOf(id) !== -1;

  return (
    <table
      className={cl(styles.dataTable, {
        [styles.selectable]: !!onRowClick,
      })}
    >
      <thead>
        <tr>
          {selected && onSelectAll && (
            <th>
              <CheckBox
                toggle={onSelectAll}
                id={"selectAll" + new Date()}
                checked={
                  tableRows.length > 0 && selected.length === tableRows.length
                }
              />
            </th>
          )}
          {tableColumns.map(({ name, label, sortable }) => (
            <th
              key={String(name)}
              className={cl(styles.headCells, {
                [styles.sortable]: sortable,
              })}
            >
              {label}
              {sortable && (
                <button
                  onClick={() => handleSort(name)}
                  className={styles.sortBtn}
                >
                  <img
                    src={
                      orderField === name
                        ? orderDirection === "ASC"
                          ? sortIconAsc
                          : sortIconDesc
                        : sortIcon
                    }
                    alt={orderDirection}
                  />
                </button>
              )}
            </th>
          ))}
        </tr>
      </thead>
      <tbody>
        {tableRows.map((row) => (
          <tr key={row.id} className={styles.tableRow}>
            {selected && onSelect && (
              <td>
                <CheckBox
                  id={`checkbox${row.id}`}
                  checked={isSelected(row.id)}
                  toggle={() => onSelect(row.id)}
                />
              </td>
            )}
            {tableColumns.map(({ name }) => (
              <td
                key={String(name)}
                onClick={() => {
                  onRowClick && onRowClick(row);
                }}
                className={styles.tableCells}
              >
                {row[name]}
              </td>
            ))}
          </tr>
        ))}
      </tbody>
    </table>
  );
};
