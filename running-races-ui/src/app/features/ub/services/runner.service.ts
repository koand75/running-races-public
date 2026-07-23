import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Runner } from '../models/ub.models';

@Injectable({
    providedIn: 'root'
})
export class RunnerService {
    private http = inject(HttpClient);
    private baseUrl = 'https://localhost:7156/api';

    getByTeam(teamId: number): Observable<Runner[]> {
        return this.http.get<Runner[]>(`${this.baseUrl}/team/${teamId}/runner`);
    }

    getById(teamId: number, id: number): Observable<Runner> {
        return this.http.get<Runner>(`${this.baseUrl}/team/${teamId}/runner/${id}`);
    }

    create(teamId: number, runner: Runner): Observable<Runner> {
        return this.http.post<Runner>(`${this.baseUrl}/team/${teamId}/runner`, runner);
    }

    update(teamId: number, runner: Runner): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/team/${teamId}/runner/${runner.id}`, runner);
    }

    delete(teamId: number, id: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/team/${teamId}/runner/${id}`);
    }
}