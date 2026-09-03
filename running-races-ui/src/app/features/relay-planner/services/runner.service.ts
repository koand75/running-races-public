import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Runner } from '../models/relay-planner.models';
import {environment } from '../../../../../src/environments/environment'

@Injectable({
    providedIn: 'root'
})
export class RunnerService {
    private http = inject(HttpClient);

    getByTeam(teamId: number): Observable<Runner[]> {
        return this.http.get<Runner[]>(`${environment.apiUrl}/team/${teamId}/runner`);
    }

    getById(teamId: number, id: number): Observable<Runner> {
        return this.http.get<Runner>(`${environment.apiUrl}/team/${teamId}/runner/${id}`);
    }

    create(teamId: number, runner: Runner): Observable<Runner> {
        return this.http.post<Runner>(`${environment.apiUrl}/team/${teamId}/runner`, runner);
    }

    update(teamId: number, runner: Runner): Observable<void> {
        return this.http.put<void>(`${environment.apiUrl}/team/${teamId}/runner/${runner.id}`, runner);
    }

    delete(teamId: number, id: number): Observable<void> {
        return this.http.delete<void>(`${environment.apiUrl}/team/${teamId}/runner/${id}`);
    }
}