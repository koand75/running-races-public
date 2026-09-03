import { RaceType } from "../features/relay-planner/models/relay-planner.models";

export interface Race {
  id: string;
  name: string;
  date: string;
  location: string;
  distance: number;
  isActive?: boolean;
  raceType?: RaceType;
}