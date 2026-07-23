export interface RaceSearchModel {
  searchTerm?: string;
  searchField?: string;
  sortBy?: string;
  sortDirection?: string;
  isActive?: boolean;
  page?: number;
  pageSize?: number;
}