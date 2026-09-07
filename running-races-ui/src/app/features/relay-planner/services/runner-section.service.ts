import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RunnerSection } from '../models/relay-planner.models';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RunnerSectionService {
  private http = inject(HttpClient);
 
  getByTeam(raceId: string, teamId: number): Observable<RunnerSection[]> {
    return this.http.get<RunnerSection[]>(`${environment.apiUrl}/race/${raceId}/team/${teamId}/assignments`);
  }

  saveAll(raceId: string, teamId: number, assignments: RunnerSection[]): Observable<void> {
    return this.http.put<void>(`${environment.apiUrl}/race/${raceId}/team/${teamId}/assignments`, assignments);
  }
}