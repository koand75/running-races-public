import { RaceType } from "../features/relay-planner/models/relay-planner.models";

export interface Race {
  id: string;
  name: string;
  startDate: string;
  endDate?: string;
  location: string;
  distance: number;
  isActive?: boolean;
  raceType?: RaceType;
}
