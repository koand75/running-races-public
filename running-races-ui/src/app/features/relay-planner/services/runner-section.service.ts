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

  getByTeam(teamId: number): Observable<RunnerSection[]> {
    return this.http.get<RunnerSection[]>(`${environment.apiUrl}/team/${teamId}/assignments`);
  }

  saveAll(teamId: number, assignments: RunnerSection[]): Observable<void> {
    return this.http.put<void>(`${environment.apiUrl}/team/${teamId}/assignments`, assignments);
  }
}