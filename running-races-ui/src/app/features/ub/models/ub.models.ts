export interface WayPoint {
  id: number;
  name: string;
  lat?: number | null;
  lng?: number | null;
}

export interface Section {
  id: number;
  name: string;
  distance: number;
  description?: string;
  order: number;
  startWayPointId?: number | null;
  endWayPointId?: number | null;
  startWayPoint?: WayPoint | null;
  endWayPoint?: WayPoint | null;
}

export interface Team {
  id: number;
  name: string;
  year: number;
  runners?: Runner[];
  startTime?: string | null;
}

export interface Runner {
  id: number;
  teamId: number;
  name: string;
  email?: string;
  basePace: number;
  notes?: string;
}

export interface RunnerSection {
  id?: number;
  sectionId: number;
  runnerId: number;
  customPace: number;
  section?: Section;
  runner?: Runner;
}