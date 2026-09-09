import { RaceType } from "../features/relay-planner/models/relay-planner.models";

export interface Race {
  id: string;
  name: string;
  startDate: string;
  endDate?: string;
  location: string;
  isActive?: boolean;
}

export interface RaceCategoryDropdownDto {
  raceId: string;
  raceName: string;
  categoryId: number;
  categoryName: string;
}
