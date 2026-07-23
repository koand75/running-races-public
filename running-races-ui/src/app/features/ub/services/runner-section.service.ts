import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RunnerSection } from '../models/ub.models';

@Injectable({
  providedIn: 'root'
})
export class RunnerSectionService {
  private http = inject(HttpClient);
  private baseUrl = 'https://localhost:7156/api';

  getByTeam(teamId: number): Observable<RunnerSection[]> {
    return this.http.get<RunnerSection[]>(`${this.baseUrl}/team/${teamId}/assignments`);
  }

  saveAll(teamId: number, assignments: RunnerSection[]): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/team/${teamId}/assignments`, assignments);
  }
}