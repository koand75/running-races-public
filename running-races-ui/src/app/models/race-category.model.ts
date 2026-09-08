import { RaceType } from "../features/relay-planner/models/relay-planner.models";

export interface RaceCategory {
  id?: number;
  raceId: string;
  name: string;
  raceType: RaceType;
  measurement: MeasurementType;
  distance?: number | null;
  duration?: number | null;
  startDateTime?: string | null;
}

export enum MeasurementType {
  None = 'None',
  DistanceBased = 'DistanceBased',
  TimeBased = 'TimeBased'
}